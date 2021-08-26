using Verse;

namespace VarietyMattersDT
{
    // Token: 0x02000007 RID: 7
    public class ModSettings_VMDT : ModSettings
    {
        // Token: 0x04000013 RID: 19
        public static float assignmentPos;

        // Token: 0x04000014 RID: 20
        public static bool preferDiningFood = true;

        // Token: 0x04000015 RID: 21
        public static bool preferSpoiling = true;

        // Token: 0x04000016 RID: 22
        public static bool foodsWithoutTable;

        // Token: 0x04000017 RID: 23
        public static bool useTableThought;

        // Token: 0x04000018 RID: 24
        public static bool memorableLavish;

        // Token: 0x04000019 RID: 25
        public static bool cookingQuality;

        // Token: 0x0400001A RID: 26
        public static bool warmMeals;

        // Token: 0x0400001B RID: 27
        public static bool leftoverMeals;

        // Token: 0x0400001C RID: 28
        public static bool frozenMeals;

        // Token: 0x0400001D RID: 29
        public static float minFreshTemp = 60f;

        // Token: 0x0400001E RID: 30
        public static float warmHours = 20f;

        // Token: 0x0400001F RID: 31
        public static float leftoverHours = 40f;

        // Token: 0x04000020 RID: 32
        public static float refrigMulti = 2f;

        // Token: 0x04000021 RID: 33
        public static int freshUpdate = -1;

        // Token: 0x06000019 RID: 25 RVA: 0x00002A2C File Offset: 0x00000C2C
        public override void ExposeData()
        {
            Scribe_Values.Look(ref assignmentPos, "foodPos");
            Scribe_Values.Look(ref preferDiningFood, "preferDiningFood", true);
            Scribe_Values.Look(ref preferSpoiling, "preferSpoiling", true);
            Scribe_Values.Look(ref foodsWithoutTable, "foodsWithoutTable");
            Scribe_Values.Look(ref useTableThought, "useTableThought");
            Scribe_Values.Look(ref memorableLavish, "memorableLavish");
            Scribe_Values.Look(ref cookingQuality, "cookingQuality");
            Scribe_Values.Look(ref warmMeals, "warmMeals");
            Scribe_Values.Look(ref leftoverMeals, "leftoverMeals");
            Scribe_Values.Look(ref frozenMeals, "frozenMeals");
            Scribe_Values.Look(ref warmHours, "warmHours", 20f);
            Scribe_Values.Look(ref leftoverHours, "leftoverHours", 40f);
            Scribe_Values.Look(ref refrigMulti, "refrigMulti", 2f);
            Scribe_Values.Look(ref minFreshTemp, "minFreshTemp", 60f);
            Scribe_Values.Look(ref freshUpdate, "freshUpdate", -1);
            base.ExposeData();
        }
    }
}