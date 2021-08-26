using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VarietyMattersDT
{
    // Token: 0x02000004 RID: 4
    [HarmonyPatch]
    public class CookingQuality
    {
        // Token: 0x0400000D RID: 13
        public static List<ThoughtDef> poorlyCookedThoughts = new List<ThoughtDef>
        {
            ThoughtDef.Named("VMDT_Burnt"),
            ThoughtDef.Named("VMDT_Overcooked"),
            ThoughtDef.Named("VMDT_Overseasoned"),
            ThoughtDef.Named("VMDT_SmallPortions"),
            ThoughtDef.Named("VMDT_Undercooked"),
            ThoughtDef.Named("VMDT_Unforgettable"),
            ThoughtDef.Named("VMDT_Underseasoned"),
            ThoughtDef.Named("VMDT_LargePortions")
        };

        // Token: 0x06000012 RID: 18 RVA: 0x00002730 File Offset: 0x00000930
        [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
        [HarmonyPostfix]
        public static void Postfix(Thing __result, RecipeDef recipeDef, Pawn worker)
        {
            if (!ModSettings_VMDT.cookingQuality)
            {
                return;
            }

            var compFreshness = __result.TryGetComp<CompFreshness>();
            if (compFreshness == null || __result.def.ingestible == null)
            {
                return;
            }

            var preferability = __result.def.ingestible.preferability;
            var level = worker.skills.GetSkill(SkillDefOf.Cooking).Level;
            var num = 0f;
            switch (preferability)
            {
                case FoodPreferability.MealAwful:
                case FoodPreferability.MealSimple:
                    num = (float)preferability;
                    break;
                case FoodPreferability.MealFine:
                    num = (float)preferability * 1.5f;
                    break;
                case FoodPreferability.MealLavish:
                    num = (float)preferability * 2f;
                    break;
            }

            var num2 = Math.Max(0f, (num - level) * (float)preferability / 200f);
            var num3 = Rand.Range(0f, 1f);
            if (num3 < num2)
            {
                compFreshness.badChance = 1f;
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002828 File Offset: 0x00000A28
        public static void PoorlyCookedEffects(Pawn ingester, Thing thing)
        {
            var index = Rand.Range(0, poorlyCookedThoughts.Count - 1);
            var thoughtDef = poorlyCookedThoughts[index];
            if (thoughtDef == ThoughtDef.Named("VMDT_SmallPortions"))
            {
                thing.TryGetComp<CompFreshness>().smallPortion = true;
            }

            if (thoughtDef == ThoughtDef.Named("VMDT_LargePortions"))
            {
                var curLevelPercentage = ingester.needs.food.CurLevelPercentage;
                var firstHediffOfDef = ingester.health.hediffSet.GetFirstHediffOfDef(DefOf_VMDT.VMDT_Overate);
                if (firstHediffOfDef == null)
                {
                    ingester.health.AddHediff(HediffMaker.MakeHediff(DefOf_VMDT.VMDT_Overate, ingester));
                    firstHediffOfDef = ingester.health.hediffSet.GetFirstHediffOfDef(DefOf_VMDT.VMDT_Overate);
                }

                firstHediffOfDef.Severity += curLevelPercentage;
            }

            ingester.needs.mood.thoughts.memories.TryGainMemory(thoughtDef);
        }

        // Token: 0x06000014 RID: 20 RVA: 0x0000292C File Offset: 0x00000B2C
        [HarmonyPatch(typeof(Thing), "Ingested")]
        [HarmonyPostfix]
        public static void SmallPortions(ref float __result, Thing __instance)
        {
            var compFreshness = __instance.TryGetComp<CompFreshness>();
            if (compFreshness is { smallPortion: true })
            {
                __result -= 0.3f;
            }
        }
    }
}