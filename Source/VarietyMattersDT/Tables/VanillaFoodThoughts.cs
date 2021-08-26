using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VarietyMattersDT.Tables
{
    // Token: 0x02000010 RID: 16
    [HarmonyPatch]
    public class VanillaFoodThoughts
    {
        // Token: 0x06000036 RID: 54 RVA: 0x00003B94 File Offset: 0x00001D94
        [HarmonyPatch(typeof(MemoryThoughtHandler), "TryGainMemory", typeof(ThoughtDef), typeof(Pawn), typeof(Precept))]
        public static bool Prefix(ref ThoughtDef def, Pawn ___pawn)
        {
            if (def == ThoughtDefOf.AteWithoutTable)
            {
                var foodsWithoutTable = ModSettings_VMDT.foodsWithoutTable;
                if (foodsWithoutTable)
                {
                    ThingDef thingDef = null;
                    if (___pawn.CurJob.GetTarget(TargetIndex.A) != null)
                    {
                        thingDef = ___pawn.CurJob.GetTarget(TargetIndex.A).Thing.def;
                    }

                    if (thingDef != null && thingDef.HasModExtension<DefMod_VMDT>())
                    {
                        return false;
                    }
                }

                var useTableThought = ModSettings_VMDT.useTableThought;
                if (useTableThought)
                {
                    def = DefOf_VMDT.VMDT_AteWithoutTable;
                }
            }

            if (def == ThoughtDefOf.AteLavishMeal && ModSettings_VMDT.memorableLavish)
            {
                def = DefOf_VMDT.VMDT_AteLavishMeal;
            }

            return true;
        }
    }
}