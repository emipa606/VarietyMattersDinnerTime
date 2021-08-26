using Verse;

namespace VarietyMattersDT.VMDT
{
    // Token: 0x0200000F RID: 15
    public class Component_VMDT : GameComponent
    {
        // Token: 0x06000033 RID: 51 RVA: 0x00003B08 File Offset: 0x00001D08
        public Component_VMDT(Game game)
        {
        }

        // Token: 0x06000034 RID: 52 RVA: 0x00003B12 File Offset: 0x00001D12
        public override void FinalizeInit()
        {
            UpdateDef.DisplayUpdate();
            base.FinalizeInit();
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00003B24 File Offset: 0x00001D24
        public static void ShowTablelessFoods()
        {
            foreach (var thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
            {
                if (thingDef.HasModExtension<DefMod_VMDT>())
                {
                    Log.Message(thingDef.label);
                }
            }
        }
    }
}