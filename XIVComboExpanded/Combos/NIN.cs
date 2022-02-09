using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class NIN
    {
        public const double GDC = 0.55;
        public const uint GCD_SKILL = SpinningEdge;
        public const byte ClassID = 29;
        public const byte JobID = 30;

        public const uint
            SpinningEdge = 2240,
            GustSlash = 2242,
            Hide = 2245,
            Assassinate = 8814,
            Mug = 2248,
            DeathBlossom = 2254,
            AeolianEdge = 2255,
            TrickAttack = 2258,
            Ninjutsu = 2260,
            Chi = 2261,
            JinNormal = 2263,
            Kassatsu = 2264,
            ArmorCrush = 3563,
            DreamWithinADream = 3566,
            TenChiJin = 7403,
            HakkeMujinsatsu = 16488,
            Meisui = 16489,
            Jin = 18807,
            Bunshin = 16493,
            Huraijin = 25876,
            PhantomKamaitachi = 25774,
            ForkedRaiju = 25777,
            Bhavacakra = 7402,
            ThrowingDagger = 2247,
            FumaShuriken = 2265,
            Huton = 2269,
            katon = 2266,
            Raiton = 2267,
            Hyoton = 2268,
            Doton = 2270,
            Suiton = 2271,
            GokaMekkyaku = 16491,
            HyoshoRanryu = 16492,
            FleetingRaiju = 25778;

        public static class Buffs
        {
            public const ushort
                Mudra = 496,
                Kassatsu = 497,
                Suiton = 507,
                Hidden = 614,
                Bunshin = 1954,
                TenChiJin = 1186,
                RaijuReady = 2690;
        }

        public static class Debuffs
        {
            public const ushort
                Placeholder = 0;
        }

        public static class Levels
        {
            public const byte
                GustSlash = 4,
                Hide = 10,
                Mug = 15,
                AeolianEdge = 26,
                Ninjitsu = 30,
                Suiton = 45,
                HakkeMujinsatsu = 52,
                ArmorCrush = 54,
                Huraijin = 60,
                TenChiJin = 70,
                Meisui = 72,
                Kassatsu = 50,
                EnhancedKassatsu = 76,
                Bunshin = 80,
                ThrowingDagger = 15,
                Bhavacakra = 68,
                TrickAttack = 18,
                PhantomKamaitachi = 82,
                DreamWithinADream = 56,
                Raiju = 90;
        }
    }

    internal class NinjaAeolianEdge : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NinAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == NIN.AeolianEdge)
            {
                var gauge = GetJobGauge<NINGauge>();

                if ((System.Numerics.Vector3.Distance(CurrentTarget.Position, LocalPlayer.Position) -
                     CurrentTarget.HitboxRadius) >= 4.0)
                {
                    if (level >= NIN.Levels.ThrowingDagger)
                    {
                        return NIN.ThrowingDagger;
                    }
                }

                if (IsUnderGcd(NIN.GCD_SKILL))
                {
                    if (level >= NIN.Levels.Huraijin && IsOffCooldown(NIN.Huraijin) && gauge.HutonTimer <= 1000)
                    {
                        return NIN.Huraijin;
                    }
                    
                    if (level >= NIN.Levels.Raiju && HasEffect(NIN.Buffs.RaijuReady) && !HasEffect(NIN.Buffs.Mudra))
                    {
                        return NIN.ForkedRaiju;

                        //if (IsEnabled(CustomComboPreset.NinjaNinjitsuFleetingRaijuFeature))
                        //    return NIN.FleetingRaiju;
                    }


                    if (comboTime > 0)
                    {
                        if (lastComboMove == NIN.GustSlash && level >= NIN.Levels.AeolianEdge)
                        {
                            if (level >= NIN.Levels.ArmorCrush && IsOffCooldown(NIN.ArmorCrush) &&
                                gauge.HutonTimer <= 10000)
                            {
                                return NIN.ArmorCrush;
                            }
                            else
                            {
                                return NIN.AeolianEdge;
                            }
                        }

                        if (lastComboMove == NIN.SpinningEdge && level >= NIN.Levels.GustSlash)
                            return NIN.GustSlash;
                    }

                    return NIN.SpinningEdge;
                }


                if (!HasEffect(NIN.Buffs.Hidden))
                {
                    if (!HasEffect(NIN.Buffs.Kassatsu))
                    {
                        if (level >= NIN.Levels.TenChiJin && IsOffCooldown(NIN.TenChiJin))
                        {
                            return NIN.TenChiJin;
                        }
                    }

                    if (!HasEffect(NIN.Buffs.TenChiJin))
                    {
                        if (IsOffCooldown(NIN.FumaShuriken))
                        {
                            return NIN.FumaShuriken;
                        }

                        if (IsOffCooldown(NIN.Raiton))
                        {
                            return NIN.Raiton;
                        }

                        if (IsOffCooldown(NIN.Suiton))
                        {
                            return NIN.Suiton;
                        }

                        if (IsOffCooldown(NIN.Doton))
                        {
                            return NIN.Doton;
                        }

                        if (IsOffCooldown(NIN.katon))
                        {
                            return NIN.katon;
                        }
                    }

                    if (level >= NIN.Levels.Kassatsu && IsOffCooldown(NIN.Kassatsu))
                    {
                        return NIN.Kassatsu;
                    }


                    if (level >= NIN.Levels.Bunshin && IsOffCooldown(NIN.Bunshin) && gauge.Ninki >= 50)
                    {
                        return NIN.Bunshin;
                    }

                    if (level >= NIN.Levels.Meisui && IsOffCooldown(NIN.Meisui) && gauge.Ninki < 50 && HasEffect(NIN.Buffs.Suiton))
                    {
                        return NIN.Meisui;
                    }

                    if (level >= NIN.Levels.Bhavacakra && IsOffCooldown(NIN.Bhavacakra) && gauge.Ninki >= 50)
                    {
                        return NIN.Bhavacakra;
                    }

                    if (level >= NIN.Levels.Mug && IsOffCooldown(NIN.Mug) && gauge.Ninki < 40)
                    {
                        return NIN.Mug;
                    }

                    if (level >= NIN.Levels.Hide && IsOffCooldown(NIN.Hide) && IsOffCooldown(NIN.TrickAttack))
                    {
                        return NIN.Hide;
                    }


                    if (level >= NIN.Levels.DreamWithinADream && IsOffCooldown(NIN.DreamWithinADream))
                    {
                        return NIN.DreamWithinADream;
                    }
                }


                if (level >= NIN.Levels.TrickAttack && IsOffCooldown(NIN.TrickAttack) && HasEffect(NIN.Buffs.Hidden) ||
                    HasEffect(NIN.Buffs.Suiton))
                {
                    return NIN.TrickAttack;
                }
            }

            return actionID;
        }
    }

    internal class NinjaArmorCrush : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NinAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == NIN.ArmorCrush)
            {
                if (IsEnabled(CustomComboPreset.NinjaArmorCrushRaijuFeature))
                {
                    if (level >= NIN.Levels.Raiju && HasEffect(NIN.Buffs.RaijuReady))
                        return NIN.ForkedRaiju;
                }

                if (IsEnabled(CustomComboPreset.NinjaArmorCrushNinjutsuFeature))
                {
                    if (level >= NIN.Levels.Ninjitsu && HasEffect(NIN.Buffs.Mudra))
                        return OriginalHook(NIN.Ninjutsu);
                }

                if (IsEnabled(CustomComboPreset.NinjaArmorCrushCombo))
                {
                    if (comboTime > 0)
                    {
                        if (lastComboMove == NIN.GustSlash && level >= NIN.Levels.ArmorCrush)
                            return NIN.ArmorCrush;

                        if (lastComboMove == NIN.SpinningEdge && level >= NIN.Levels.GustSlash)
                            return NIN.GustSlash;
                    }

                    return NIN.SpinningEdge;
                }
            }

            return actionID;
        }
    }

    internal class NinjaHuraijin : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NinAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == NIN.Huraijin)
            {
                if (level >= NIN.Levels.Raiju && HasEffect(NIN.Buffs.RaijuReady))
                {
                    if (IsEnabled(CustomComboPreset.NinjaHuraijinForkedRaijuFeature))
                        return NIN.ForkedRaiju;

                    if (IsEnabled(CustomComboPreset.NinjaHuraijinFleetingRaijuFeature))
                        return NIN.FleetingRaiju;
                }

                if (IsEnabled(CustomComboPreset.NinjaHuraijinNinjutsuFeature))
                {
                    if (level >= NIN.Levels.Ninjitsu && HasEffect(NIN.Buffs.Mudra))
                        return OriginalHook(NIN.Ninjutsu);
                }

                if (IsEnabled(CustomComboPreset.NinjaHuraijinArmorCrushCombo))
                {
                    if (comboTime > 0)
                    {
                        if (lastComboMove == NIN.GustSlash && level >= NIN.Levels.ArmorCrush)
                            return NIN.ArmorCrush;
                    }
                }
            }

            return actionID;
        }
    }

    internal class NinjaHakkeMujinsatsu : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NinAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == NIN.HakkeMujinsatsu)
            {
                if (IsEnabled(CustomComboPreset.NinjaHakkeMujinsatsuNinjutsuFeature))
                {
                    if (level >= NIN.Levels.Ninjitsu && HasEffect(NIN.Buffs.Mudra))
                        return OriginalHook(NIN.Ninjutsu);
                }

                if (IsEnabled(CustomComboPreset.NinjaHakkeMujinsatsuCombo))
                {
                    if (comboTime > 0)
                    {
                        if (lastComboMove == NIN.DeathBlossom && level >= NIN.Levels.HakkeMujinsatsu)
                            return NIN.HakkeMujinsatsu;
                    }

                    return NIN.DeathBlossom;
                }
            }

            return actionID;
        }
    }

    internal class NinjaKassatsu : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NinjaKassatsuTrickFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == NIN.Kassatsu)
            {
                if ((level >= NIN.Levels.Hide && HasEffect(NIN.Buffs.Hidden)) ||
                    (level >= NIN.Levels.Suiton && HasEffect(NIN.Buffs.Suiton)))
                    return NIN.TrickAttack;
            }

            return actionID;
        }
    }

    internal class NinjaHide : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NinjaHideMugFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == NIN.Hide)
            {
                if (level >= NIN.Levels.Mug && HasCondition(ConditionFlag.InCombat))
                    return NIN.Mug;
            }

            return actionID;
        }
    }

    internal class NinjaChi : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NinjaKassatsuChiJinFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == NIN.Chi)
            {
                if (level >= NIN.Levels.EnhancedKassatsu && HasEffect(NIN.Buffs.Kassatsu))
                    return NIN.Jin;
            }

            return actionID;
        }
    }

    internal class NinjaTenChiJin : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.NinjaTCJMeisuiFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == NIN.TenChiJin)
            {
                if (level >= NIN.Levels.Meisui && HasEffect(NIN.Buffs.Suiton))
                    return NIN.Meisui;
            }

            return actionID;
        }
    }
}