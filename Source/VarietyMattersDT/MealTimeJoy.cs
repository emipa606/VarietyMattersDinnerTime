using HarmonyLib;
using RimWorld;
using Verse;

namespace VarietyMattersDT
{
    // Token: 0x0200000A RID: 10
    [HarmonyPatch]
    public class MealTimeJoy
    {
        // Token: 0x06000026 RID: 38 RVA: 0x000033F0 File Offset: 0x000015F0
        [HarmonyPatch(typeof(JoyGiver), "GetChance")]
        public static void Postfix(ref float __result, JoyGiver __instance, Pawn pawn)
        {
            if (pawn.IsPrisoner || pawn.NonHumanlikeOrWildMan() || pawn.Faction is not { IsPlayer: true })
            {
                return;
            }

            var timeAssignmentDef = pawn.timetable == null
                ? TimeAssignmentDefOf.Anything
                : pawn.timetable.CurrentAssignment;
            var joyKind = __instance.def.joyKind;
            var jobDef = __instance.def.jobDef;
            if (timeAssignmentDef == DefOf_VMDT.VMDT_Food)
            {
                if (jobDef == JobDefOf.SocialRelax || joyKind == JoyKindDefOf.Gluttonous)
                {
                    __result *= 1.4f;
                }
                else
                {
                    if (joyKind.defName == "Television")
                    {
                        __result = 0f;
                    }
                    else
                    {
                        if (joyKind.defName != "Chemical" ||
                            jobDef.driverClass.Name != "JobDriver_PlayPoker" ||
                            jobDef.driverClass.Name != "JobDriver_PlayBilliards")
                        {
                            __result *= 0.6f;
                        }
                    }
                }
            }
            else
            {
                if (pawn.timetable != null && !pawn.timetable.times.Contains(DefOf_VMDT.VMDT_Food))
                {
                    return;
                }

                if (jobDef == JobDefOf.SocialRelax || joyKind == JoyKindDefOf.Gluttonous)
                {
                    __result *= 5f;
                }
            }
        }
    }
}