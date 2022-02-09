using System;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class SMN
    {
        public const byte ClassID = 26;
        public const uint GCD_SKILL = Ruin;
        public const byte JobID = 27;

        public const uint
            Ruin = 163,
            Ruin2 = 172,
            Ruin3 = 3579,
            Ruin4 = 7426,
            Rekindle = 25830,
            Fester = 181,
            Painflare = 3578,
            DreadwyrmTrance = 3581,
            SummonBahamut = 25800,
            SummonTitan = 25803,
            EnkindleBahamut = 7429,
            EnergySyphon = 16510,
            Outburst = 16511,
            EnergyDrain = 16508,
            SummonCarbuncle = 25798,
            RadiantAegis = 25799,
            Aethercharge = 25800,
            SearingLight = 25801,
            AstralFlow = 25822,
            TriDisaster = 25826,
            CrimsonCyclone = 25835,
            MountainBuster = 25836,
            Slipstream = 25837,
            CrimsonStrike = 25885,
            AstralImpulse = 25820,
            Gemshine = 25883,
            AkhMorn = 7449,
            TopazRite = 25824,
            EmeraldRite = 25825,
            SummonGaruda = 25804,
            SummonIfrit = 25802,
            DeathFlare = 3582,
            PreciousBrilliance = 25884;

        public static class Buffs
        {
            public const ushort
                FurtherRuin = 2701,
                IfritsFavor = 2724,
                GarudasFavor = 2725,
                TitansFavor = 2853;
        }

        public static class Debuffs
        {
            public const ushort
                Placeholder = 0;
        }

        public static class Levels
        {
            public const byte
                SummonCarbuncle = 2,
                RadiantAegis = 2,
                Gemshine = 6,
                EnergyDrain = 10,
                PreciousBrilliance = 26,
                Painflare = 40,
                EnergySyphon = 52,
                Ruin3 = 54,
                Ruin4 = 62,
                SummonGaruda = 22,
                SearingLight = 66,
                EnkindleBahamut = 70,
                Rekindle = 80,
                SummonBahamut = 6,
                SummonTitan = 15,
                ElementalMastery = 86,
                AstralImpulse = 58,
                AkhMorn = 70,
                TopazRite = 72,
                EmeraldRite = 72,
                DeathFlare = 60,
                MountainBuster = 86,
                SummonIfrit = 6,
                AstralFlow = 60,
                SummonPhoenix = 80;
        }
    }

    internal class SummonerFester : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerEDFesterFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Fester)
            {
                var gauge = GetJobGauge<SMNGauge>();

                if (level >= SMN.Levels.EnergyDrain && !gauge.HasAetherflowStacks)
                    return SMN.EnergyDrain;
            }

            return actionID;
        }
    }

    internal class SummonerPainflare : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SummonerESPainflareFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Painflare)
            {
                var gauge = GetJobGauge<SMNGauge>();

                if (level >= SMN.Levels.EnergySyphon && !gauge.HasAetherflowStacks)
                    return SMN.EnergySyphon;

                if (level < SMN.Levels.Painflare)
                    return SMN.EnergyDrain;
            }

            return actionID;
        }
    }

    internal class SummonerRuin : CustomCombo
    {
        private string currentSummon = "";
        private int summonGcdUsedCount = 0;
        private Boolean rekydleUsed = false;
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SmnAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Ruin || actionID == SMN.Ruin2 || actionID == SMN.Ruin3)
            {
                var gauge = GetJobGauge<SMNGauge>();
                if (gauge.SummonTimerRemaining <= 0)
                {
                    this.summonGcdUsedCount = 0;
                    this.currentSummon = "";
                    this.rekydleUsed = false;
                }

                if (gauge.IsIfritAttuned)
                {
                    this.currentSummon = "ifrit";
                }

                if (gauge.IsTitanAttuned)
                {
                    this.currentSummon = "titan";
                }

                if (gauge.IsGarudaAttuned)
                {
                    this.currentSummon = "garuda";
                }


                if (level >= SMN.Levels.SearingLight && IsOffCooldown(SMN.SearingLight) && HasPetPresent() &&
                    !gauge.IsIfritAttuned &&
                    !gauge.IsTitanAttuned &&
                    !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining <= 0 && HasCondition(ConditionFlag.InCombat))
                {
                    return OriginalHook(SMN.SearingLight);
                }

                if (level >= SMN.Levels.RadiantAegis && IsOffCooldown(SMN.RadiantAegis) && HasPetPresent() &&
                    !gauge.IsIfritAttuned &&
                    !gauge.IsTitanAttuned &&
                    !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining <= 0 && HasCondition(ConditionFlag.InCombat))
                {
                    return OriginalHook(SMN.RadiantAegis);
                }

                if (IsUnderGcd(SMN.GCD_SKILL))
                {
                    if (!gauge.IsIfritAttuned &&
                        !gauge.IsTitanAttuned &&
                        !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining <= 0)
                    {
                        if (level >= SMN.Levels.SummonCarbuncle && !HasPetPresent() && gauge.Attunement == 0)
                            return SMN.SummonCarbuncle;

                        if (level >= SMN.Levels.SummonBahamut && gauge.IsBahamutReady &&
                            IsOffCooldown(SMN.SummonBahamut) &&
                            !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned && !gauge.IsIfritAttuned && HasCondition(ConditionFlag.InCombat))
                        {
                            this.currentSummon = "bahamut";
                            return OriginalHook(SMN.SummonBahamut);
                        }

                        if (level >= SMN.Levels.SummonPhoenix && gauge.IsPhoenixReady &&
                            IsOffCooldown(SMN.SummonBahamut) &&
                            !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned && !gauge.IsIfritAttuned)
                        {
                            this.currentSummon = "phoenix";
                            return OriginalHook(SMN.SummonBahamut);
                        }

                        if (level >= SMN.Levels.SummonTitan && gauge.IsTitanReady && IsOffCooldown(SMN.SummonTitan) &&
                            !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned && !gauge.IsIfritAttuned)
                        {
                            this.currentSummon = "titan";
                            return OriginalHook(SMN.SummonTitan);
                        }

                        if (level >= SMN.Levels.SummonGaruda && gauge.IsGarudaReady &&
                            IsOffCooldown(SMN.SummonGaruda) && !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned &&
                            !gauge.IsIfritAttuned)
                        {
                            this.currentSummon = "garuda";
                            return OriginalHook(SMN.SummonGaruda);
                        }

                        if (level >= SMN.Levels.SummonIfrit && gauge.IsIfritReady &&
                            IsOffCooldown(SMN.SummonGaruda) && !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned &&
                            !gauge.IsIfritAttuned)
                        {
                            this.currentSummon = "ifrit";
                            return OriginalHook(SMN.SummonIfrit);
                        }
                    }

                    if (!gauge.IsIfritAttuned && !gauge.IsTitanAttuned &&
                        !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining > 0)
                    {
                        // Rekindle
                        if (level >= SMN.Levels.EnkindleBahamut && IsOffCooldown(OriginalHook(SMN.EnkindleBahamut)))
                            return OriginalHook(SMN.EnkindleBahamut);

                        //if (level >= SMN.Levels.Rekindle && IsOffCooldown(OriginalHook(SMN.AstralFlow)) && this.currentSummon == "phoenix")
                        //    return OriginalHook(SMN.Rekindle);
                    }

                    if (level >= SMN.Levels.AstralFlow && !gauge.IsIfritAttuned &&
                        !gauge.IsTitanAttuned &&
                        !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining > 0 &&
                        IsOffCooldown(OriginalHook(SMN.AstralFlow)))
                    {
                        return OriginalHook(SMN.AstralFlow);
                    }


                    if (level >= SMN.Levels.AstralImpulse && this.currentSummon == "bahamut")
                    {
                        this.summonGcdUsedCount = this.summonGcdUsedCount + 1;
                        return OriginalHook(SMN.AstralImpulse);
                    }


                    if (level >= SMN.Levels.Gemshine)
                    {
                        if (gauge.IsIfritAttuned || gauge.IsTitanAttuned || gauge.IsGarudaAttuned)
                        {
                            this.summonGcdUsedCount = this.summonGcdUsedCount + 1;
                            return OriginalHook(SMN.Gemshine);
                        }
                    }

                    if (level >= SMN.Levels.Ruin4 && gauge.SummonTimerRemaining == 0 &&
                        gauge.AttunmentTimerRemaining == 0 && HasEffect(SMN.Buffs.FurtherRuin))
                    {
                        return SMN.Ruin4;
                    }

                    return OriginalHook(actionID);
                }


                if (level >= ALL.Levels.LucidDream && GetPlayerMana() <= 40 && IsOffCooldown(ALL.LucidDream))
                    return ALL.LucidDream;


                if (level >= SMN.Levels.ElementalMastery && HasEffect(SMN.Buffs.TitansFavor))
                    return SMN.MountainBuster;


                if (gauge.IsTitanAttuned)
                {
                    if (level >= SMN.Levels.MountainBuster && IsOffCooldown(SMN.MountainBuster) &&
                        this.summonGcdUsedCount >= 3)
                        return SMN.MountainBuster;
                }


                if (IsOffCooldown(SMN.Fester) && gauge.AetherflowStacks > 0)
                {
                    return SMN.Fester;
                }

                if (level >= SMN.Levels.EnergyDrain && IsOffCooldown(SMN.EnergyDrain))
                    return SMN.EnergyDrain;

                //if (IsEnabled(CustomComboPreset.SummonerRuinTitansFavorFeature))
                //{
                //    if (level >= SMN.Levels.ElementalMastery && HasEffect(SMN.Buffs.TitansFavor))
                //        return SMN.MountainBuster;
                //}
//
                //if (IsEnabled(CustomComboPreset.SummonerRuinFeature))
                //{
                //    if (level >= SMN.Levels.Gemshine)
                //    {
                //        if (gauge.IsIfritAttuned || gauge.IsTitanAttuned || gauge.IsGarudaAttuned)
                //            return OriginalHook(SMN.Gemshine);
                //    }
                //}
//
                //if (IsEnabled(CustomComboPreset.SummonerFurtherRuinFeature))
                //{
                //    if (level >= SMN.Levels.Ruin4 && gauge.SummonTimerRemaining == 0 && gauge.AttunmentTimerRemaining == 0 && HasEffect(SMN.Buffs.FurtherRuin))
                // }        return SMN.Ruin4;
            }

            return actionID;
        }
    }

    internal class SummonerOutburstTriDisaster : CustomCombo
    {
        private string currentSummon = "";
        private int summonGcdUsedCount = 0;
        private Boolean rekydleUsed = false;
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SmnAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Outburst || actionID == SMN.TriDisaster)
            {
                var gauge = GetJobGauge<SMNGauge>();
                if (gauge.SummonTimerRemaining <= 0)
                {
                    this.summonGcdUsedCount = 0;
                    this.currentSummon = "";
                    this.rekydleUsed = false;
                }

                if (gauge.IsIfritAttuned)
                {
                    this.currentSummon = "ifrit";
                }

                if (gauge.IsTitanAttuned)
                {
                    this.currentSummon = "titan";
                }

                if (gauge.IsGarudaAttuned)
                {
                    this.currentSummon = "garuda";
                }


                if (level >= SMN.Levels.SearingLight && IsOffCooldown(SMN.SearingLight) && HasPetPresent() &&
                    !gauge.IsIfritAttuned &&
                    !gauge.IsTitanAttuned &&
                    !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining <= 0 && HasCondition(ConditionFlag.InCombat))
                {
                    return OriginalHook(SMN.SearingLight);
                }

                if (level >= SMN.Levels.RadiantAegis && IsOffCooldown(SMN.RadiantAegis) && HasPetPresent() &&
                    !gauge.IsIfritAttuned &&
                    !gauge.IsTitanAttuned &&
                    !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining <= 0 && HasCondition(ConditionFlag.InCombat))
                {
                    return OriginalHook(SMN.RadiantAegis);
                }

                if (IsUnderGcd(SMN.GCD_SKILL))
                {
                    if (!gauge.IsIfritAttuned &&
                        !gauge.IsTitanAttuned &&
                        !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining <= 0)
                    {
                        if (level >= SMN.Levels.SummonCarbuncle && !HasPetPresent() && gauge.Attunement == 0)
                            return SMN.SummonCarbuncle;

                        if (level >= SMN.Levels.SummonBahamut && gauge.IsBahamutReady &&
                            IsOffCooldown(SMN.SummonBahamut) &&
                            !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned && !gauge.IsIfritAttuned && HasCondition(ConditionFlag.InCombat))
                        {
                            this.currentSummon = "bahamut";
                            return OriginalHook(SMN.SummonBahamut);
                        }

                        if (level >= SMN.Levels.SummonPhoenix && gauge.IsPhoenixReady &&
                            IsOffCooldown(SMN.SummonBahamut) &&
                            !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned && !gauge.IsIfritAttuned)
                        {
                            this.currentSummon = "phoenix";
                            return OriginalHook(SMN.SummonBahamut);
                        }

                        if (level >= SMN.Levels.SummonTitan && gauge.IsTitanReady && IsOffCooldown(SMN.SummonTitan) &&
                            !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned && !gauge.IsIfritAttuned)
                        {
                            this.currentSummon = "titan";
                            return OriginalHook(SMN.SummonTitan);
                        }

                        if (level >= SMN.Levels.SummonGaruda && gauge.IsGarudaReady &&
                            IsOffCooldown(SMN.SummonGaruda) && !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned &&
                            !gauge.IsIfritAttuned)
                        {
                            this.currentSummon = "garuda";
                            return OriginalHook(SMN.SummonGaruda);
                        }

                        if (level >= SMN.Levels.SummonIfrit && gauge.IsIfritReady &&
                            IsOffCooldown(SMN.SummonGaruda) && !gauge.IsTitanAttuned && !gauge.IsGarudaAttuned &&
                            !gauge.IsIfritAttuned)
                        {
                            this.currentSummon = "ifrit";
                            return OriginalHook(SMN.SummonIfrit);
                        }
                    }

                    if (!gauge.IsIfritAttuned && !gauge.IsTitanAttuned &&
                        !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining > 0)
                    {
                        // Rekindle
                        if (level >= SMN.Levels.EnkindleBahamut && IsOffCooldown(OriginalHook(SMN.EnkindleBahamut)))
                            return OriginalHook(SMN.EnkindleBahamut);

                        //if (level >= SMN.Levels.Rekindle && IsOffCooldown(OriginalHook(SMN.AstralFlow)) && this.currentSummon == "phoenix")
                        //    return OriginalHook(SMN.Rekindle);
                    }

                    if (level >= SMN.Levels.AstralFlow && !gauge.IsIfritAttuned &&
                        !gauge.IsTitanAttuned &&
                        !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining > 0 &&
                        IsOffCooldown(OriginalHook(SMN.AstralFlow)))
                    {
                        return OriginalHook(SMN.AstralFlow);
                    }


                    if (level >= SMN.Levels.AstralImpulse && this.currentSummon == "bahamut")
                    {
                        this.summonGcdUsedCount = this.summonGcdUsedCount + 1;
                        return OriginalHook(SMN.AstralImpulse);
                    }


                    if (level >= SMN.Levels.PreciousBrilliance)
                    {
                        if (gauge.IsIfritAttuned || gauge.IsTitanAttuned || gauge.IsGarudaAttuned)
                        {
                            this.summonGcdUsedCount = this.summonGcdUsedCount + 1;
                            return OriginalHook(SMN.PreciousBrilliance);
                        }
                    }

                    if (level >= SMN.Levels.Ruin4 && gauge.SummonTimerRemaining == 0 &&
                        gauge.AttunmentTimerRemaining == 0 && HasEffect(SMN.Buffs.FurtherRuin))
                    {
                        return SMN.Ruin4;
                    }

                    return OriginalHook(actionID);
                }


                if (level >= ALL.Levels.LucidDream && GetPlayerMana() <= 40 && IsOffCooldown(ALL.LucidDream))
                    return ALL.LucidDream;


                if (level >= SMN.Levels.ElementalMastery && HasEffect(SMN.Buffs.TitansFavor))
                    return SMN.MountainBuster;


                if (gauge.IsTitanAttuned)
                {
                    if (level >= SMN.Levels.MountainBuster && IsOffCooldown(SMN.MountainBuster) &&
                        this.summonGcdUsedCount >= 3)
                        return SMN.MountainBuster;
                }


                if (IsOffCooldown(SMN.Painflare) && gauge.AetherflowStacks > 0)
                {
                    return SMN.Painflare;
                }

                if (level >= SMN.Levels.EnergySyphon && IsOffCooldown(SMN.EnergySyphon))
                    return SMN.EnergySyphon;
            }

            return actionID;
        }
    }

    internal class SummonerGemshinePreciousBrilliance : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SmnAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Gemshine || actionID == SMN.PreciousBrilliance)
            {
                var gauge = GetJobGauge<SMNGauge>();

                if (IsEnabled(CustomComboPreset.SummonerShinyTitansFavorFeature))
                {
                    if (level >= SMN.Levels.ElementalMastery && HasEffect(SMN.Buffs.TitansFavor))
                        return SMN.MountainBuster;
                }

                if (IsEnabled(CustomComboPreset.SummonerShinyEnkindleFeature))
                {
                    if (level >= SMN.Levels.EnkindleBahamut && !gauge.IsIfritAttuned && !gauge.IsTitanAttuned &&
                        !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining > 0)
                        // Rekindle
                        return OriginalHook(SMN.EnkindleBahamut);
                }

                if (IsEnabled(CustomComboPreset.SummonerFurtherShinyFeature))
                {
                    if (level >= SMN.Levels.Ruin4 && gauge.SummonTimerRemaining == 0 &&
                        gauge.AttunmentTimerRemaining == 0 && HasEffect(SMN.Buffs.FurtherRuin))
                        return SMN.Ruin4;
                }
            }

            return actionID;
        }
    }

    internal class SummonerDemiFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SmnAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.Aethercharge || actionID == SMN.DreadwyrmTrance || actionID == SMN.SummonBahamut)
            {
                var gauge = GetJobGauge<SMNGauge>();

                if (IsEnabled(CustomComboPreset.SummonerDemiEnkindleFeature))
                {
                    if (level >= SMN.Levels.EnkindleBahamut && !gauge.IsIfritAttuned && !gauge.IsTitanAttuned &&
                        !gauge.IsGarudaAttuned && gauge.SummonTimerRemaining > 0)
                        // Rekindle
                        return OriginalHook(SMN.EnkindleBahamut);
                }
            }

            return actionID;
        }
    }

    internal class SummonerRadiantCarbundleFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.SummonerRadiantCarbuncleFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.RadiantAegis)
            {
                var gauge = GetJobGauge<SMNGauge>();

                if (level >= SMN.Levels.SummonCarbuncle && !HasPetPresent() && gauge.Attunement == 0)
                    return SMN.SummonCarbuncle;
            }

            return actionID;
        }
    }

    internal class SummonerSearingCarbuncleFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.SummonerSearingCarbuncleFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SMN.SearingLight)
            {
                var gauge = GetJobGauge<SMNGauge>();

                if (level >= SMN.Levels.SummonCarbuncle && !HasPetPresent() && gauge.Attunement == 0)
                    return SMN.SummonCarbuncle;
            }

            return actionID;
        }
    }
}