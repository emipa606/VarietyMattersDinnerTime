using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VarietyMattersDT
{
    // Token: 0x0200000C RID: 12
    [HarmonyPatch]
    public class MealTimeAssignment
    {
        // Token: 0x0600002D RID: 45 RVA: 0x000038DC File Offset: 0x00001ADC
        [HarmonyPatch(typeof(TimeAssignmentSelector), "DrawTimeAssignmentSelectorGrid")]
        public static void Postfix(Rect rect)
        {
            rect.yMax -= 2f;
            var rect2 = rect;
            rect2.xMax = rect2.center.x;
            rect2.yMax = rect2.center.y;
            rect2.x += (2f + ModSettings_VMDT.assignmentPos) * rect2.width;
            var royaltyActive = ModsConfig.RoyaltyActive;
            if (royaltyActive)
            {
                rect2.x += rect2.width;
            }

            DrawTimeAssignmentSelectorFor(rect2, DefOf_VMDT.VMDT_Food);
        }

        // Token: 0x0600002E RID: 46 RVA: 0x00003978 File Offset: 0x00001B78
        public static void DrawTimeAssignmentSelectorFor(Rect rect, TimeAssignmentDef ta)
        {
            rect = rect.ContractedBy(2f);
            GUI.DrawTexture(rect, ta.ColorTexture);
            if (Widgets.ButtonInvisible(rect))
            {
                TimeAssignmentSelector.selectedAssignment = ta;
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            GUI.color = Color.white;
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
            }

            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            GUI.color = Color.white;
            Widgets.Label(rect, ta.LabelCap);
            Text.Anchor = TextAnchor.UpperLeft;
            if (TimeAssignmentSelector.selectedAssignment == ta)
            {
                Widgets.DrawBox(rect, 2);
            }
            else
            {
                UIHighlighter.HighlightOpportunity(rect, ta.cachedHighlightNotSelectedTag);
            }
        }
    }
}