using System.Reflection;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VarietyMattersDT
{
    // Token: 0x02000008 RID: 8
    public class Mod_VMDT : Mod
    {
        // Token: 0x04000022 RID: 34
        private readonly Listing_Standard listingStandard = new Listing_Standard();

        // Token: 0x0600001C RID: 28 RVA: 0x00002BE8 File Offset: 0x00000DE8
        public Mod_VMDT(ModContentPack content) : base(content)
        {
            GetSettings<ModSettings_VMDT>();
            if (ModSettings_VMDT.freshUpdate < 1)
            {
                ModSettings_VMDT.freshUpdate++;
            }

            WriteSettings();
            var harmony = new Harmony("rimworld.varietymattersDT");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        // Token: 0x0600001D RID: 29 RVA: 0x00002C48 File Offset: 0x00000E48
        public override string SettingsCategory()
        {
            return "VarietyMattersDinnerTime";
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00002C60 File Offset: 0x00000E60
        public override void DoSettingsWindowContents(Rect inRect)
        {
            var rect = new Rect(100f, 50f, inRect.width * 0.8f, inRect.height);
            listingStandard.Begin(rect);
            listingStandard.Label("Meal Time:");
            var text = ModSettings_VMDT.assignmentPos.ToString("F1");
            var label = "Food Assignment Button Position (0 = left, 44 = right, 85+ = gone):";
            LabeledFloatEntry(listingStandard.GetRect(24f), label, ref ModSettings_VMDT.assignmentPos, ref text, 0.1f,
                1f, 0f, 15f);
            listingStandard.GapLine();
            listingStandard.Label("Food Selection:");
            listingStandard.CheckboxLabeled("Prefer food in dining room, hospital, or prison: ",
                ref ModSettings_VMDT.preferDiningFood);
            listingStandard.CheckboxLabeled("Prefer food close to spoiling: ", ref ModSettings_VMDT.preferSpoiling);
            listingStandard.GapLine();
            listingStandard.Label("Tables Are For Meals:");
            listingStandard.CheckboxLabeled("Tables are optional for pemmican, fruit and certain non-meals: ",
                ref ModSettings_VMDT.foodsWithoutTable);
            listingStandard.CheckboxLabeled("Ate without table thought lasts longer and stacks: ",
                ref ModSettings_VMDT.useTableThought);
            listingStandard.GapLine();
            listingStandard.Label("Quality  Cooking:");
            listingStandard.CheckboxLabeled("Unskilled chefs may cook meals poorly",
                ref ModSettings_VMDT.cookingQuality);
            listingStandard.CheckboxLabeled("Lavish meals are memorable:", ref ModSettings_VMDT.memorableLavish);
            listingStandard.GapLine();
            listingStandard.Label("Freshly Cooked:");
            listingStandard.CheckboxLabeled("Hot meals taste better", ref ModSettings_VMDT.warmMeals);
            listingStandard.CheckboxLabeled("Leftovers taste worse", ref ModSettings_VMDT.leftoverMeals);
            listingStandard.CheckboxLabeled("Frozen leftovers taste the worst", ref ModSettings_VMDT.frozenMeals);
            var warmMeals = ModSettings_VMDT.warmMeals;
            if (warmMeals)
            {
                var label2 = "Hours to stay warm if not refrigerated (default = 20):";
                var text2 = ModSettings_VMDT.warmHours.ToString();
                LabeledFloatEntry(listingStandard.GetRect(24f), label2, ref ModSettings_VMDT.warmHours, ref text2, 1f,
                    5f, 1f, 72f);
            }

            var leftoverMeals = ModSettings_VMDT.leftoverMeals;
            if (leftoverMeals)
            {
                var label3 = "Hours to become leftovers if not refrigerated (default = 40):";
                var text3 = ModSettings_VMDT.leftoverHours.ToString();
                LabeledFloatEntry(listingStandard.GetRect(24f), label3, ref ModSettings_VMDT.leftoverHours, ref text3,
                    1f, 10f, ModSettings_VMDT.warmHours, 72f);
            }

            if (ModSettings_VMDT.warmMeals || ModSettings_VMDT.leftoverMeals)
            {
                var label4 = "Refrigerated Meal Multiplier (1x = no multiplier; default = 2x):";
                var text4 = ModSettings_VMDT.refrigMulti.ToString();
                LabeledFloatEntry(listingStandard.GetRect(24f), label4, ref ModSettings_VMDT.refrigMulti, ref text4,
                    0.1f, 1f, 1f, 10f);
                var label5 = "Minimum stay-warm temperatue (default = 60)";
                var text5 = ModSettings_VMDT.minFreshTemp.ToString();
                LabeledFloatEntry(listingStandard.GetRect(24f), label5, ref ModSettings_VMDT.minFreshTemp, ref text5,
                    1f, 10f, 1f, 100f);
            }

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002FD4 File Offset: 0x000011D4
        public void LabeledFloatEntry(Rect rect, string label, ref float value, ref string editBuffer, float multiplier,
            float largeMultiplier, float min, float max)
        {
            var num = (int)rect.width / 15;
            Widgets.Label(rect, label);
            if (multiplier != largeMultiplier)
            {
                if (Widgets.ButtonText(new Rect(rect.xMax - (num * 5f), rect.yMin, num, rect.height),
                    (-1f * largeMultiplier).ToString()))
                {
                    value -= largeMultiplier * GenUI.CurrentAdjustmentMultiplier();
                    editBuffer = value.ToString();
                    SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
                }

                if (Widgets.ButtonText(new Rect(rect.xMax - num, rect.yMin, num, rect.height),
                    "+" + largeMultiplier))
                {
                    value += largeMultiplier * GenUI.CurrentAdjustmentMultiplier();
                    editBuffer = value.ToString();
                    SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
                }
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - (num * 4f), rect.yMin, num, rect.height),
                (-1f * multiplier).ToString()))
            {
                value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
                editBuffer = value.ToString();
                SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera();
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - (num * 2), rect.yMin, num, rect.height),
                "+" + multiplier))
            {
                value += multiplier * GenUI.CurrentAdjustmentMultiplier();
                editBuffer = value.ToString();
                SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera();
            }

            Widgets.TextFieldNumeric(new Rect(rect.xMax - (num * 3), rect.yMin, num, rect.height), ref value,
                ref editBuffer, min, max);
        }
    }
}