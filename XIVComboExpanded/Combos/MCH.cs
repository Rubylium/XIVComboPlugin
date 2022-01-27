using System;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class MCH
    {
        public const double GDC = 0.55;
        public const byte JobID = 31;

        public const uint
            // Single target
            Reassemble = 2876,
            CleanShot = 2873,
            HeatedCleanShot = 7413,
            SplitShot = 2866,
            HeatedSplitShot = 7411,
            SlugShot = 2868,
            HeatedSlugshot = 7412,
            // Charges
            GaussRound = 2874,
            Ricochet = 2890,
            // AoE
            SpreadShot = 2870,
            AutoCrossbow = 16497,
            Scattergun = 25786,
            // Rook
            RookAutoturret = 2864,
            RookOverdrive = 7415,
            AutomatonQueen = 16501,
            QueenOverdrive = 16502,
            // Other
            Wildfire = 2878,
            Detonator = 16766,
            Hypercharge = 17209,
            HeatBlast = 7410,
            HotShot = 2872,
            Drill = 16498,
            AirAnchor = 16500,
            BarrelStabilizer = 7414,
            Flamethrower = 7418,
            Chainsaw = 25788;

        public static class Buffs
        {
            public const ushort
                Reassemble = 851,
                Flamethrower = 1205;
        }

        public static class Debuffs
        {
            public const ushort
                Wildfire = 861;
        }

        public static class Levels
        {
            public const byte
                Reassemble = 10,
                BarrelStabilizer = 66,
                SlugShot = 2,
                GaussRound = 15,
                CleanShot = 26,
                Hypercharge = 30,
                HeatBlast = 35,
                RookOverdrive = 40,
                Wildfire = 45,
                Ricochet = 50,
                AutoCrossbow = 52,
                HeatedSplitShot = 54,
                Drill = 58,
                HeatedSlugshot = 60,
                HeatedCleanShot = 64,
                ChargedActionMastery = 74,
                AirAnchor = 76,
                AutomatonQueen = 80,
                QueenOverdrive = 80,
                RookAutoturret = 40,
                Flamethrower = 70,
                HotShot = 4,
                Chainsaw = 90;
        }
    }

    internal class MachinistMainCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MachinistMainCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MCH.CleanShot || actionID == MCH.HeatedCleanShot)
            {
                var gauge = GetJobGauge<MCHGauge>();
                var inCombat = HasCondition(Dalamud.Game.ClientState.Conditions.ConditionFlag.InCombat);

                if (GetCooldown(MCH.SplitShot).CooldownRemaining <= MCH.GDC)
                {
                    if (gauge.IsOverheated && level >= MCH.Levels.HeatBlast && !HasEffect(MCH.Buffs.Reassemble))
                        return MCH.HeatBlast;


                    if (level >= MCH.Levels.AirAnchor && IsOffCooldown(MCH.AirAnchor) &&
                        HasEffect(MCH.Buffs.Reassemble))
                    {
                        return MCH.AirAnchor;
                    }

                    if (level >= MCH.Levels.AirAnchor && IsOffCooldown(MCH.AirAnchor) &&
                        GetCooldown(MCH.Reassemble).CooldownRemaining > 10)
                    {
                        return MCH.AirAnchor;
                    }

                    if (level < MCH.Levels.AirAnchor && level >= MCH.Levels.HotShot && IsOffCooldown(MCH.HotShot) &&
                        HasEffect(MCH.Buffs.Reassemble))
                    {
                        return MCH.HotShot;
                    }

                    if (level < MCH.Levels.AirAnchor && level >= MCH.Levels.AirAnchor && IsOffCooldown(MCH.HotShot) &&
                        GetCooldown(MCH.Reassemble).CooldownRemaining > 10)
                    {
                        return MCH.HotShot;
                    }


                    if (level >= MCH.Levels.Drill && IsOffCooldown(MCH.Drill) &&
                        HasEffect(MCH.Buffs.Reassemble))
                    {
                        return MCH.Drill;
                    }

                    if (level >= MCH.Levels.Drill && IsOffCooldown(MCH.Drill) &&
                        GetCooldown(MCH.Reassemble).CooldownRemaining > 10)
                    {
                        return MCH.Drill;
                    }


                    if (comboTime > 0 && !HasEffect(MCH.Buffs.Reassemble))
                    {
                        if (lastComboMove == MCH.SlugShot && level >= MCH.Levels.CleanShot)
                            // Heated
                            return OriginalHook(MCH.CleanShot);

                        if (lastComboMove == MCH.SplitShot && level >= MCH.Levels.SlugShot)
                            // Heated
                            return OriginalHook(MCH.SlugShot);
                    }

                    // Heated
                    if (!HasEffect(MCH.Buffs.Flamethrower) && !HasEffect(MCH.Buffs.Reassemble))
                    {
                        return OriginalHook(MCH.SplitShot);
                    }
                }

                if (!HasEffect(MCH.Buffs.Reassemble))
                {
                    if (level >= ALL.Levels.HeadGraze && CanInterruptEnemy() && !GetCooldown(ALL.HeadGraze).IsCooldown)
                    {
                        return ALL.HeadGraze;
                    }

                    if ((LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp <= 40 &&
                        IsOffCooldown(ALL.SecondWind) && level >= ALL.Levels.SecondWind)
                    {
                        return ALL.SecondWind;
                    }

                    if (TargetHasEffect(MCH.Debuffs.Wildfire) &&
                        FindTargetEffect(MCH.Debuffs.Wildfire).RemainingTime <= 2)
                    {
                        return MCH.Detonator;
                    }

                    if (level >= MCH.Levels.Wildfire && IsOffCooldown(MCH.Wildfire) &&
                        !TargetHasEffect(MCH.Debuffs.Wildfire) && gauge.IsOverheated)
                    {
                        return MCH.Wildfire;
                    }


                    if (level >= MCH.Levels.BarrelStabilizer && IsOffCooldown(MCH.BarrelStabilizer) &&
                        gauge.Heat <= 50 && inCombat)
                    {
                        return MCH.BarrelStabilizer;
                    }

                    if (level < MCH.Levels.AutomatonQueen && level >= MCH.Levels.RookAutoturret &&
                        IsOffCooldown(MCH.RookAutoturret) && gauge.Battery >= 50)
                    {
                        return MCH.RookAutoturret;
                    }

                    if (level >= MCH.Levels.AutomatonQueen && IsOffCooldown(MCH.AutomatonQueen) &&
                        gauge.Battery >= 50)
                    {
                        return MCH.AutomatonQueen;
                    }

                    if (level >= MCH.Levels.Hypercharge && IsOffCooldown(MCH.Hypercharge) && gauge.Heat >= 50 &&
                        !IsOffCooldown(MCH.Reassemble))
                    {
                        return MCH.Hypercharge;
                    }

                    if (level >= MCH.Levels.Hypercharge && IsOffCooldown(MCH.Hypercharge) && gauge.Heat >= 50)
                    {
                        return MCH.Hypercharge;
                    }


                    if (level >= MCH.Levels.Ricochet && GetCooldown(MCH.Ricochet).RemainingCharges >= 1 &&
                        !IsOffCooldown(MCH.Reassemble))
                    {
                        return MCH.Ricochet;
                    }

                    if (level >= MCH.Levels.GaussRound && GetCooldown(MCH.GaussRound).RemainingCharges >= 1 &&
                        !IsOffCooldown(MCH.Reassemble))
                    {
                        return MCH.GaussRound;
                    }


                    if (level >= MCH.Levels.Reassemble && IsOffCooldown(MCH.Reassemble))
                    {
                        if (IsOffCooldown(MCH.Drill) && level >= MCH.Levels.Drill)
                        {
                            return MCH.Reassemble;
                        }

                        if (IsOffCooldown(MCH.AirAnchor) && level >= MCH.Levels.AirAnchor)
                        {
                            return MCH.Reassemble;
                        }

                        if (IsOffCooldown(MCH.Chainsaw) && level >= MCH.Levels.Chainsaw)
                        {
                            return MCH.Reassemble;
                        }
                    }
                }
            }

            return actionID;
        }
    }

    internal class MachinistGaussRoundRicochet : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.MachinistGaussRoundRicochetFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MCH.GaussRound || actionID == MCH.Ricochet)
            {
                var gauge = GetJobGauge<MCHGauge>();

                if (IsEnabled(CustomComboPreset.MachinistGaussRoundRicochetFeatureOption))
                {
                    if (!gauge.IsOverheated)
                        return actionID;
                }

                if (level >= MCH.Levels.Ricochet)
                    return CalcBestAction(actionID, MCH.GaussRound, MCH.Ricochet);

                return MCH.GaussRound;
            }

            return actionID;
        }
    }

    internal class MachinistWildfire : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MachinistHyperfireFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MCH.Hypercharge)
            {
                if (level >= MCH.Levels.Wildfire && IsOffCooldown(MCH.Wildfire) && HasTarget())
                    return MCH.Wildfire;

                if (level >= MCH.Levels.Wildfire && IsOnCooldown(MCH.Hypercharge) && !IsOriginal(MCH.Wildfire))
                    return MCH.Detonator;
            }

            return actionID;
        }
    }

    internal class MachinistHeatBlastAutoCrossbow : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MachinistOverheatFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MCH.HeatBlast || actionID == MCH.AutoCrossbow)
            {
                var gauge = GetJobGauge<MCHGauge>();

                if (IsEnabled(CustomComboPreset.MachinistHyperfireFeature))
                {
                    if (level >= MCH.Levels.Wildfire && IsOffCooldown(MCH.Wildfire) && HasTarget())
                        return MCH.Wildfire;
                }

                if (level >= MCH.Levels.Hypercharge && !gauge.IsOverheated)
                    return MCH.Hypercharge;

                if (level < MCH.Levels.AutoCrossbow)
                    return MCH.HeatBlast;
            }

            return actionID;
        }
    }

    internal class MachinistSpreadShot : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MachinistSpreadShotFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MCH.SpreadShot || actionID == MCH.Scattergun)
            {
                var gauge = GetJobGauge<MCHGauge>();

                if (level >= MCH.Levels.AutoCrossbow && gauge.IsOverheated)
                    return MCH.AutoCrossbow;
            }

            return actionID;
        }
    }

    internal class MachinistRookAutoturret : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.MachinistOverdriveFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MCH.RookAutoturret || actionID == MCH.AutomatonQueen)
            {
                var gauge = GetJobGauge<MCHGauge>();

                if (level >= MCH.Levels.RookOverdrive && gauge.IsRobotActive)
                    // Queen Overdrive
                    return OriginalHook(MCH.RookOverdrive);
            }

            return actionID;
        }
    }

    internal class MachinistDrillAirAnchorChainsaw : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.MachinistHotShotDrillChainsawFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MCH.HotShot || actionID == MCH.AirAnchor || actionID == MCH.Drill ||
                actionID == MCH.Chainsaw)
            {
                if (level >= MCH.Levels.Chainsaw)
                    return CalcBestAction(actionID, MCH.Chainsaw, MCH.AirAnchor, MCH.Drill);

                if (level >= MCH.Levels.AirAnchor)
                    return CalcBestAction(actionID, MCH.AirAnchor, MCH.Drill);

                if (level >= MCH.Levels.Drill)
                    return CalcBestAction(actionID, MCH.Drill, MCH.HotShot);

                return MCH.HotShot;
            }

            return actionID;
        }
    }

    internal class MachinistAirAnchorChainsaw : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.MachinistHotShotChainsawFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == MCH.HotShot || actionID == MCH.AirAnchor || actionID == MCH.Chainsaw)
            {
                if (level >= MCH.Levels.Chainsaw)
                    return CalcBestAction(actionID, MCH.Chainsaw, MCH.AirAnchor);

                if (level >= MCH.Levels.AirAnchor)
                    return MCH.AirAnchor;

                return MCH.HotShot;
            }

            return actionID;
        }
    }
}