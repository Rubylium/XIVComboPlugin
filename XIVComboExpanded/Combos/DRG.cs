using Dalamud.Game.ClientState.JobGauge.Types;
using Lumina.Data.Parsing.Layer;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class DRG
    {
        public const double GDC = 0.55;
        public const uint GCD_SKILL = TrueThrust;
        public const byte ClassID = 4;
        public const byte JobID = 22;

        public const uint
            // Single Target
            TrueThrust = 75,
            VorpalThrust = 78,
            Disembowel = 87,
            FullThrust = 84,
            PiercingTalon = 90,
            ChaosThrust = 88,
            LifeSurge = 83,
            HeavensThrust = 25771,
            ChaoticSpring = 25772,
            WheelingThrust = 3556,
            FangAndClaw = 3554,
            RaidenThrust = 16479,
            // AoE
            DoomSpike = 86,
            SonicThrust = 7397,
            CoerthanTorment = 16477,
            DraconianFury = 25770,
            // Combined
            Geirskogul = 3555,
            Nastrond = 7400,
            // Jumps
            Jump = 92,
            SpineshatterDive = 95,
            DragonfireDive = 96,
            HighJump = 16478,
            MirageDive = 7399,
            // Dragon
            Stardiver = 16480,
            LanceCharge = 85,
            DragonSight = 7398,
            BattleLitany = 3557,
            Nastond = 7400,
            WyrmwindThrust = 25773;

        public static class Buffs
        {
            public const ushort
                SharperFangAndClaw = 802,
                EnhancedWheelingThrust = 803,
                LifeSurge = 116,
                LanceCharge = 1864,
                DraconianFire = 1863,
                DragonSight = 1910,
                DiveReady = 1243;
        }

        public static class Debuffs
        {
            public const ushort
                ChaoticSpring = 2719;
        }

        public static class Levels
        {
            public const byte
                Nastond = 70,
                Jump = 30,
                LanceCharge = 30,
                VorpalThrust = 4,
                Disembowel = 18,
                BattleLitany = 52,
                PiercingTalon = 15,
                FullThrust = 26,
                SpineshatterDive = 45,
                DragonfireDive = 50,
                ChaosThrust = 50,
                HeavensThrust = 86,
                ChaoticSpring = 86,
                FangAndClaw = 56,
                WheelingThrust = 58,
                Geirskogul = 60,
                SonicThrust = 62,
                MirageDive = 68,
                LifeOfTheDragon = 70,
                CoerthanTorment = 72,
                HighJump = 74,
                RaidenThrust = 76,
                WyrmwindThrust = 90,
                DragonSight = 66,
                Stardiver = 80;
        }
    }

    internal class DragoonJump : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonJumpFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == DRG.Jump || actionID == DRG.HighJump)
            {
                if (level >= DRG.Levels.MirageDive && HasEffect(DRG.Buffs.DiveReady))
                    return DRG.MirageDive;
            }

            return actionID;
        }
    }

    internal class DragoonCoerthanTorment : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DrgAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == DRG.CoerthanTorment)
            {
                var gauge = GetJobGauge<DRGGauge>();

                if (IsEnabled(CustomComboPreset.DragoonCoerthanWyrmwindFeature))
                {
                    if (gauge.FirstmindsFocusCount == 2)
                        return DRG.WyrmwindThrust;
                }

                if (IsEnabled(CustomComboPreset.DragoonCoerthanTormentCombo))
                {
                    if (comboTime > 0)
                    {
                        if (lastComboMove == DRG.SonicThrust && level >= DRG.Levels.CoerthanTorment)
                            return DRG.CoerthanTorment;

                        if ((lastComboMove == DRG.DoomSpike || lastComboMove == DRG.DraconianFury) &&
                            level >= DRG.Levels.SonicThrust)
                            return DRG.SonicThrust;
                    }

                    // Draconian Fury
                    return OriginalHook(DRG.DoomSpike);
                }
            }

            return actionID;
        }
    }

    internal class DragoonChaosThrust : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DrgAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == DRG.ChaosThrust || actionID == DRG.ChaoticSpring)
            {
                if (IsEnabled(CustomComboPreset.DragoonFangThrustFeature))
                {
                    if (level >= DRG.Levels.FangAndClaw && (HasEffect(DRG.Buffs.SharperFangAndClaw) ||
                                                            HasEffect(DRG.Buffs.EnhancedWheelingThrust)))
                        return DRG.WheelingThrust;
                }

                if (IsEnabled(CustomComboPreset.DragoonChaosThrustCombo))
                {
                    if (level >= DRG.Levels.FangAndClaw && HasEffect(DRG.Buffs.SharperFangAndClaw))
                        return DRG.FangAndClaw;

                    if (level >= DRG.Levels.WheelingThrust && HasEffect(DRG.Buffs.EnhancedWheelingThrust))
                        return DRG.WheelingThrust;

                    if (comboTime > 0)
                    {
                        if (lastComboMove == DRG.Disembowel && level >= DRG.Levels.ChaosThrust)
                            // ChaoticSpring
                            return OriginalHook(DRG.ChaosThrust);

                        if ((lastComboMove == DRG.TrueThrust || lastComboMove == DRG.RaidenThrust) &&
                            level >= DRG.Levels.Disembowel)
                            return DRG.Disembowel;
                    }

                    if (IsEnabled(CustomComboPreset.DragoonChaosThrustComboOption))
                        return DRG.Disembowel;

                    // Vorpal Thrust
                    return OriginalHook(DRG.TrueThrust);
                }
            }

            return actionID;
        }
    }

    internal class DragoonFullThrust : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DrgAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == DRG.FullThrust)
            {
                var gauge = GetJobGauge<DRGGauge>();

                //if ((System.Numerics.Vector3.Distance(CurrentTarget.Position, LocalPlayer.Position) -
                //     CurrentTarget.HitboxRadius) >= 4.0)
                //{
                //    if (level >= DRG.Levels.PiercingTalon)
                //    {
                //        return DRG.PiercingTalon;
                //    }
                //}

                if (IsUnderGcd(DRG.GCD_SKILL) && IsTargetInRange())
                {
                    if (level >= DRG.Levels.WheelingThrust && HasEffect(DRG.Buffs.EnhancedWheelingThrust))
                        return DRG.WheelingThrust;

                    if (level >= DRG.Levels.FangAndClaw && HasEffect(DRG.Buffs.SharperFangAndClaw))
                        return DRG.FangAndClaw;

                    var ShouldDoCombo = true;
                    if (lastComboMove != DRG.WheelingThrust)
                    {
                        if (level >= DRG.Levels.WyrmwindThrust && gauge.FirstmindsFocusCount == 2)
                        {
                            ShouldDoCombo = false;
                        }
                    }

                    if (ShouldDoCombo)
                    {
                        if (comboTime > 0)
                        {
                            if (!TargetHasEffect(DRG.Debuffs.ChaoticSpring) ||
                                FindTargetEffect(DRG.Debuffs.ChaoticSpring).RemainingTime <= 5)
                            {
                                if (lastComboMove == DRG.Disembowel && level >= DRG.Levels.ChaosThrust)
                                    // ChaoticSpring
                                    return OriginalHook(DRG.ChaosThrust);

                                if ((lastComboMove == DRG.TrueThrust || lastComboMove == DRG.RaidenThrust) &&
                                    level >= DRG.Levels.Disembowel)
                                    return DRG.Disembowel;
                            }
                        }

                        if (comboTime > 0)
                        {
                            if (lastComboMove == DRG.VorpalThrust && level >= DRG.Levels.FullThrust)
                                // Heavens' Thrust
                                return OriginalHook(DRG.FullThrust);

                            if ((lastComboMove == DRG.TrueThrust || lastComboMove == DRG.RaidenThrust) &&
                                level >= DRG.Levels.VorpalThrust)
                                return DRG.VorpalThrust;
                        }


                        // Vorpal Thrust
                        return OriginalHook(DRG.TrueThrust);
                    }
                }

                if ((LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp <= 40 &&
                    IsOffCooldown(ALL.SecondWind) && level >= ALL.Levels.SecondWind)
                {
                    return ALL.SecondWind;
                }


                if (GetCooldown(DRG.LifeSurge).RemainingCharges > 0 && !HasEffect(DRG.Buffs.LifeSurge) &&
                    lastComboMove == DRG.VorpalThrust && IsTargetInRange())
                {
                    return DRG.LifeSurge;
                }


                if (level < DRG.Levels.FullThrust &&
                    GetCooldown(DRG.LifeSurge).RemainingCharges > 0 &&
                    !HasEffect(DRG.Buffs.LifeSurge) && IsTargetInRange())
                {
                    return DRG.LifeSurge;
                }


                if ((LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp <= 50 &&
                    IsOffCooldown(ALL.Bloodbath) && level >= ALL.Levels.Bloodbath && IsTargetInRange())
                {
                    return ALL.Bloodbath;
                }

                if (level >= DRG.Levels.LanceCharge && IsOffCooldown(DRG.LanceCharge) && IsTargetInRange())
                {
                    return DRG.LanceCharge;
                }

                if (level >= DRG.Levels.BattleLitany && IsOffCooldown(DRG.BattleLitany) && IsTargetInRange())
                {
                    return DRG.BattleLitany;
                }

                if (level >= ALL.Levels.TrueNorth && GetCooldown(ALL.TrueNorth).RemainingCharges > 0 &&
                    IsTargetInRange())
                {
                    if (HasEffect(DRG.Buffs.EnhancedWheelingThrust) || HasEffect(DRG.Buffs.SharperFangAndClaw))
                        return ALL.TrueNorth;
                }

                if (level >= DRG.Levels.DragonSight && IsOffCooldown(DRG.DragonSight) &&
                    IsOffCooldown(DRG.DragonfireDive))
                {
                    return DRG.DragonSight;
                }

                //if (level >= ALL.Levels.Feint && IsOffCooldown(ALL.Feint) && IsTargetInRange())
                //{
                //    return ALL.Feint;
                //}


                if (level >= DRG.Levels.MirageDive && IsOffCooldown(DRG.MirageDive) && level >= DRG.Levels.HighJump &&
                    HasEffect(DRG.Buffs.DiveReady) && IsTargetInRangeGiven(20) && gauge.EyeCount < 2)
                {
                    return DRG.MirageDive;
                }


                if (level >= DRG.Levels.Geirskogul && IsOffCooldown(DRG.Geirskogul) && level >= DRG.Levels.HighJump &&
                    gauge.EyeCount == 2 && IsTargetInRangeGiven(15))
                {
                    return DRG.Geirskogul;
                }


                if (level >= DRG.Levels.Geirskogul && IsOffCooldown(DRG.Geirskogul) && IsTargetInRangeGiven(15))
                {
                    if (GetCooldown(DRG.HighJump).CooldownRemaining >= 20)
                    {
                        return DRG.Geirskogul;
                    }

                    if (level < DRG.Levels.HighJump)
                    {
                        return DRG.Geirskogul;
                    }
                }

                if (level >= DRG.Levels.HighJump && IsOffCooldown(DRG.HighJump) && GetCurrentGcd(DRG.GCD_SKILL) >= 70 &&
                    IsTargetInRange())
                {
                    return DRG.HighJump;
                }

                if (level < DRG.Levels.HighJump && level >= DRG.Levels.Jump && IsOffCooldown(DRG.Jump) &&
                    GetCurrentGcd(DRG.GCD_SKILL) >= 70 && IsTargetInRange())
                {
                    return DRG.Jump;
                }

                if (level >= DRG.Levels.WyrmwindThrust && gauge.FirstmindsFocusCount == 2 && IsTargetInRangeGiven(15))
                    return DRG.WyrmwindThrust;


                if (level >= DRG.Levels.Nastond && IsOffCooldown(DRG.Nastond) && gauge.IsLOTDActive &&
                    IsTargetInRangeGiven(15))
                {
                    return DRG.Nastond;
                }

                if (level >= DRG.Levels.Stardiver && IsOffCooldown(DRG.Stardiver) && gauge.IsLOTDActive &&
                    GetCurrentGcd(DRG.GCD_SKILL) >= 70 && IsTargetInRange())
                {
                    return DRG.Stardiver;
                }

                if ((System.Numerics.Vector3.Distance(CurrentTarget.Position, LocalPlayer.Position) -
                     CurrentTarget.HitboxRadius) <= 4.5 && GetCurrentGcd(DRG.GCD_SKILL) >= 70)
                {
                    if (level >= DRG.Levels.DragonfireDive && IsOffCooldown(DRG.DragonfireDive) &&
                        HasEffect(DRG.Buffs.DragonSight))
                    {
                        return DRG.DragonfireDive;
                    }

                    if (level < DRG.Levels.DragonSight && level >= DRG.Levels.DragonfireDive &&
                        IsOffCooldown(DRG.DragonfireDive))
                    {
                        return DRG.DragonfireDive;
                    }

                    if (level >= DRG.Levels.SpineshatterDive && GetCooldown(DRG.SpineshatterDive).RemainingCharges > 0)
                    {
                        return DRG.SpineshatterDive;
                    }
                }
            }

            return actionID;
        }
    }

    internal class DragoonStardiver : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DrgAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == DRG.Stardiver)
            {
                var gauge = GetJobGauge<DRGGauge>();

                if (IsEnabled(CustomComboPreset.DragoonStardiverNastrondFeature))
                {
                    if (level >= DRG.Levels.Geirskogul && (!gauge.IsLOTDActive || IsOffCooldown(DRG.Nastrond) ||
                                                           IsOnCooldown(DRG.Stardiver)))
                        // Nastrond
                        return OriginalHook(DRG.Geirskogul);
                }

                if (IsEnabled(CustomComboPreset.DragoonStardiverDragonfireDiveFeature))
                {
                    if (level < DRG.Levels.Stardiver || !gauge.IsLOTDActive || IsOnCooldown(DRG.Stardiver) ||
                        (IsOffCooldown(DRG.DragonfireDive) && gauge.LOTDTimer > 7.5))
                        return DRG.DragonfireDive;
                }
            }

            return actionID;
        }
    }

    internal class DragoonDives : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.DragoonDiveFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == DRG.SpineshatterDive || actionID == DRG.DragonfireDive || actionID == DRG.Stardiver)
            {
                if (level >= DRG.Levels.Stardiver)
                {
                    var gauge = GetJobGauge<DRGGauge>();

                    if (gauge.IsLOTDActive)
                        return CalcBestAction(actionID, DRG.SpineshatterDive, DRG.DragonfireDive, DRG.Stardiver);

                    return CalcBestAction(actionID, DRG.SpineshatterDive, DRG.DragonfireDive);
                }

                if (level >= DRG.Levels.DragonfireDive)
                    return CalcBestAction(actionID, DRG.SpineshatterDive, DRG.DragonfireDive);

                if (level >= DRG.Levels.SpineshatterDive)
                    return DRG.SpineshatterDive;
            }

            return actionID;
        }
    }
}