using HarmonyLib;
using RimWorld;
using Verse;

namespace VarietyMattersDT
{
    // Token: 0x0200000D RID: 13
    [HarmonyPatch(typeof(JobGiver_PackFood), "IsGoodPackableFoodFor")]
    public static class PackRawFoods
    {
        // Token: 0x06000030 RID: 48 RVA: 0x00003A38 File Offset: 0x00001C38
        private static bool Prefix(ref bool __result, Thing food, Pawn forPawn)
        {
            var foodsWithoutTable = ModSettings_VMDT.foodsWithoutTable;
            bool result;
            if (foodsWithoutTable)
            {
                __result = food.def.IsNutritionGivingIngestible && food.def.EverHaulable &&
                           food.def.ingestible.preferability >= FoodPreferability.RawTasty &&
                           forPawn.WillEat(food, null, false);
                result = false;
            }
            else
            {
                result = true;
            }

            return result;
        }
    }
}