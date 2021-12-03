using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class BLM
    {
        public const byte ClassID = 7;
        public const byte JobID = 25;

        public const uint
            Fire = 141,
            Blizzard = 142,
            Thunder = 144,
            Blizzard2 = 146,
            Transpose = 149,
            Fire3 = 152,
            Thunder3 = 153,
            Blizzard3 = 154,
            Scathe = 156,
            Freeze = 159,
            Flare = 162,
            LeyLines = 3573,
            Blizzard4 = 3576,
            Fire4 = 3577,
            BetweenTheLines = 7419,
            Despair = 16505,
            UmbralSoul = 16506,
            Xenoglossy = 16507;

        public static class Buffs
        {
            public const ushort
                Thundercloud = 164,
                LeyLines = 737,
                Firestarter = 165;
        }

        public static class Debuffs
        {
            public const ushort
                Thunder = 161,
                Thunder3 = 163;
        }

        public static class Levels
        {
            public const byte
                Fire3 = 35,
                Blizzard3 = 35,
                Freeze = 40,
                Thunder3 = 45,
                Flare = 50,
                Enochian = 60,
                BetweenTheLines = 62,
                Despair = 72,
                UmbralSoul = 76,
                Xenoglossy = 80,
                HighFire2 = 82,
                HighBlizzard2 = 82,
                Paradox = 90;
        }
    }

    internal class BlackEnochianFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BlackEnochianFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLM.Fire4 || actionID == BLM.Blizzard4)
            {
                var gauge = GetJobGauge<BLMGauge>();
                if (level >= BLM.Levels.Enochian)
                {
                    if (gauge.InUmbralIce)
                        return BLM.Blizzard4;

                    return BLM.Fire4;
                }
            }

            return actionID;
        }
    }

    internal class BlackManaFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BlackManaFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLM.Transpose)
            {
                var gauge = GetJobGauge<BLMGauge>();
                if (gauge.InUmbralIce && gauge.IsEnochianActive && level >= BLM.Levels.UmbralSoul)
                    return BLM.UmbralSoul;
            }

            return actionID;
        }
    }

    internal class BlackLeyLinesFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BlackLeyLinesFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLM.LeyLines)
            {
                if (HasEffect(BLM.Buffs.LeyLines) && level >= BLM.Levels.BetweenTheLines)
                    return BLM.BetweenTheLines;
            }

            return actionID;
        }
    }

    internal class BlackBlizzardFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BlackBlizzardFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLM.Blizzard)
            {
                var gauge = GetJobGauge<BLMGauge>();
                if (level >= BLM.Levels.Blizzard3 && !gauge.InUmbralIce)
                    return BLM.Blizzard3;
            }

            return actionID;
        }
    }

    internal class BlackFreezeFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BlackFreezeFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLM.Freeze)
            {
                if (level < BLM.Levels.Freeze)
                    return BLM.Blizzard2;
            }

            return actionID;
        }
    }

    internal class BlackFireFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BlackFireFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLM.Fire)
            {
                var gauge = GetJobGauge<BLMGauge>();
                if (level >= BLM.Levels.Fire3 && (!gauge.InAstralFire || HasEffect(BLM.Buffs.Firestarter)))
                    return BLM.Fire3;

                // Paradox
                return OriginalHook(BLM.Fire);
            }

            return actionID;
        }
    }

    internal class BlackScatheFeature : CustomCombo
    {
        protected override CustomComboPreset Preset => CustomComboPreset.BlackScatheFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BLM.Scathe)
            {
                var gauge = GetJobGauge<BLMGauge>();
                if (level >= BLM.Levels.Xenoglossy && gauge.PolyglotStacks > 0)
                    return BLM.Xenoglossy;
            }

            return actionID;
        }
    }
}