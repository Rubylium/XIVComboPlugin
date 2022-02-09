using Dalamud.Game.ClientState.JobGauge.Types;
using FFXIVClientStructs.FFXIV.Client.Graphics;
using Lumina.Data.Parsing;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class WAR
    {
        public const double GDC = 0.55;
        public const uint GCD_SKILL = HeavySwing;
        public const byte ClassID = 3;
        public const byte JobID = 21;

        public const uint
            HeavySwing = 31,
            Maim = 37,
            Berserk = 38,
            Overpower = 41,
            StormsPath = 42,
            StormsEye = 45,
            InnerBeast = 49,
            SteelCyclone = 51,
            Infuriate = 52,
            FellCleave = 3549,
            Decimate = 3550,
            RawIntuition = 3551,
            InnerRelease = 7389,
            MythrilTempest = 16462,
            ChaoticCyclone = 16463,
            NascentFlash = 16464,
            InnerChaos = 16465,
            Upheaval = 7387,
            Onslaugth = 7386,
            Tomahawk = 46,
            Orogeny = 25752,
            Interject = 7538,
            Equilibrium = 3552,
            Bloodwhetting = 25751,
            ThrillOfBattle = 40,
            Vengeance = 44,
            ShakeItOff = 7388,
            PrimalRend = 25753;

        public static class Buffs
        {
            public const ushort
                Berserk = 86,
                InnerRelease = 1177,
                NascentChaos = 1897,
                PrimalRendReady = 2624,
                Vengeance = 89,
                SurgingTempest = 2677;
        }

        public static class Debuffs
        {
            public const ushort
                Placeholder = 0;
        }

        public static class Levels
        {
            public const byte
                Maim = 4,
                Berserk = 6,
                StormsPath = 26,
                InnerBeast = 35,
                MythrilTempest = 40,
                StormsEye = 50,
                Infuriate = 50,
                FellCleave = 54,
                Decimate = 60,
                InnerRelease = 70,
                MythrilTempestTrait = 74,
                NascentFlash = 76,
                InnerChaos = 80,
                Upheaval = 64,
                Onslaugth = 62,
                Tomahawk = 15,
                Orogeny = 86,
                Interject = 18,
                Equilibrium = 58,
                Bloodwhetting = 82,
                RawIntuition = 56,
                ThrillOfBattle = 30,
                Vengeance = 38,
                ShakeItOff = 68,
                PrimalRend = 90;
        }
    }

    internal class WarriorStormsPathCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WarriorStormsPathCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WAR.StormsPath)
            {
                var gauge = GetJobGauge<WARGauge>();
                if ((System.Numerics.Vector3.Distance(CurrentTarget.Position, LocalPlayer.Position) -
                     CurrentTarget.HitboxRadius) >= 4.0)
                {
                    if (level >= WAR.Levels.Tomahawk)
                    {
                        return WAR.Tomahawk;
                    }
                }


                if (IsUnderGcd(WAR.GCD_SKILL))
                {
                    if (level >= WAR.Levels.InnerRelease && HasEffect(WAR.Buffs.InnerRelease))
                        return WAR.FellCleave;

                    if (level >= WAR.Levels.PrimalRend && IsOffCooldown(WAR.PrimalRend) &&
                        HasEffect(WAR.Buffs.PrimalRendReady))
                    {
                        return WAR.PrimalRend;
                    }

                    if (comboTime > 0)
                    {
                        if (lastComboMove == WAR.Maim && level >= WAR.Levels.StormsEye &&
                            !HasEffect(WAR.Buffs.SurgingTempest) && !HasEffect(WAR.Buffs.InnerRelease))
                            return WAR.StormsEye;

                        if (lastComboMove == WAR.Maim && level >= WAR.Levels.StormsEye &&
                            FindEffect(WAR.Buffs.SurgingTempest).RemainingTime <= 15 &&
                            !HasEffect(WAR.Buffs.InnerRelease))
                            return WAR.StormsEye;

                        if (lastComboMove == WAR.Maim && level >= WAR.Levels.StormsPath &&
                            !HasEffect(WAR.Buffs.InnerRelease))
                        {
                            if (level >= WAR.Levels.FellCleave && gauge.BeastGauge >= 50)
                            {
                                return WAR.FellCleave;
                            }

                            if (level < WAR.Levels.FellCleave && level >= WAR.Levels.InnerBeast &&
                                gauge.BeastGauge >= 50)
                            {
                                return WAR.InnerBeast;
                            }


                            return WAR.StormsPath;
                        }


                        if (lastComboMove == WAR.HeavySwing && level >= WAR.Levels.Maim &&
                            !HasEffect(WAR.Buffs.InnerRelease))
                        {
                            if (IsEnabled(CustomComboPreset.WarriorStormsPathOvercapFeature))
                            {
                                if (level >= WAR.Levels.InnerBeast && gauge.BeastGauge >= 50)
                                    // Fell Cleave
                                    return OriginalHook(WAR.InnerBeast);
                            }

                            return WAR.Maim;
                        }
                    }

                    if (level >= WAR.Levels.InnerChaos && HasEffect(WAR.Buffs.NascentChaos) &&
                        !HasEffect(WAR.Buffs.InnerRelease) && gauge.BeastGauge >= 50)
                    {
                        return WAR.InnerChaos;
                    }

                    return WAR.HeavySwing;
                }

                if (level >= WAR.Levels.Interject && CanInterruptEnemy() && !GetCooldown(WAR.Interject).IsCooldown)
                {
                    return WAR.Interject;
                }

                if (level >= ALL.Levels.Reprisal && IsOffCooldown(ALL.Reprisal) &&
                    IsTargetInRange())
                {
                    if (IsEnemyCasting() && GetEnemyCastingTimeRemaining() <= 3 && GetEnemyCastingTimeRemaining() >= 1)
                        return ALL.Reprisal;
                }

                if (level >= ALL.Levels.Rampart && IsOffCooldown(ALL.Rampart) &&
                    IsTargetInRange() && !TargetHasEffect(ALL.Debuffs.Reprisal))
                {
                    if (IsEnemyCasting() && GetEnemyCastingTimeRemaining() <= 3 && GetEnemyCastingTimeRemaining() >= 1)
                        return ALL.Rampart;
                }

                if (level >= WAR.Levels.Vengeance && IsOffCooldown(WAR.Vengeance) &&
                    IsTargetInRange() && !TargetHasEffect(ALL.Debuffs.Reprisal) && !HasEffect(ALL.Buffs.Rampart))
                {
                    if (IsEnemyCasting() && GetEnemyCastingTimeRemaining() <= 3 && GetEnemyCastingTimeRemaining() >= 1)
                        return WAR.Vengeance;
                }

                if (level >= WAR.Levels.ShakeItOff && IsOffCooldown(WAR.ShakeItOff) &&
                    IsTargetInRange() && !TargetHasEffect(ALL.Debuffs.Reprisal) && !HasEffect(ALL.Buffs.Rampart) &&
                    !HasEffect(WAR.Buffs.Vengeance))
                {
                    if (IsEnemyCasting() && GetEnemyCastingTimeRemaining() <= 3 && GetEnemyCastingTimeRemaining() >= 1)
                        return WAR.ShakeItOff;
                }


                if (GetPlayerHealth() <= 70 &&
                    IsOffCooldown(WAR.RawIntuition) && level >= WAR.Levels.RawIntuition)
                {
                    return OriginalHook(WAR.RawIntuition);
                }


                if (GetPlayerHealth() <= 60 &&
                    IsOffCooldown(WAR.ThrillOfBattle) && level >= WAR.Levels.ThrillOfBattle)
                {
                    return WAR.ThrillOfBattle;
                }

                if (GetPlayerHealth() <= 40 &&
                    IsOffCooldown(WAR.Equilibrium) && level >= WAR.Levels.Equilibrium)
                {
                    return WAR.Equilibrium;
                }


                if (!HasEffect(WAR.Buffs.InnerRelease))
                {
                    if ((System.Numerics.Vector3.Distance(CurrentTarget.Position, LocalPlayer.Position) -
                         CurrentTarget.HitboxRadius) <= 4.0)
                    {
                        if (level >= WAR.Levels.Onslaugth &&
                            GetCooldown(WAR.Onslaugth).RemainingCharges > 1 &&
                            GetCooldown(WAR.HeavySwing).CooldownRemaining <= WAR.GDC &&
                            !HasEffect(WAR.Buffs.InnerRelease))
                        {
                            return WAR.Onslaugth;
                        }
                    }


                    if (level >= WAR.Levels.InnerRelease && IsOffCooldown(WAR.InnerRelease))
                    {
                        return WAR.InnerRelease;
                    }

                    if (level < WAR.Levels.InnerRelease && level >= WAR.Levels.Berserk && IsOffCooldown(WAR.Berserk))
                    {
                        return WAR.Berserk;
                    }

                    if (level >= WAR.Levels.Infuriate &&
                        GetCooldown(WAR.Infuriate).RemainingCharges > 0 && !HasEffect(WAR.Buffs.NascentChaos) &&
                        gauge.BeastGauge <= 50)
                    {
                        return WAR.Infuriate;
                    }

                    if (level >= WAR.Levels.Upheaval && IsOffCooldown(WAR.Upheaval))
                    {
                        return WAR.Upheaval;
                    }


                    if (level >= WAR.Levels.Onslaugth &&
                        GetCooldown(WAR.Onslaugth).RemainingCharges > 0)
                    {
                        return WAR.Onslaugth;
                    }
                }
            }

            return actionID;
        }
    }

    internal class WarriorStormsEyeCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WarriorStormsEyeCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WAR.StormsEye)
            {
                if (comboTime > 0)
                {
                    if (lastComboMove == WAR.Maim && level >= WAR.Levels.StormsEye)
                        return WAR.StormsEye;

                    if (lastComboMove == WAR.HeavySwing && level >= WAR.Levels.Maim)
                        return WAR.Maim;
                }

                return WAR.HeavySwing;
            }

            return actionID;
        }
    }

    internal class WarriorMythrilTempestCombo : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WarriorMythrilTempestCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WAR.MythrilTempest)
            {
                var gauge = GetJobGauge<WARGauge>();
                if (IsUnderGcd(WAR.GCD_SKILL))
                    //if (GetCooldown(WAR.HeavySwing).CooldownRemaining <= WAR.GDC)
                {
                    if (level >= WAR.Levels.Decimate && gauge.BeastGauge >= 50)
                        return WAR.Decimate;

                    if (comboTime > 0)
                    {
                        if (lastComboMove == WAR.Overpower && level >= WAR.Levels.MythrilTempest)
                        {
                            return WAR.MythrilTempest;
                        }
                    }

                    return WAR.Overpower;
                }


                if (level >= ALL.Levels.Reprisal && IsOffCooldown(ALL.Reprisal) &&
                    IsTargetInRange() && !HasEffect(ALL.Buffs.Vulndown))
                {
                    return ALL.Reprisal;
                }


                if (GetPlayerHealth() <= 70 &&
                    IsOffCooldown(WAR.ThrillOfBattle) && level >= WAR.Levels.ThrillOfBattle)
                {
                    return WAR.ThrillOfBattle;
                }

                if (GetPlayerHealth() <= 60 &&
                    IsOffCooldown(WAR.RawIntuition) && level >= WAR.Levels.RawIntuition)
                {
                    return OriginalHook(WAR.RawIntuition);
                }

                if (GetPlayerHealth() <= 40 &&
                    IsOffCooldown(WAR.Equilibrium) && level >= WAR.Levels.Equilibrium)
                {
                    return WAR.Equilibrium;
                }


                if (level >= WAR.Levels.InnerRelease && HasEffect(WAR.Buffs.InnerRelease))
                    return WAR.Decimate;

                if (level >= WAR.Levels.Orogeny && IsOffCooldown(WAR.Orogeny))
                    return WAR.Orogeny;


                if (level >= WAR.Levels.Vengeance && IsOffCooldown(WAR.Vengeance) &&
                    IsTargetInRange() && !TargetHasEffect(ALL.Debuffs.Reprisal) && !HasEffect(ALL.Buffs.Rampart))
                {
                    return WAR.Vengeance;
                }

                if (level < WAR.Levels.Orogeny && level >= WAR.Levels.Upheaval && IsOffCooldown(WAR.Upheaval))
                {
                    return WAR.Upheaval;
                }


                if (level >= ALL.Levels.Rampart && IsOffCooldown(ALL.Rampart) &&
                    IsTargetInRange() && !TargetHasEffect(ALL.Debuffs.Reprisal) && !HasEffect(ALL.Buffs.Vulndown))
                {
                    return ALL.Rampart;
                }

                if (level >= WAR.Levels.ShakeItOff && IsOffCooldown(WAR.ShakeItOff) &&
                    IsTargetInRange() && !TargetHasEffect(ALL.Debuffs.Reprisal) && !HasEffect(ALL.Buffs.Rampart) &&
                    !HasEffect(WAR.Buffs.Vengeance))
                {
                    if (GetPlayerHealth() <= 80)
                        return WAR.ShakeItOff;
                }
            }

            return actionID;
        }
    }

    internal class WarriorNascentFlashFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WarriorNascentFlashFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WAR.NascentFlash)
            {
                if (level >= WAR.Levels.NascentFlash)
                    return WAR.NascentFlash;

                return WAR.RawIntuition;
            }

            return actionID;
        }
    }

    internal class WarriorFellCleaveDecimate : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WarAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WAR.InnerBeast || actionID == WAR.FellCleave || actionID == WAR.SteelCyclone ||
                actionID == WAR.Decimate)
            {
                if (IsEnabled(CustomComboPreset.WarriorPrimalBeastFeature))
                {
                    if (level >= WAR.Levels.PrimalRend && HasEffect(WAR.Buffs.PrimalRendReady))
                        return WAR.PrimalRend;
                }

                if (IsEnabled(CustomComboPreset.WarriorInfuriateBeastFeature))
                {
                    var gauge = GetJobGauge<WARGauge>();

                    if (level >= WAR.Levels.Infuriate && gauge.BeastGauge < 50 && !HasEffect(WAR.Buffs.InnerRelease))
                        return WAR.Infuriate;
                }
            }

            return actionID;
        }
    }

    internal class WArriorPrimalReleaseFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WarriorPrimalReleaseFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WAR.Berserk || actionID == WAR.InnerRelease)
            {
                if (level >= WAR.Levels.PrimalRend && HasEffect(WAR.Buffs.PrimalRendReady))
                    return WAR.PrimalRend;
            }

            return actionID;
        }
    }
}