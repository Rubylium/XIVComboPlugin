using System.Net.Sockets;
using System.Threading;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Logging;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class SAM
    {
        public const double GDC = 0.55;
        public const uint GCD_SKILL = Yukikaze;
        public const byte JobID = 34;

        public const uint
            // Single target
            Hakaze = 7477,
            Enpi = 7486,
            Jinpu = 7478,
            Shifu = 7479,
            Yukikaze = 7480,
            Gekko = 7481,
            Kasha = 7482,
            // AoE
            Fuga = 7483,
            Mangetsu = 7484,
            Oka = 7485,
            Fuko = 25780,
            // Iaijutsu and Tsubame
            Iaijutsu = 7867,
            TsubameGaeshi = 16483,
            KaeshiHiganbana = 16484,
            Shoha = 16487,
            // Misc
            HissatsuShinten = 7490,
            Hagakure = 7495,
            HissatsuKyuten = 7491,
            HissatsuSenei = 16481,
            HissatsuGuren = 7496,
            HissatsuKaiten = 7494,
            KaeshiSetsugekka = 16486,
            Ikishoten = 16482,
            Shoha2 = 25779,
            OgiNamikiri = 25781,
            MidareSetsugekka = 7487,
            Higanbana = 7489,
            TenkaGoken = 7488,
            MeikyoShisui = 7499,
            Feint = 7549,
            TrueNorth = 7546,
            KaeshiNamikiri = 25782;

        public static class Buffs
        {
            public const ushort
                MeikyoShisui = 1233,
                EyesOpen = 1252,
                Jinpu = 1298,
                Shifu = 1299,
                TrueNorth = 1250,
                Kaiten = 1229,
                OgiNamikiriReady = 2959;
        }

        public static class Debuffs
        {
            public const ushort
                Higanbana = 1228;
        }

        public static class Levels
        {
            public const byte
                Jinpu = 4,
                Enpi = 15,
                Shifu = 18,
                Gekko = 30,
                Mangetsu = 35,
                Kasha = 40,
                Oka = 45,
                Yukikaze = 50,
                MeikyoShisui = 50,
                HissatsuKyuten = 64,
                HissatsuGuren = 70,
                HissatsuSenei = 72,
                HissatsuKaiten = 52,
                HissatsuShinten = 62,
                Hagakure = 68,
                TsubameGaeshi = 76,
                KaeshiHiganbana = 76,
                KaeshiSetsugekka = 76,
                Ikishoten = 68,
                Shoha = 80,
                Shoha2 = 82,
                Hyosetsu = 86,
                Fuko = 86,
                Iaijutsu = 30,
                Feint = 22,
                TrueNorth = 50,
                KaeshiNamikiri = 90,
                OgiNamikiri = 90;
        }
    }

    internal class SamuraiYukikaze : CustomCombo
    {
        private bool CanUseThing = false;
        private bool isMoving = false;
        private double isMovingOffset = 70;
        private double xOldPos = 0.0;
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamuraiYukikazeCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.Yukikaze)
            {
                var gauge = GetJobGauge<SAMGauge>();
                var ShouldUseHakagure = false;
                if (LocalPlayer != null)
                {
                    var newXPos = LocalPlayer.Position.X;
                    if (newXPos != this.xOldPos)
                    {
                        this.isMovingOffset = 70;
                        this.isMoving = true;
                    }

                    if (this.isMoving && isMovingOffset >= 0)
                    {
                        PluginLog.Log("isMovingOffset = " + this.isMovingOffset);
                        this.isMovingOffset = this.isMovingOffset - 1;
                    }

                    if (this.isMovingOffset <= 0)
                    {
                        this.isMoving = false;
                    }

                    xOldPos = newXPos;
                }

                //PluginLog.Log("Is moving ? " + isMoving);

                var sens = 0;
                if (gauge.HasGetsu)
                {
                    sens = sens + 1;
                }

                if (gauge.HasKa)
                {
                    sens = sens + 1;
                }

                if (gauge.HasSetsu)
                {
                    sens = sens + 1;
                }

                //if (!HasCondition(ConditionFlag.InCombat) || CurrentTarget == null)
                //    CanUseThing = false;
                //else if (lastComboMove == SAM.Yukikaze)
                //    CanUseThing = true;

                if (CurrentTarget != null && LocalPlayer != null)
                {
                    if ((System.Numerics.Vector3.Distance(CurrentTarget.Position, LocalPlayer.Position) -
                         CurrentTarget.HitboxRadius) >= 4.0)
                    {
                        if (level >= SAM.Levels.Enpi)
                        {
                            return SAM.Enpi;
                        }
                    }
                }

                if (IsUnderGcd(SAM.GCD_SKILL))
                {
                    if (level >= SAM.Levels.KaeshiSetsugekka && gauge.Kaeshi == Kaeshi.SETSUGEKKA &&
                        GetCooldown(SAM.KaeshiSetsugekka).RemainingCharges > 0)
                    {
                        return SAM.KaeshiSetsugekka;
                    }

                    if (level >= SAM.Levels.KaeshiHiganbana && gauge.Kaeshi == Kaeshi.HIGANBANA &&
                        GetCooldown(SAM.KaeshiHiganbana).RemainingCharges > 0)
                    {
                        return SAM.KaeshiHiganbana;
                    }

                    if (level >= SAM.Levels.KaeshiNamikiri && gauge.Kaeshi == Kaeshi.NAMIKIRI)
                    {
                        return SAM.KaeshiNamikiri;
                    }

                    if (level >= SAM.Levels.OgiNamikiri && HasEffect(SAM.Buffs.OgiNamikiriReady) && !this.isMoving)
                    {
                        return SAM.OgiNamikiri;
                    }

                    if (!gauge.HasSetsu)
                    {
                        if (level >= SAM.Levels.MeikyoShisui && HasEffect(SAM.Buffs.MeikyoShisui))
                            return SAM.Yukikaze;

                        if (comboTime > 0)
                        {
                            if (lastComboMove == SAM.Hakaze && level >= SAM.Levels.Yukikaze)
                                return SAM.Yukikaze;
                        }

                        return SAM.Hakaze;
                    }

                    if (!isMoving)
                    {
                        if (GetCooldown(SAM.TsubameGaeshi).RemainingCharges > 0 ||
                            GetCooldown(SAM.TsubameGaeshi).ChargeCooldownRemaining > 8)
                        {
                            if (level >= SAM.Levels.Iaijutsu && !TargetHasEffect(SAM.Debuffs.Higanbana) &&
                                gauge.HasSetsu)
                            {
                                return SAM.Higanbana;
                            }

                            if (level >= SAM.Levels.Iaijutsu && FindTargetEffect(SAM.Debuffs.Higanbana) != null &&
                                FindTargetEffect(SAM.Debuffs.Higanbana).RemainingTime <= 7 &&
                                gauge.HasSetsu && sens <= 1)
                            {
                                return SAM.Higanbana;
                            }
                        }
                        else
                        {
                            // if (level >= SAM.Levels.Hagakure &&
                            // GetCooldown(SAM.TsubameGaeshi).ChargeCooldownRemaining < 8 &&
                            //  IsOffCooldown(SAM.Hagakure))
                            // {
                            // ShouldUseHakagure = true;
                            //return SAM.Hagakure;
                            // }
                        }
                    }


                    if (!gauge.HasGetsu)
                    {
                        if (level >= SAM.Levels.MeikyoShisui && HasEffect(SAM.Buffs.MeikyoShisui))
                            return SAM.Gekko;

                        if (comboTime > 0)
                        {
                            if (lastComboMove == SAM.Jinpu && level >= SAM.Levels.Gekko)
                                return SAM.Gekko;

                            if (lastComboMove == SAM.Hakaze && level >= SAM.Levels.Jinpu)
                                return SAM.Jinpu;
                        }

                        return SAM.Hakaze;
                    }

                    if (!gauge.HasKa)
                    {
                        if (level >= SAM.Levels.MeikyoShisui && HasEffect(SAM.Buffs.MeikyoShisui))
                            return SAM.Kasha;

                        if (comboTime > 0)
                        {
                            if (lastComboMove == SAM.Shifu && level >= SAM.Levels.Kasha)
                                return SAM.Kasha;

                            if (lastComboMove == SAM.Hakaze && level >= SAM.Levels.Shifu)
                                return SAM.Shifu;
                        }

                        return SAM.Hakaze;
                    }

                    if (!isMoving)
                    {
                        if (GetCooldown(SAM.TsubameGaeshi).RemainingCharges > 0 ||
                            GetCooldown(SAM.TsubameGaeshi).ChargeCooldownRemaining > 8)
                        {
                            if (level >= SAM.Levels.Iaijutsu && gauge.HasGetsu && gauge.HasKa && gauge.HasSetsu)
                            {
                                return SAM.MidareSetsugekka;
                            }
                        }
                        else
                        {
                            // if (level >= SAM.Levels.Hagakure &&
                            //    GetCooldown(SAM.TsubameGaeshi).ChargeCooldownRemaining < 8 &&
                            //    IsOffCooldown(SAM.Hagakure))
                            //{
                            // return SAM.Hagakure;
                            // }
                        }
                    }
                    else
                    {
                        if (level >= SAM.Levels.MeikyoShisui && HasEffect(SAM.Buffs.MeikyoShisui))
                            return SAM.Yukikaze;

                        if (comboTime > 0)
                        {
                            if (lastComboMove == SAM.Hakaze && level >= SAM.Levels.Yukikaze)
                                return SAM.Yukikaze;
                        }

                        return SAM.Hakaze;
                    }

                    return SAM.Hakaze;
                }

                //if (level >= SAM.Levels.Hagakure && IsOffCooldown(SAM.Hagakure) && ShouldUseHakagure)
                //{
                //    return SAM.Hagakure;
                //}

                if (level >= SAM.Levels.HissatsuKaiten && gauge.Kenki >= 20 && !HasEffect(SAM.Buffs.Kaiten) &&
                    IsOffCooldown(SAM.HissatsuKaiten))
                {
                    // if (GetCooldown(SAM.TsubameGaeshi).RemainingCharges > 0 ||
                    //   GetCooldown(SAM.TsubameGaeshi).ChargeCooldownRemaining > 8)
                    // {
                    //   if (level >= SAM.Levels.Iaijutsu && gauge.HasGetsu && gauge.HasKa && gauge.HasSetsu)
                    //   {
                    //       return SAM.HissatsuKaiten;
                    //  }
                    //}

                    if (level >= SAM.Levels.Iaijutsu && gauge.HasGetsu && gauge.HasKa && gauge.HasSetsu)
                    {
                        return SAM.HissatsuKaiten;
                    }

                    if (level >= SAM.Levels.OgiNamikiri && HasEffect(SAM.Buffs.OgiNamikiriReady) && !this.isMoving)
                    {
                        return SAM.HissatsuKaiten;
                    }

                    if (!this.isMoving)
                    {
                        if (GetCooldown(SAM.TsubameGaeshi).RemainingCharges > 0 ||
                            GetCooldown(SAM.TsubameGaeshi).ChargeCooldownRemaining > 8)
                        {
                            if (level >= SAM.Levels.Iaijutsu && !TargetHasEffect(SAM.Debuffs.Higanbana) &&
                                gauge.HasSetsu)
                            {
                                return SAM.HissatsuKaiten;
                            }

                            if (level >= SAM.Levels.Iaijutsu && FindTargetEffect(SAM.Debuffs.Higanbana) != null &&
                                FindTargetEffect(SAM.Debuffs.Higanbana).RemainingTime <= 7 &&
                                gauge.HasSetsu && sens <= 1)
                            {
                                return SAM.HissatsuKaiten;
                            }
                        }
                    }
                }


                if (level >= SAM.Levels.Shoha && gauge.MeditationStacks >= 3 && IsOffCooldown(SAM.Shoha))
                    return SAM.Shoha;


                if (level >= SAM.Levels.MeikyoShisui && level < 88 && IsOffCooldown(SAM.MeikyoShisui) &&
                    !HasEffect(SAM.Buffs.MeikyoShisui))
                {
                    return SAM.MeikyoShisui;
                }

                if (level >= SAM.Levels.MeikyoShisui && level >= 88 &&
                    GetCooldown(SAM.MeikyoShisui).RemainingCharges > 0 && !HasEffect(SAM.Buffs.MeikyoShisui))
                {
                    return SAM.MeikyoShisui;
                }

                if (level >= SAM.Levels.Ikishoten && IsOffCooldown(SAM.Ikishoten))
                {
                    return SAM.Ikishoten;
                }


                if (sens < 3)
                {
                    if (level >= SAM.Levels.HissatsuSenei && IsOffCooldown(SAM.HissatsuSenei) && gauge.Kenki >= 25)
                        return SAM.HissatsuSenei;

                    if (level >= SAM.Levels.HissatsuShinten && IsOffCooldown(SAM.HissatsuShinten) && gauge.Kenki >= 25 &&
                        FindTargetEffect(SAM.Debuffs.Higanbana).RemainingTime >= 15)
                    {
                        return SAM.HissatsuShinten;
                    }
                }


                if (level >= SAM.Levels.TrueNorth && GetCooldown(SAM.TrueNorth).RemainingCharges > 0 &&
                    !HasEffect(SAM.Buffs.TrueNorth))
                {
                    return SAM.TrueNorth;
                }
            }

            if (actionID == SAM.Fuga)
            {
                var gauge = GetJobGauge<SAMGauge>();
                if (IsUnderGcd(SAM.GCD_SKILL))
                {
                    if (level >= SAM.Levels.TsubameGaeshi && gauge.Sen == Sen.NONE &&
                        IsOffCooldown(SAM.TsubameGaeshi) && HasCondition(ConditionFlag.InCombat))
                    {
                        if (CanUseThing)
                            return OriginalHook(SAM.TsubameGaeshi);
                    }

                    if (!gauge.HasGetsu)
                    {
                        if (comboTime > 0)
                        {
                            if (lastComboMove == SAM.Fuga && level >= SAM.Levels.Mangetsu)
                                return SAM.Mangetsu;
                        }

                        return SAM.Fuga;
                    }

                    if (!gauge.HasKa)
                    {
                        if (comboTime > 0)
                        {
                            if (lastComboMove == SAM.Fuga && level >= SAM.Levels.Oka)
                                return SAM.Oka;
                        }

                        return SAM.Fuga;
                    }

                    if (level >= SAM.Levels.Iaijutsu && gauge.HasGetsu && gauge.HasKa)
                    {
                        return SAM.TenkaGoken;
                    }
                }

                if (level >= SAM.Levels.Shoha && gauge.MeditationStacks >= 3 && IsOffCooldown(SAM.Shoha))
                    return SAM.Shoha;

                if (level >= SAM.Levels.MeikyoShisui && IsOffCooldown(SAM.MeikyoShisui))
                {
                    return SAM.MeikyoShisui;
                }

                if (level >= SAM.Levels.Ikishoten && IsOffCooldown(SAM.Ikishoten))
                {
                    return SAM.Ikishoten;
                }

                if (level >= SAM.Levels.HissatsuKaiten && gauge.HasGetsu && gauge.HasKa && gauge.HasSetsu &&
                    gauge.Kenki >= 20)
                {
                    return SAM.HissatsuKaiten;
                }

                var sens = 0;
                if (gauge.HasGetsu)
                {
                    sens = sens + 1;
                }

                if (gauge.HasKa)
                {
                    sens = sens + 1;
                }


                if (sens < 2)
                {
                    if (level >= SAM.Levels.HissatsuKyuten && IsOffCooldown(SAM.HissatsuKyuten) && gauge.Kenki >= 25)
                    {
                        return SAM.HissatsuKyuten;
                    }

                    if (level >= SAM.Levels.HissatsuShinten && IsOffCooldown(SAM.HissatsuShinten) && gauge.Kenki >= 25 &&
                        FindTargetEffect(SAM.Debuffs.Higanbana).RemainingTime >= 15)
                    {
                        return SAM.HissatsuShinten;
                    }
                }
            }

            return actionID;
        }
    }

    internal class SamuraiGekko : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamuraiGekkoCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.Gekko)
            {
                if (level >= SAM.Levels.MeikyoShisui && HasEffect(SAM.Buffs.MeikyoShisui))
                    return SAM.Gekko;

                if (comboTime > 0)
                {
                    if (lastComboMove == SAM.Jinpu && level >= SAM.Levels.Gekko)
                        return SAM.Gekko;

                    if (lastComboMove == SAM.Hakaze && level >= SAM.Levels.Jinpu)
                        return SAM.Jinpu;
                }

                if (IsEnabled(CustomComboPreset.SamuraiGekkoOption))
                    return SAM.Jinpu;

                return SAM.Hakaze;
            }

            return actionID;
        }
    }

    internal class SamuraiKasha : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamuraiKashaCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.Kasha)
            {
                if (level >= SAM.Levels.MeikyoShisui && HasEffect(SAM.Buffs.MeikyoShisui))
                    return SAM.Kasha;

                if (comboTime > 0)
                {
                    if (lastComboMove == SAM.Shifu && level >= SAM.Levels.Kasha)
                        return SAM.Kasha;

                    if (lastComboMove == SAM.Hakaze && level >= SAM.Levels.Shifu)
                        return SAM.Shifu;
                }

                if (IsEnabled(CustomComboPreset.SamuraiKashaOption))
                    return SAM.Shifu;

                return SAM.Hakaze;
            }

            return actionID;
        }
    }

    internal class SamuraiMangetsu : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamuraiMangetsuCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.Mangetsu)
            {
                if (level >= SAM.Levels.MeikyoShisui && HasEffect(SAM.Buffs.MeikyoShisui))
                    return SAM.Mangetsu;

                if (comboTime > 0)
                {
                    if ((lastComboMove == SAM.Fuga || lastComboMove == SAM.Fuko) && level >= SAM.Levels.Mangetsu)
                        return SAM.Mangetsu;
                }

                // Fuko
                return OriginalHook(SAM.Fuga);
            }

            return actionID;
        }
    }

    internal class SamuraiOka : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamuraiOkaCombo;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.Oka)
            {
                if (level >= SAM.Levels.MeikyoShisui && HasEffect(SAM.Buffs.MeikyoShisui))
                    return SAM.Oka;

                if (comboTime > 0)
                {
                    if ((lastComboMove == SAM.Fuga || lastComboMove == SAM.Fuko) && level >= SAM.Levels.Oka)
                        return SAM.Oka;
                }

                // Fuko
                return OriginalHook(SAM.Fuga);
            }

            return actionID;
        }
    }

    internal class SamuraiTsubame : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.TsubameGaeshi)
            {
                var gauge = GetJobGauge<SAMGauge>();

                if (IsEnabled(CustomComboPreset.SamuraiTsubameGaeshiShohaFeature))
                {
                    if (level >= SAM.Levels.Shoha && gauge.MeditationStacks >= 3)
                        return SAM.Shoha;
                }

                if (IsEnabled(CustomComboPreset.SamuraiTsubameGaeshiIaijutsuFeature))
                {
                    if (level >= SAM.Levels.TsubameGaeshi && gauge.Sen == Sen.NONE)
                        return OriginalHook(SAM.TsubameGaeshi);

                    return OriginalHook(SAM.Iaijutsu);
                }
            }

            return actionID;
        }
    }

    internal class SamuraiIaijutsu : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.Iaijutsu)
            {
                var gauge = GetJobGauge<SAMGauge>();

                if (IsEnabled(CustomComboPreset.SamuraiIaijutsuShohaFeature))
                {
                    if (level >= SAM.Levels.Shoha && gauge.MeditationStacks >= 3)
                        return SAM.Shoha;
                }

                if (IsEnabled(CustomComboPreset.SamuraiIaijutsuTsubameGaeshiFeature))
                {
                    if (level >= SAM.Levels.TsubameGaeshi && gauge.Sen == Sen.NONE)
                        return OriginalHook(SAM.TsubameGaeshi);

                    return OriginalHook(SAM.Iaijutsu);
                }
            }

            return actionID;
        }
    }

    internal class SamuraiShinten : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.HissatsuShinten)
            {
                var gauge = GetJobGauge<SAMGauge>();

                if (IsEnabled(CustomComboPreset.SamuraiShintenShohaFeature))
                {
                    if (level >= SAM.Levels.Shoha && gauge.MeditationStacks >= 3)
                        return SAM.Shoha;
                }

                if (IsEnabled(CustomComboPreset.SamuraiShintenSeneiFeature))
                {
                    if (level >= SAM.Levels.HissatsuSenei && IsOffCooldown(SAM.HissatsuSenei))
                        return SAM.HissatsuSenei;

                    if (IsEnabled(CustomComboPreset.SamuraiSeneiGurenFeature))
                    {
                        if (level >= SAM.Levels.HissatsuGuren && level < SAM.Levels.HissatsuSenei &&
                            IsOffCooldown(SAM.HissatsuGuren))
                            return SAM.HissatsuGuren;
                    }
                }
            }

            return actionID;
        }
    }

    internal class SamuraiSenei : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.HissatsuSenei)
            {
                if (IsEnabled(CustomComboPreset.SamuraiSeneiGurenFeature))
                {
                    if (level >= SAM.Levels.HissatsuGuren && level < SAM.Levels.HissatsuSenei)
                        return SAM.HissatsuGuren;
                }
            }

            return actionID;
        }
    }

    internal class SamuraiKyuten : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.SamAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.HissatsuKyuten)
            {
                var gauge = GetJobGauge<SAMGauge>();

                if (IsEnabled(CustomComboPreset.SamuraiKyutenShoha2Feature))
                {
                    if (level >= SAM.Levels.Shoha2 && gauge.MeditationStacks >= 3)
                        return SAM.Shoha2;
                }

                if (IsEnabled(CustomComboPreset.SamuraiKyutenGurenFeature))
                {
                    if (level >= SAM.Levels.HissatsuGuren && IsOffCooldown(SAM.HissatsuGuren))
                        return SAM.HissatsuGuren;
                }
            }

            return actionID;
        }
    }

    internal class SamuraiIkishoten : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.SamuraiIkishotenNamikiriFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == SAM.Ikishoten)
            {
                if (level >= SAM.Levels.OgiNamikiri)
                {
                    var gauge = GetJobGauge<SAMGauge>();

                    if (level >= SAM.Levels.Shoha && gauge.MeditationStacks >= 3)
                        return SAM.Shoha;

                    if (gauge.Kaeshi == Kaeshi.NAMIKIRI)
                        return SAM.KaeshiNamikiri;

                    if (HasEffect(SAM.Buffs.OgiNamikiriReady))
                        return SAM.OgiNamikiri;
                }
            }

            return actionID;
        }
    }
}