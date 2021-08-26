using HarmonyLib;
using RimWorld;
using Verse;

namespace VarietyMattersDT
{
    // Token: 0x0200000B RID: 11
    [HarmonyPatch]
    public class MealTimePriorities
    {
        // Token: 0x06000028 RID: 40 RVA: 0x00003560 File Offset: 0x00001760
        [HarmonyPatch(typeof(JobGiver_GetFood), "GetPriority")]
        [HarmonyPostfix]
        public static void Postfix_GetFoodPriority(ref float __result, Pawn pawn)
        {
            var food = pawn.needs.food;
            if (food == null || pawn.Faction == null || !pawn.Faction.IsPlayer || pawn.NonHumanlikeOrWildMan())
            {
                return;
            }

            if (food.CurCategory < HungerCategory.Starving && FoodUtility.ShouldBeFedBySomeone(pawn))
            {
                return;
            }

            var timeAssignmentDef = pawn.timetable == null
                ? TimeAssignmentDefOf.Anything
                : pawn.timetable.CurrentAssignment;
            if (timeAssignmentDef == DefOf_VMDT.VMDT_Food)
            {
                if (food.CurLevelPercentage < pawn.RaceProps.FoodLevelPercentageWantEat * 1.4)
                {
                    __result = 9.5f;
                }
            }
            else
            {
                if (__result == 0f || food.CurCategory >= HungerCategory.UrgentlyHungry)
                {
                    return;
                }

                var num = GenLocalDate.HourInteger(pawn);
                for (var i = 0; i < 3; i++)
                {
                    num++;
                    if (num == 24)
                    {
                        num = 0;
                    }

                    timeAssignmentDef = pawn.timetable?.GetAssignment(num) == null
                        ? TimeAssignmentDefOf.Anything
                        : pawn.timetable.GetAssignment(num);
                    if (timeAssignmentDef != DefOf_VMDT.VMDT_Food)
                    {
                        continue;
                    }

                    __result = 0f;
                    break;
                }
            }
        }

        // Token: 0x06000029 RID: 41 RVA: 0x000036B4 File Offset: 0x000018B4
        [HarmonyPatch(typeof(ThinkNode_Priority_GetJoy), "GetPriority")]
        [HarmonyPrefix]
        public static bool Prefix_GetJoy(ref float __result, Pawn pawn)
        {
            var joy = pawn.needs.joy;
            if (joy == null || JoyUtility.LordPreventsGettingJoy(pawn) || pawn.IsPrisoner ||
                pawn.Faction is not { IsPlayer: true } || pawn.NonHumanlikeOrWildMan())
            {
                return true;
            }

            var timeAssignmentDef = pawn.timetable == null
                ? TimeAssignmentDefOf.Anything
                : pawn.timetable.CurrentAssignment;
            if (timeAssignmentDef != DefOf_VMDT.VMDT_Food)
            {
                return true;
            }

            __result = 0f;
            if (joy.CurLevel < 0.95f && (pawn.needs.food == null ||
                                         pawn.needs.food.CurLevel >
                                         pawn.RaceProps.FoodLevelPercentageWantEat * 1.4))
            {
                __result = 7f;
            }

            return false;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x000037A0 File Offset: 0x000019A0
        [HarmonyPatch(typeof(JobGiver_GetRest), "GetPriority")]
        [HarmonyPrefix]
        public static bool Prefix_GetRest(ref float __result, Pawn pawn)
        {
            var rest = pawn.needs.rest;
            if (rest == null || pawn.IsPrisoner || pawn.Faction is not { IsPlayer: true } ||
                pawn.NonHumanlikeOrWildMan())
            {
                return true;
            }

            var timeAssignmentDef = pawn.timetable == null
                ? TimeAssignmentDefOf.Anything
                : pawn.timetable.CurrentAssignment;
            if (timeAssignmentDef != DefOf_VMDT.VMDT_Food)
            {
                return true;
            }

            __result = 0f;
            if (rest.CurLevel < 0.3f &&
                pawn.needs.food.CurLevel > pawn.RaceProps.FoodLevelPercentageWantEat * 1.4)
            {
                __result = 8f;
            }

            return false;
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00003870 File Offset: 0x00001A70
        [HarmonyPatch(typeof(JobGiver_Work), "GetPriority")]
        [HarmonyPrefix]
        public static bool Prefix_Work(ref float __result, Pawn pawn)
        {
            if (pawn.workSettings is not { EverWork: true })
            {
                return true;
            }

            var timeAssignmentDef = pawn.timetable == null
                ? TimeAssignmentDefOf.Anything
                : pawn.timetable.CurrentAssignment;
            if (timeAssignmentDef != DefOf_VMDT.VMDT_Food)
            {
                return true;
            }

            __result = 2f;
            return false;
        }
    }
}