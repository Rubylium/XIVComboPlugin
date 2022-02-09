using Dalamud.Game.ClientState.JobGauge.Types;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using FFXIVClientStructs.FFXIV.Client.UI;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class PLD
    {
        public const double GDC = 0.55;
        public const uint GCD_SKILL = GoringBlade;
        public const byte ClassID = 1;
        public const byte JobID = 19;

        public const uint
            FastBlade = 9,
            RiotBlade = 15,
            RageOfHalone = 21,
            CircleOfScorn = 23,
            SpiritsWithin = 29,
            ShieldLob = 24,
            GoringBlade = 3538,
            RoyalAuthority = 3539,
            TotalEclipse = 7381,
            Requiescat = 7383,
            HolySpirit = 7384,
            Prominence = 16457,
            HolyCircle = 16458,
            Confiteor = 16459,
            Atonement = 16460,
            Expiacion = 25747,
            BladeOfFaith = 25748,
            BladeOfTruth = 25749,
            FightOrFlight = 20,
            Intervene = 16461,
            Clemency = 3541,
            Sheltron = 3542,
            DivineVeil = 3540,
            BladeOfValor = 25750;

        public static class Buffs
        {
            public const ushort
                Requiescat = 1368,
                Sheltron = 1856,
                FightOrFlight = 76,
                SwordOath = 1902;
        }

        public static class Debuffs
        {
            public const ushort
                Reprisal = 1193,
                GloryBlade = 725;
        }

        public static class Levels
        {
            public const byte
                RiotBlade = 4,
                DivineVeil = 56,
                FightOrFlight = 2,
                SpiritsWithin = 30,
                CircleOfScorn = 50,
                RageOfHalone = 26,
                Intervene = 74,
                Prominence = 40,
                GoringBlade = 54,
                RoyalAuthority = 60,
                HolyCircle = 72,
                HolySpirit = 64,
                Sheltron = 35,
                Atonement = 76,
                Confiteor = 80,
                Expiacion = 86,
                Requiescat = 68,
                BladeOfFaith = 90,
                BladeOfTruth = 90,
                Clemency = 58,
                BladeOfValor = 90;
        }
    }

    internal class PaladinGoringBlade : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PldAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.FastBlade)
            {
                var gauge = GetJobGauge<PLDGauge>();
                if (IsUnderGcd(PLD.GCD_SKILL))
                    //if (GetCooldown(PLD.GoringBlade).CooldownRemaining <= PLD.GDC)
                {
                    var mpToAdd = 0;
                    if (level < 64)
                    {
                        mpToAdd = 2;
                    }

                    if (level >= PLD.Levels.Atonement && HasEffect(PLD.Buffs.SwordOath) && IsTargetInRange())
                        return PLD.Atonement;

                    if (level >= PLD.Levels.HolySpirit && HasEffect(PLD.Buffs.Requiescat) &&
                        FindEffect(PLD.Buffs.Requiescat).StackCount > 1 && GetPlayerManaExact() >= (1000 * mpToAdd))
                        return PLD.HolySpirit;

                    if (level >= PLD.Levels.HolySpirit && level < PLD.Levels.Confiteor &&
                        HasEffect(PLD.Buffs.Requiescat) &&
                        FindEffect(PLD.Buffs.Requiescat).StackCount == 1 && GetPlayerManaExact() >= (1000 * mpToAdd))
                        return PLD.HolySpirit;

                    if (level >= PLD.Levels.Confiteor && HasEffect(PLD.Buffs.Requiescat) &&
                        FindEffect(PLD.Buffs.Requiescat).StackCount == 1 && GetPlayerManaExact() >= (1000 * mpToAdd))
                        return PLD.Confiteor;

                    if (level >= PLD.Levels.Clemency && GetPlayerHealth() <= 50 &&
                        GetPlayerManaExact() >= (2000 * mpToAdd))
                        return PLD.Clemency;

                    if (!IsTargetInRange())
                    {
                        return PLD.ShieldLob;
                    }

                    if (level >= PLD.Levels.GoringBlade && !TargetHasEffect(PLD.Debuffs.GloryBlade) ||
                        FindTargetEffect(PLD.Debuffs.GloryBlade).RemainingTime <= 7 && IsTargetInRange() &&
                        GetCooldown(PLD.FightOrFlight).CooldownRemaining > 10)
                    {
                        if (comboTime > 0)
                        {
                            if (lastComboMove == PLD.RiotBlade && level >= PLD.Levels.GoringBlade)
                                return PLD.GoringBlade;

                            if (lastComboMove == PLD.FastBlade && level >= PLD.Levels.RiotBlade)
                                return PLD.RiotBlade;
                        }

                        return PLD.FastBlade;
                    }

                    if (comboTime > 0 && IsTargetInRange())
                    {
                        if (lastComboMove == PLD.RiotBlade && level >= PLD.Levels.RageOfHalone)
                            // Royal Authority
                            return OriginalHook(PLD.RageOfHalone);

                        if (lastComboMove == PLD.FastBlade && level >= PLD.Levels.RiotBlade)
                            return PLD.RiotBlade;
                    }

                    return PLD.FastBlade;
                }

                if (level >= PLD.Levels.FightOrFlight && IsOffCooldown(PLD.FightOrFlight) &&
                    !HasEffect(PLD.Buffs.Requiescat))
                {
                    return PLD.FightOrFlight;
                }

                if (level >= ALL.Levels.Interject && CanInterruptEnemy() && !GetCooldown(ALL.Interject).IsCooldown &&
                    IsTargetInRange())
                {
                    return ALL.Interject;
                }

                if (level >= PLD.Levels.CircleOfScorn && IsOffCooldown(PLD.CircleOfScorn) && IsTargetInRange())
                {
                    return PLD.CircleOfScorn;
                }


                if (level >= PLD.Levels.SpiritsWithin && IsOffCooldown(PLD.SpiritsWithin) && IsTargetInRange())
                {
                    return PLD.SpiritsWithin;
                }

                if (level >= PLD.Levels.Intervene && GetCooldown(PLD.Intervene).RemainingCharges >= 1 &&
                    IsTargetInRange())
                {
                    return PLD.Intervene;
                }


                if (level >= PLD.Levels.Requiescat && IsOffCooldown(PLD.Requiescat) &&
                    !HasEffect(PLD.Buffs.SwordOath) && IsTargetInRange() &&
                    !HasEffect(PLD.Buffs.FightOrFlight))
                {
                    return PLD.Requiescat;
                }

                if (level >= ALL.Levels.Reprisal && IsOffCooldown(ALL.Reprisal) &&
                    IsTargetInRange())
                {
                    return ALL.Reprisal;
                }

                if (level >= PLD.Levels.Sheltron && IsOffCooldown(PLD.Sheltron) && gauge.OathGauge >= 50 &&
                    !TargetHasEffect(PLD.Debuffs.Reprisal))
                {
                    if (IsEnemyCasting() && GetEnemyCastingTimeRemaining() <= 3 && GetEnemyCastingTimeRemaining() >= 1)
                        return OriginalHook(PLD.Sheltron);
                }

                if (level >= PLD.Levels.DivineVeil && IsOffCooldown(PLD.DivineVeil) && GetPlayerHealth() <= 60 &&
                    !TargetHasEffect(PLD.Debuffs.Reprisal))
                {
                    return PLD.DivineVeil;
                }
            }

            return actionID;
        }
    }

    internal class PaladinRoyalAuthority : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PldAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.RageOfHalone || actionID == PLD.RoyalAuthority)
            {
                if (IsEnabled(CustomComboPreset.PaladinRoyalAuthorityAtonementFeature))
                {
                    if (level >= PLD.Levels.Atonement && HasEffect(PLD.Buffs.SwordOath))
                        return PLD.Atonement;
                }

                if (IsEnabled(CustomComboPreset.PaladinRoyalAuthorityCombo))
                {
                    if (comboTime > 0)
                    {
                        if (lastComboMove == PLD.RiotBlade && level >= PLD.Levels.RageOfHalone)
                            // Royal Authority
                            return OriginalHook(PLD.RageOfHalone);

                        if (lastComboMove == PLD.FastBlade && level >= PLD.Levels.RiotBlade)
                            return PLD.RiotBlade;
                    }

                    return PLD.FastBlade;
                }
            }

            return actionID;
        }
    }

    internal class PaladinProminence : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PaladinProminenceCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.Prominence)
            {
                var mpToAdd = 0;
                if (level < 64)
                {
                    mpToAdd = 2;
                }

                var gauge = GetJobGauge<PLDGauge>();
                if (GetCooldown(PLD.GoringBlade).CooldownRemaining <= PLD.GDC)
                {
                    if (level >= PLD.Levels.HolyCircle && HasEffect(PLD.Buffs.Requiescat) &&
                        FindEffect(PLD.Buffs.Requiescat).StackCount > 1 && GetPlayerManaExact() >= (1000 * mpToAdd))
                        return PLD.HolyCircle;

                    if (level >= PLD.Levels.HolyCircle && level < PLD.Levels.Confiteor &&
                        HasEffect(PLD.Buffs.Requiescat) &&
                        FindEffect(PLD.Buffs.Requiescat).StackCount == 1 && GetPlayerManaExact() >= (1000 * mpToAdd))
                        return PLD.HolyCircle;

                    if (level >= PLD.Levels.Confiteor && HasEffect(PLD.Buffs.Requiescat) &&
                        FindEffect(PLD.Buffs.Requiescat).StackCount == 1 && GetPlayerManaExact() >= (1000 * mpToAdd))
                        return PLD.Confiteor;

                    if (level >= PLD.Levels.Clemency && GetPlayerHealth() <= 60 &&
                        GetPlayerManaExact() >= (2000 * mpToAdd))
                        return PLD.Clemency;

                    if (comboTime > 0)
                    {
                        if (lastComboMove == PLD.TotalEclipse && level >= PLD.Levels.Prominence)
                            return PLD.Prominence;
                    }

                    return PLD.TotalEclipse;
                }


                if (level >= PLD.Levels.FightOrFlight && IsOffCooldown(PLD.FightOrFlight) &&
                    !HasEffect(PLD.Buffs.Requiescat))
                {
                    return PLD.FightOrFlight;
                }

                if (level >= PLD.Levels.Sheltron && IsOffCooldown(PLD.Sheltron) &&
                    !HasEffect(PLD.Buffs.Sheltron) && gauge.OathGauge >= 50)
                {
                    return OriginalHook(PLD.Sheltron);
                }

                if (level >= ALL.Levels.Interject && CanInterruptEnemy() && !GetCooldown(ALL.Interject).IsCooldown &&
                    IsTargetInRange())
                {
                    return ALL.Interject;
                }

                if (level >= PLD.Levels.CircleOfScorn && IsOffCooldown(PLD.CircleOfScorn) && IsTargetInRange())
                {
                    return PLD.CircleOfScorn;
                }

                if (level >= PLD.Levels.Intervene && GetCooldown(PLD.Intervene).RemainingCharges >= 1 &&
                    IsTargetInRange())
                {
                    return PLD.Intervene;
                }

                if (level >= PLD.Levels.Requiescat && IsOffCooldown(PLD.Requiescat) &&
                    !HasEffect(PLD.Buffs.SwordOath) && IsTargetInRange() &&
                    !HasEffect(PLD.Buffs.FightOrFlight))
                {
                    return PLD.Requiescat;
                }

                if (level >= PLD.Levels.SpiritsWithin && IsOffCooldown(PLD.SpiritsWithin) && IsTargetInRange())
                {
                    return PLD.SpiritsWithin;
                }


                if (level >= ALL.Levels.Reprisal && IsOffCooldown(ALL.Reprisal) &&
                    IsTargetInRange())
                {
                    return ALL.Reprisal;
                }
            }


            return actionID;
        }
    }

    internal class PaladinHolySpiritHolyCircle : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PaladinConfiteorFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.HolySpirit || actionID == PLD.HolyCircle)
            {
                if (lastComboMove == PLD.BladeOfTruth && level >= PLD.Levels.BladeOfValor)
                    return PLD.BladeOfValor;

                if (lastComboMove == PLD.BladeOfFaith && level >= PLD.Levels.BladeOfTruth)
                    return PLD.BladeOfTruth;

                if (lastComboMove == PLD.Confiteor && level >= PLD.Levels.BladeOfFaith)
                    return PLD.BladeOfFaith;

                if (level >= PLD.Levels.Confiteor)
                {
                    var requiescat = FindEffect(PLD.Buffs.Requiescat);
                    if (requiescat != null && (requiescat.StackCount == 1 || LocalPlayer?.CurrentMp < 2000))
                        return PLD.Confiteor;
                }
            }

            return actionID;
        }
    }

    internal class PaladinRequiescat : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PaladinRequiescatCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.Requiescat)
            {
                if (lastComboMove == PLD.BladeOfTruth && level >= PLD.Levels.BladeOfValor)
                    return PLD.BladeOfValor;

                if (lastComboMove == PLD.BladeOfFaith && level >= PLD.Levels.BladeOfTruth)
                    return PLD.BladeOfTruth;

                if (lastComboMove == PLD.Confiteor && level >= PLD.Levels.BladeOfFaith)
                    return PLD.BladeOfFaith;

                if (level >= PLD.Levels.Confiteor)
                {
                    var requiescat = FindEffect(PLD.Buffs.Requiescat);
                    if (requiescat != null)
                        return PLD.Confiteor;
                }
            }

            return actionID;
        }
    }

    internal class PaladinSpiritsWithinCircleOfScorn : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.PaladinScornfulSpiritsFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == PLD.SpiritsWithin || actionID == PLD.Expiacion || actionID == PLD.CircleOfScorn)
            {
                if (level >= PLD.Levels.Expiacion)
                    return CalcBestAction(actionID, PLD.Expiacion, PLD.CircleOfScorn);

                if (level >= PLD.Levels.CircleOfScorn)
                    return CalcBestAction(actionID, PLD.SpiritsWithin, PLD.CircleOfScorn);

                return PLD.SpiritsWithin;
            }

            return actionID;
        }
    }
}