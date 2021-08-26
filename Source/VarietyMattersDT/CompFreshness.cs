using RimWorld;
using UnityEngine;
using Verse;

namespace VarietyMattersDT
{
    // Token: 0x02000002 RID: 2
    public class CompFreshness : ThingComp
    {
        // Token: 0x04000004 RID: 4
        private static readonly int freezeTemp = 0;

        // Token: 0x04000005 RID: 5
        private static readonly int refrigTemp = 10;

        // Token: 0x04000006 RID: 6
        public float badChance;

        // Token: 0x04000001 RID: 1
        private bool frozen;

        // Token: 0x04000002 RID: 2
        private float leftoverProgress;

        // Token: 0x04000003 RID: 3
        private float leftoverProgressPct;

        // Token: 0x04000007 RID: 7
        public bool smallPortion = false;

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public int TicksToLeftover => Mathf.RoundToInt(ModSettings_VMDT.leftoverHours * 2500f);

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000002 RID: 2 RVA: 0x00002074 File Offset: 0x00000274
        public int WarmTicks => Mathf.RoundToInt(ModSettings_VMDT.warmHours * 2500f);

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000003 RID: 3 RVA: 0x00002098 File Offset: 0x00000298
        // (set) Token: 0x06000004 RID: 4 RVA: 0x000020D5 File Offset: 0x000002D5
        public float LeftoverProgPct
        {
            get
            {
                float result;
                if (leftoverProgressPct >= 1f)
                {
                    result = 1f;
                }
                else
                {
                    result = leftoverProgress / TicksToLeftover;
                }

                return result;
            }
            private set => leftoverProgressPct = value;
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000005 RID: 5 RVA: 0x000020E0 File Offset: 0x000002E0
        public FreshnessCategory Freshness
        {
            get
            {
                FreshnessCategory result;
                if (ModSettings_VMDT.frozenMeals && frozen)
                {
                    result = FreshnessCategory.FrozenLeftover;
                }
                else
                {
                    if (ModSettings_VMDT.leftoverMeals && LeftoverProgPct >= 1f)
                    {
                        result = FreshnessCategory.Leftover;
                    }
                    else
                    {
                        if (ModSettings_VMDT.warmMeals && leftoverProgress < WarmTicks)
                        {
                            result = FreshnessCategory.Warm;
                        }
                        else
                        {
                            result = FreshnessCategory.Cool;
                        }
                    }
                }

                return result;
            }
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002151 File Offset: 0x00000351
        public void Reset()
        {
            leftoverProgressPct = 0f;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x0000215F File Offset: 0x0000035F
        public override void CompTick()
        {
            DoTicks(1);
        }

        // Token: 0x06000008 RID: 8 RVA: 0x0000216A File Offset: 0x0000036A
        public override void CompTickRare()
        {
            DoTicks(250);
        }

        // Token: 0x06000009 RID: 9 RVA: 0x0000217C File Offset: 0x0000037C
        private void DoTicks(int ticks)
        {
            if (frozen)
            {
                return;
            }

            var ambientTemperature = parent.AmbientTemperature;
            if (ambientTemperature <= freezeTemp)
            {
                if (leftoverProgress < WarmTicks)
                {
                    leftoverProgress = WarmTicks;
                }

                if (leftoverProgressPct == 1f)
                {
                    frozen = true;
                    return;
                }
            }

            if (!ModSettings_VMDT.warmMeals && !ModSettings_VMDT.leftoverMeals && !ModSettings_VMDT.frozenMeals ||
                !(LeftoverProgPct < 1f))
            {
                return;
            }

            if (ambientTemperature <= refrigTemp)
            {
                leftoverProgress += ModSettings_VMDT.refrigMulti * ticks;
            }
            else
            {
                if (ambientTemperature < ModSettings_VMDT.minFreshTemp)
                {
                    leftoverProgress += ticks;
                }
            }

            if (leftoverProgress >= TicksToLeftover)
            {
                LeftoverProgPct = 1f;
            }
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002298 File Offset: 0x00000498
        public override void PrePostIngested(Pawn ingester)
        {
            if (ingester.needs.mood == null)
            {
                return;
            }

            if (ModSettings_VMDT.cookingQuality && badChance >= 1f)
            {
                CookingQuality.PoorlyCookedEffects(ingester, parent);
            }

            switch (Freshness)
            {
                case FreshnessCategory.Warm:
                    ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtDef.Named("VMDT_HotMeal"));
                    goto IL_E7;
                case FreshnessCategory.Leftover:
                    ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtDef.Named("VMDT_Leftovers"));
                    goto IL_E7;
                case FreshnessCategory.FrozenLeftover:
                    ingester.needs.mood.thoughts.memories.TryGainMemory(ThoughtDef.Named("VMDT_FrozenLeftovers"));
                    goto IL_E7;
            }

            base.PrePostIngested(ingester);
            IL_E7: ;
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002390 File Offset: 0x00000590
        public override bool AllowStackWith(Thing other)
        {
            var num = 21f;
            var spawned = parent.Spawned;
            if (spawned)
            {
                num = GenTemperature.GetTemperatureForCell(parent.Position, parent.Map);
            }

            if (!(num > freezeTemp))
            {
                return base.AllowStackWith(other);
            }

            var compFreshness = other.TryGetComp<CompFreshness>();
            if (frozen && !compFreshness.frozen || Freshness != compFreshness.Freshness)
            {
                return false;
            }

            return base.AllowStackWith(other);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002424 File Offset: 0x00000624
        public override void PreAbsorbStack(Thing otherStack, int count)
        {
            var t = count / (float)(parent.stackCount + count);
            var compFreshness = otherStack.TryGetComp<CompFreshness>();
            if (compFreshness.frozen)
            {
                leftoverProgressPct = 1f;
                frozen = true;
            }
            else
            {
                leftoverProgress = Mathf.Lerp(leftoverProgress, compFreshness.leftoverProgress, t);
            }

            badChance = Mathf.Lerp(badChance, compFreshness.badChance, t);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000024A0 File Offset: 0x000006A0
        public override void PostSplitOff(Thing piece)
        {
            var compFreshness = piece.TryGetComp<CompFreshness>();
            compFreshness.frozen = frozen;
            compFreshness.leftoverProgress = leftoverProgress;
            if (!ModSettings_VMDT.cookingQuality || !(badChance > 0f))
            {
                return;
            }

            float num = parent.stackCount + piece.stackCount;
            if (badChance == 1f)
            {
                compFreshness.badChance = badChance;
            }
            else
            {
                if (compFreshness.badChance == 1f)
                {
                    return;
                }

                float num2 = (int)(badChance * num);
                var num3 = Rand.Range(0f, 1f);
                if (num3 < badChance)
                {
                    badChance = (num2 - piece.stackCount) / (num - piece.stackCount);
                    compFreshness.badChance = 1f;
                }
                else
                {
                    badChance = num2 / (num - piece.stackCount);
                    compFreshness.badChance = 0f;
                }
            }
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000025B0 File Offset: 0x000007B0
        public override string CompInspectStringExtra()
        {
            var result = base.CompInspectStringExtra();
            switch (Freshness)
            {
                case FreshnessCategory.Warm:
                {
                    if (parent.AmbientTemperature < ModSettings_VMDT.minFreshTemp)
                    {
                        result = "VMDT: Warm, but getting cold (" + (leftoverProgress / WarmTicks).ToStringPercent() +
                                 ")";
                    }
                    else
                    {
                        result = "VMDT: Staying warm (" + (leftoverProgress / WarmTicks).ToStringPercent() + ")";
                    }

                    return result;
                }
                case FreshnessCategory.Leftover:
                    return "VMDT: Leftovers";
                case FreshnessCategory.FrozenLeftover:
                    return "VMDT: Freezer Burned";
            }

            var leftoverMeals = ModSettings_VMDT.leftoverMeals;
            if (leftoverMeals)
            {
                result = "VMDT: Turning to leftovers (" + LeftoverProgPct.ToStringPercent() + ")";
            }

            return result;
        }

        // Token: 0x0600000F RID: 15 RVA: 0x0000268C File Offset: 0x0000088C
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref badChance, "poorlyCooked");
            Scribe_Values.Look(ref frozen, "frozen");
            Scribe_Values.Look(ref leftoverProgress, "leftoverProgress");
            Scribe_Values.Look(ref leftoverProgressPct, "leftoverProgressPct");
        }
    }
}