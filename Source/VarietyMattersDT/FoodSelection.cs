using HarmonyLib;
using RimWorld;
using Verse;

namespace VarietyMattersDT
{
    // Token: 0x02000009 RID: 9
    [HarmonyPatch]
    public class FoodSelection
    {
        // Token: 0x17000005 RID: 5
        // (get) Token: 0x06000020 RID: 32 RVA: 0x000031CC File Offset: 0x000013CC
        // (set) Token: 0x06000021 RID: 33 RVA: 0x000031D3 File Offset: 0x000013D3
        private static Pawn Packer { get; set; }

        // Token: 0x06000022 RID: 34 RVA: 0x000031DB File Offset: 0x000013DB
        [HarmonyPatch(typeof(JobGiver_PackFood), "TryGiveJob")]
        [HarmonyPrefix]
        public static void TrackPawn(Pawn pawn)
        {
            Packer = pawn;
        }

        // Token: 0x06000023 RID: 35 RVA: 0x000031E5 File Offset: 0x000013E5
        [HarmonyPatch(typeof(JobGiver_PackFood), "TryGiveJob")]
        [HarmonyPostfix]
        public static void StopTrackingPawn(Pawn pawn)
        {
            Packer = null;
        }

        // Token: 0x06000024 RID: 36 RVA: 0x000031F0 File Offset: 0x000013F0
        [HarmonyPatch(typeof(FoodUtility), "FoodOptimality")]
        public static void Postfix(ref float __result, Pawn eater, Thing foodSource, ThingDef foodDef, float dist)
        {
            if (ModSettings_VMDT.foodsWithoutTable && Packer != null && Packer == eater &&
                foodDef.HasModExtension<DefMod_VMDT>())
            {
                __result += foodDef.GetModExtension<DefMod_VMDT>().packBonus;
            }

            if (ModSettings_VMDT.preferSpoiling && !(Packer != null && Packer == eater) &&
                foodSource.AmbientTemperature > 0f)
            {
                var compRottable = foodSource.TryGetComp<CompRottable>();
                if (compRottable != null)
                {
                    __result += 12f * (1f + compRottable.RotProgressPct);
                }
            }

            if (!ModSettings_VMDT.preferDiningFood || !((foodSource != null) & (eater != null)) ||
                eater.NonHumanlikeOrWildMan() || Packer != null && Packer == eater)
            {
                return;
            }

            var room = foodSource.GetRoom(RegionType.Normal | RegionType.Portal);
            var room2 = eater.GetRoom(RegionType.Normal | RegionType.Portal);
            if (foodSource is Building_NutrientPasteDispenser)
            {
                var intVec = foodSource.def.hasInteractionCell ? foodSource.InteractionCell : foodSource.Position;
                room = intVec.GetRoom(foodSource.Map);
            }

            if (room != null && room2 != null)
            {
                if (FoodUtility.ShouldBeFedBySomeone(eater) && room == room2)
                {
                    __result += dist;
                    return;
                }
            }

            if (room == null || room.Role == null)
            {
                return;
            }

            if (room2 != null)
            {
                if (room.Role == RoomRoleDefOf.PrisonCell && eater.IsPrisoner && room == room2)
                {
                    __result += dist;
                    return;
                }
            }

            if (room.Role != RoomRoleDefOf.DiningRoom)
            {
                return;
            }

            if (dist < 75f)
            {
                __result += dist;
            }
            else
            {
                if (dist < 150f)
                {
                    __result += dist / 2f;
                }
            }
        }
    }
}