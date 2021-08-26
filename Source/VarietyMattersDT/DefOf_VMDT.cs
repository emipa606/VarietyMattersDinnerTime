using RimWorld;
using Verse;

namespace VarietyMattersDT
{
    // Token: 0x02000006 RID: 6
    [DefOf]
    public static class DefOf_VMDT
    {
        // Token: 0x0400000F RID: 15
        public static TimeAssignmentDef VMDT_Food;

        // Token: 0x04000010 RID: 16
        public static ThoughtDef VMDT_AteWithoutTable;

        // Token: 0x04000011 RID: 17
        public static ThoughtDef VMDT_AteLavishMeal;

        // Token: 0x04000012 RID: 18
        public static HediffDef VMDT_Overate;

        // Token: 0x06000018 RID: 24 RVA: 0x00002A18 File Offset: 0x00000C18
        static DefOf_VMDT()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(DefOf_VMDT));
        }
    }
}