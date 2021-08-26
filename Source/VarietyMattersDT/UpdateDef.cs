using Verse;

namespace VarietyMattersDT
{
    // Token: 0x0200000E RID: 14
    public class UpdateDef : Def
    {
        // Token: 0x06000031 RID: 49 RVA: 0x00003A94 File Offset: 0x00001C94
        public static void DisplayUpdate()
        {
            if (ModSettings_VMDT.freshUpdate != 0)
            {
                return;
            }

            var diaNode = new DiaNode(DefDatabase<UpdateDef>.GetNamedSilentFail("VMDT_FreshlyCookedUpdate")
                .description);
            diaNode.options.Add(DiaOption.DefaultOK);
            Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false,
                "Variety Matters Dinner Time - New (Optional) Features"));
            ModSettings_VMDT.freshUpdate++;
        }
    }
}