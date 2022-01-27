using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class BRD
    {
        public const byte ClassID = 5;
        public const byte JobID = 23;
        public const double GDC = 0.55;

        public const uint
            HeavyShot = 97,
            StraightShot = 98,
            VenomousBite = 100,
            QuickNock = 106,
            Bloodletter = 110,
            Windbite = 113,
            RainOfDeath = 117,
            EmpyrealArrow = 3558,
            WanderersMinuet = 3559,
            IronJaws = 3560,
            Sidewinder = 3562,
            PitchPerfect = 7404,
            CausticBite = 7406,
            Stormbite = 7407,
            RefulgentArrow = 7409,
            BurstShot = 16495,
            ApexArrow = 16496,
            Shadowbite = 16494,
            Ladonsbite = 25783,
            Barrage = 107,
            BattleVoice = 118,
            RagingStrikes = 101,
            Mageballad = 114,
            ArmyPeon = 116,
            WandererMinet = 3559,
            RadiantFinal = 25785,
            HeadGraze = 7551,
            Troubadour = 7405,
            NatureMinne = 7408,
            BlastArrow = 25784;

        public static class PvpSkills
        {
            public const ushort
                BurstShot = 17745,
                Shadowbite = 18931,
                Sidewinder = 8841,
                ApexArrow = 17747,
                PitchPerfect = 8842,
                WanderMinuet = 8843,
                ArmyPeon = 8844,
                RepellingShot = 8839,
                Potion = 18943,
                Nature = 19071,
                EmpyrealArrow = 8838;
        }

        public static class Buffs
        {
            public const ushort
                StraightShotReady = 122,
                BlastShotReady = 2692,
                Mageballad = 2217,
                ArmyPeon = 2218,
                WandererMinet = 2216,
                RadiantFinal = 2964,
                RagingStrike = 125,
                BattleVoice = 141,
                ShadowbiteReady = 3002;
        }

        public static class Debuffs
        {
            public const ushort
                VenomousBite = 124,
                Windbite = 129,
                CausticBite = 1200,
                Stormbite = 1201;
        }

        public static class Levels
        {
            public const byte
                NatureMinne = 66,
                Troubadour = 62,
                HeadGraze = 24,
                StraightShot = 2,
                VenomousBite = 6,
                Bloodletter = 12,
                Windbite = 30,
                RainOfDeath = 45,
                PitchPerfect = 52,
                EmpyrealArrow = 54,
                IronJaws = 56,
                Sidewinder = 60,
                BiteUpgrade = 64,
                RefulgentArrow = 70,
                Shadowbite = 72,
                BurstShot = 76,
                ApexArrow = 80,
                Ladonsbite = 82,
                Barrage = 38,
                BattleVoice = 50,
                RagingStrikes = 4,
                Mageballad = 30,
                ArmyPeon = 40,
                WandererMinet = 52,
                RadiantFinal = 90,
                CausticBite = 64,
                Stormbite = 64,
                QuickNock = 18,
                BlastShot = 86;
        }
    }

    internal class BardWanderersPitchPerfectFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.BardWanderersPitchPerfectFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.WanderersMinuet)
            {
                var gauge = GetJobGauge<BRDGauge>();

                if (level >= BRD.Levels.PitchPerfect && gauge.Song == Song.WANDERER)
                    return BRD.PitchPerfect;
            }

            return actionID;
        }
    }

    internal class BardStraightShotUpgradeFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BrdAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.HeavyShot || actionID == BRD.BurstShot)
            {
                var gauge2 = GetJobGauge<BRDGauge>();

                int codaCount = 0;
                foreach (Song song in gauge2.Coda)
                {
                    if (song == Song.NONE)
                    {
                        codaCount = codaCount + 1;
                    }
                }


                if (!IsOnCooldown(BRD.StraightShot) && level >= BRD.Levels.StraightShot &&
                    HasEffect(BRD.Buffs.StraightShotReady))
                    // Refulgent Arrow
                    return OriginalHook(BRD.StraightShot);


                var gauge = GetJobGauge<BRDGauge>();


                if (!IsOnCooldown(BRD.ApexArrow) && gauge.SoulVoice >= 80 &&
                    gauge.Song == Song.MAGE && gauge.SongTimer >= 21000)
                    return BRD.ApexArrow;

                if (!IsOnCooldown(BRD.ApexArrow) && gauge.SoulVoice >= 80 &&
                    HasEffect(BRD.Buffs.RagingStrike) &&
                    HasEffect(BRD.Buffs.BattleVoice))
                    return BRD.ApexArrow;

                if (!IsOnCooldown(BRD.BlastArrow) && level >= BRD.Levels.BlastShot &&
                    HasEffect(BRD.Buffs.BlastShotReady))
                    return BRD.BlastArrow;


                if (GetCooldown(BRD.HeavyShot).CooldownRemaining < BRD.GDC)
                {
                    var windbite = FindTargetEffect(BRD.Debuffs.Windbite);
                    var stormbite = FindTargetEffect(BRD.Debuffs.Stormbite);

                    var causticBite = FindTargetEffect(BRD.Debuffs.CausticBite);
                    var VenomousBite = FindTargetEffect(BRD.Debuffs.VenomousBite);

                    if (IsOffCooldown(BRD.IronJaws) && level >= BRD.Levels.IronJaws &&
                        GetCooldown(BRD.BurstShot).CooldownRemaining <= BRD.GDC)
                    {
                        if (TargetHasEffect(BRD.Debuffs.VenomousBite) && VenomousBite.RemainingTime <= 7)
                        {
                            return BRD.IronJaws;
                        }

                        if (TargetHasEffect(BRD.Debuffs.CausticBite) && causticBite.RemainingTime <= 7)
                        {
                            return BRD.IronJaws;
                        }

                        if (TargetHasEffect(BRD.Debuffs.Windbite) && windbite.RemainingTime <= 7)
                        {
                            return BRD.IronJaws;
                        }

                        if (TargetHasEffect(BRD.Debuffs.Stormbite) && stormbite.RemainingTime <= 7)
                        {
                            return BRD.IronJaws;
                        }
                    }


                    if (level >= BRD.Levels.Stormbite && !TargetHasEffect(BRD.Debuffs.Stormbite))
                    {
                        return BRD.Stormbite;
                    }


                    if (level < BRD.Levels.Stormbite && level >= BRD.Levels.Windbite &&
                        !TargetHasEffect(BRD.Debuffs.Windbite))
                    {
                        return BRD.Windbite;
                    }


                    if (level >= BRD.Levels.Stormbite && stormbite.RemainingTime <= 7)
                    {
                        return BRD.Stormbite;
                    }


                    if (level < BRD.Levels.Stormbite && level >= BRD.Levels.Windbite &&
                        windbite.RemainingTime <= 7)
                    {
                        return BRD.Windbite;
                    }


                    if (level >= BRD.Levels.CausticBite && !TargetHasEffect(BRD.Debuffs.CausticBite))
                    {
                        return BRD.CausticBite;
                    }


                    if (level < BRD.Levels.CausticBite && level >= BRD.Levels.VenomousBite &&
                        !TargetHasEffect(BRD.Debuffs.VenomousBite))
                    {
                        return BRD.VenomousBite;
                    }


                    if (level >= BRD.Levels.CausticBite && causticBite.RemainingTime <= 7)
                    {
                        return BRD.CausticBite;
                    }


                    if (level < BRD.Levels.CausticBite && level >= BRD.Levels.VenomousBite &&
                        VenomousBite.RemainingTime <= 7)
                    {
                        return BRD.VenomousBite;
                    }
                }


                if (GetCooldown(BRD.HeavyShot).CooldownRemaining >= BRD.GDC)
                {
                    if (level >= ALL.Levels.HeadGraze && CanInterruptEnemy() && !GetCooldown(ALL.HeadGraze).IsCooldown)
                    {
                        return ALL.HeadGraze;
                    }

                    if (!IsOnCooldown(BRD.WandererMinet) && level >= BRD.Levels.WandererMinet &&
                        gauge.Song == Song.NONE && gauge.SongTimer <= 2000)
                    {
                        return BRD.WandererMinet;
                    }

                    if (!IsOnCooldown(BRD.Mageballad) && level >= BRD.Levels.Mageballad && gauge.Song == Song.NONE &&
                        gauge.SongTimer <= 12000)
                    {
                        return BRD.Mageballad;
                    }

                    if (level >= BRD.Levels.ArmyPeon && !IsOnCooldown(BRD.ArmyPeon) && gauge.Song == Song.MAGE &&
                        gauge.SongTimer <= 11000)
                    {
                        return BRD.ArmyPeon;
                    }

                    if (!IsOnCooldown(BRD.ArmyPeon) && level >= BRD.Levels.ArmyPeon && gauge.Song == Song.NONE)
                    {
                        return BRD.ArmyPeon;
                    }

                    if (!IsOnCooldown(BRD.RagingStrikes) && level >= BRD.Levels.RagingStrikes)
                        return BRD.RagingStrikes;

                    if (codaCount <= 2 && !IsOnCooldown(BRD.RadiantFinal) && level >= BRD.Levels.RadiantFinal &&
                        HasEffect(BRD.Buffs.RagingStrike))
                    {
                        return BRD.RadiantFinal;
                    }

                    if (!IsOnCooldown(BRD.BattleVoice) && level >= BRD.Levels.BattleVoice)
                    {
                        if (gauge.Song != Song.NONE)
                        {
                            return BRD.BattleVoice;
                        }
                    }

                    if (level >= BRD.Levels.WandererMinet && !IsOnCooldown(BRD.WandererMinet) &&
                        gauge.Song == Song.ARMY)
                    {
                        return BRD.WandererMinet;
                    }

                    if (!IsOnCooldown(BRD.PitchPerfect) && level >= BRD.Levels.PitchPerfect &&
                        gauge.Song == Song.WANDERER && gauge.Repertoire >= 3)
                    {
                        return BRD.PitchPerfect;
                    }

                    if (!IsOnCooldown(BRD.PitchPerfect) && level >= BRD.Levels.PitchPerfect &&
                        gauge.Song == Song.WANDERER && gauge.Repertoire >= 2 && gauge.SongTimer <= 9000)
                    {
                        return BRD.PitchPerfect;
                    }

                    if (!IsOnCooldown(BRD.PitchPerfect) && level >= BRD.Levels.PitchPerfect &&
                        gauge.Song == Song.WANDERER && gauge.Repertoire >= 1 && gauge.SongTimer <= 6000)
                    {
                        return BRD.PitchPerfect;
                    }

                    if (!IsOnCooldown(BRD.EmpyrealArrow) && level >= BRD.Levels.EmpyrealArrow)
                    {
                        return BRD.EmpyrealArrow;
                    }


                    if (!IsOnCooldown(BRD.Barrage) && level >= BRD.Levels.Barrage &&
                        !HasEffect(BRD.Buffs.StraightShotReady))
                    {
                        return BRD.Barrage;
                    }


                    if (!IsOnCooldown(BRD.Sidewinder) && level >= BRD.Levels.Sidewinder)
                    {
                        return BRD.Sidewinder;
                    }

                    if ((LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp <= 40 &&
                        IsOffCooldown(ALL.SecondWind) && level >= ALL.Levels.SecondWind)
                    {
                        return ALL.SecondWind;
                    }


                    if (level >= BRD.Levels.Bloodletter &&
                        GetCooldown(BRD.Bloodletter).RemainingCharges > 0)
                    {
                        if (HasEffect(BRD.Buffs.RadiantFinal) || HasEffect(BRD.Buffs.RagingStrike))
                        {
                            return BRD.Bloodletter;
                        }

                        if (gauge.Song == Song.ARMY && GetCooldown(BRD.Bloodletter).RemainingCharges >= 2)
                        {
                            return BRD.Bloodletter;
                        }

                        if (gauge.Song == Song.MAGE)
                        {
                            return BRD.Bloodletter;
                        }

                        if (gauge.Song == Song.WANDERER)
                        {
                            return BRD.Bloodletter;
                        }

                        if (gauge.Song == Song.NONE)
                        {
                            return BRD.Bloodletter;
                        }
                    }


                    if (level >= BRD.Levels.Troubadour)
                    {
                        if (IsOffCooldown(BRD.Troubadour))
                        {
                            return BRD.Troubadour;
                        }
                    }

                    if (level >= BRD.Levels.NatureMinne)
                    {
                        if (IsOffCooldown(BRD.NatureMinne) && GetPlayerHealth() <= 50)
                        {
                            return BRD.NatureMinne;
                        }
                    }


                    if (level >= ALL.Levels.LegGraze && !GetCooldown(ALL.LegGraze).IsCooldown)
                    {
                        return ALL.LegGraze;
                    }

                    if (level >= ALL.Levels.FootGraze && !GetCooldown(ALL.FootGraze).IsCooldown)
                    {
                        return ALL.FootGraze;
                    }
                }


                if (level >= BRD.Levels.BurstShot)
                {
                    if (GetCooldown(BRD.BurstShot).CooldownRemaining <= BRD.GDC)
                    {
                        return BRD.BurstShot;
                    }
                }

                if (level < BRD.Levels.BurstShot)
                {
                    if (GetCooldown(BRD.HeavyShot).CooldownRemaining <= BRD.GDC)
                    {
                        return BRD.HeavyShot;
                    }
                }
            }

            if (actionID == BRD.PvpSkills.BurstShot)
            {
                var gauge = GetJobGauge<BRDGauge>();

                if ((LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp <= 40 &&
                    GetCooldown(BRD.PvpSkills.Potion).RemainingCharges > 0)
                {
                    return BRD.PvpSkills.Potion;
                }

                if (GetCooldown(BRD.BurstShot).CooldownRemaining >= BRD.GDC)
                {
                    if (gauge.SongTimer <= 1)
                    {
                        if (!IsOnCooldown(BRD.PvpSkills.WanderMinuet))
                            return BRD.PvpSkills.WanderMinuet;
                        if (!IsOnCooldown(BRD.PvpSkills.ArmyPeon))
                            return BRD.PvpSkills.ArmyPeon;
                    }

                    if (!IsOnCooldown(BRD.PvpSkills.Nature) && (LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp < 80)
                        return BRD.PvpSkills.Nature;

                    if (!IsOnCooldown(BRD.PvpSkills.Shadowbite))
                        return BRD.PvpSkills.Shadowbite;
                    if (!IsOnCooldown(BRD.PvpSkills.Sidewinder))
                        return BRD.PvpSkills.Sidewinder;
                    if (GetCooldown(BRD.PvpSkills.EmpyrealArrow).RemainingCharges > 0)
                        return BRD.PvpSkills.EmpyrealArrow;

                    if (!IsOnCooldown(BRD.PvpSkills.PitchPerfect) && gauge.SongTimer >= 1 && gauge.Repertoire > 1 &&
                        gauge.Song == Song.WANDERER)
                        return BRD.PvpSkills.PitchPerfect;

                    if ((System.Numerics.Vector3.Distance(CurrentTarget.Position, LocalPlayer.Position) -
                         CurrentTarget.HitboxRadius) <= 6.0 && IsOffCooldown(BRD.PvpSkills.RepellingShot))
                    {
                        return BRD.PvpSkills.RepellingShot;
                    }
                }

                if (gauge.SoulVoice >= 50 && !IsOnCooldown(BRD.PvpSkills.ApexArrow))
                    return BRD.PvpSkills.ApexArrow;
                return BRD.PvpSkills.BurstShot;
            }

            return actionID;
        }
    }

    internal class BardIronJawsFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BardIronJawsFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.IronJaws)
            {
                if (level < BRD.Levels.Windbite)
                    return BRD.VenomousBite;

                if (level < BRD.Levels.IronJaws)
                {
                    var venomous = FindTargetEffect(BRD.Debuffs.VenomousBite);
                    var windbite = FindTargetEffect(BRD.Debuffs.Windbite);

                    if (venomous is null)
                        return BRD.VenomousBite;

                    if (windbite is null)
                        return BRD.Windbite;

                    if (venomous?.RemainingTime < windbite?.RemainingTime)
                        return BRD.VenomousBite;

                    return BRD.Windbite;
                }

                if (level < BRD.Levels.BiteUpgrade)
                {
                    var venomous = TargetHasEffect(BRD.Debuffs.VenomousBite);
                    var windbite = TargetHasEffect(BRD.Debuffs.Windbite);

                    if (venomous && windbite)
                        return BRD.IronJaws;

                    if (windbite)
                        return BRD.VenomousBite;

                    return BRD.Windbite;
                }

                var caustic = TargetHasEffect(BRD.Debuffs.CausticBite);
                var stormbite = TargetHasEffect(BRD.Debuffs.Stormbite);

                if (caustic && stormbite)
                    return BRD.IronJaws;

                if (stormbite)
                    return BRD.CausticBite;

                return BRD.Stormbite;
            }

            return actionID;
        }
    }

    internal class BardShadowbiteFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BrdAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.QuickNock || actionID == BRD.Ladonsbite)
            {
                if (IsEnabled(CustomComboPreset.BardApexFeature))
                {
                    var gauge = GetJobGauge<BRDGauge>();

                    if (level >= BRD.Levels.ApexArrow && gauge.SoulVoice == 80)
                        return BRD.ApexArrow;

                    if (level >= BRD.Levels.BlastShot && HasEffect(BRD.Buffs.BlastShotReady))
                        return BRD.BlastArrow;
                }

                var gauge2 = GetJobGauge<BRDGauge>();

                int codaCount = 0;
                foreach (Song song in gauge2.Coda)
                {
                    if (song == Song.NONE)
                    {
                        codaCount = codaCount + 1;
                    }
                }

                if (codaCount <= 1 && !IsOnCooldown(BRD.RadiantFinal) && level >= BRD.Levels.RadiantFinal)
                {
                    return BRD.RadiantFinal;
                }

                if (!IsOnCooldown(BRD.BattleVoice) && level >= BRD.Levels.BattleVoice)
                {
                    if (HasEffect(BRD.Buffs.ArmyPeon) || HasEffect(BRD.Buffs.Mageballad) ||
                        HasEffect(BRD.Buffs.WandererMinet))
                    {
                        return BRD.BattleVoice;
                    }
                }

                if (!IsOnCooldown(BRD.RagingStrikes) && level >= BRD.Levels.RagingStrikes)
                    return BRD.RagingStrikes;


                if (IsEnabled(CustomComboPreset.BardShadowbiteFeature))
                {
                    if (level >= BRD.Levels.Shadowbite && HasEffect(BRD.Buffs.ShadowbiteReady) &&
                        GetCooldown(BRD.Shadowbite).CooldownRemaining <= BRD.GDC)
                        return BRD.Shadowbite;
                }

                if (level >= BRD.Levels.Ladonsbite)
                {
                    if (GetCooldown(BRD.Ladonsbite).CooldownRemaining <= BRD.GDC)
                    {
                        return BRD.Ladonsbite;
                    }
                }

                if (level < BRD.Levels.QuickNock)
                {
                    if (GetCooldown(BRD.QuickNock).CooldownRemaining <= BRD.GDC)
                    {
                        return BRD.QuickNock;
                    }
                }


                if (level >= BRD.Levels.RainOfDeath &&
                    GetCooldown(BRD.RainOfDeath).RemainingCharges > 0)
                {
                    return BRD.RainOfDeath;
                }

                gauge2 = GetJobGauge<BRDGauge>();
                if (!IsOnCooldown(BRD.WandererMinet) && level >= BRD.Levels.WandererMinet && gauge2.Song == Song.NONE &&
                    GetCooldown(BRD.BurstShot).CooldownRemaining >= BRD.GDC && gauge2.SongTimer <= 2000)
                {
                    return BRD.WandererMinet;
                }

                if (!IsOnCooldown(BRD.Mageballad) && level >= BRD.Levels.Mageballad && gauge2.Song == Song.NONE &&
                    GetCooldown(BRD.BurstShot).CooldownRemaining >= BRD.GDC && gauge2.SongTimer <= 12000)
                {
                    return BRD.Mageballad;
                }

                if (level >= BRD.Levels.ArmyPeon && !IsOnCooldown(BRD.ArmyPeon) && gauge2.Song == Song.MAGE &&
                    gauge2.SongTimer <= 11000 &&
                    GetCooldown(BRD.BurstShot).CooldownRemaining >= BRD.GDC)
                {
                    return BRD.ArmyPeon;
                }

                gauge2 = GetJobGauge<BRDGauge>();
                if (!IsOnCooldown(BRD.ArmyPeon) && level >= BRD.Levels.ArmyPeon && gauge2.Song == Song.NONE &&
                    GetCooldown(BRD.BurstShot).CooldownRemaining >= BRD.GDC)
                {
                    return BRD.ArmyPeon;
                }

                if (level >= BRD.Levels.WandererMinet && !IsOnCooldown(BRD.WandererMinet) && gauge2.Song == Song.ARMY &&
                    GetCooldown(BRD.BurstShot).CooldownRemaining >= BRD.GDC)
                {
                    return BRD.WandererMinet;
                }

                if (!IsOnCooldown(BRD.PitchPerfect) && level >= BRD.Levels.PitchPerfect &&
                    gauge2.Song == Song.WANDERER && gauge2.Repertoire >= 3 &&
                    GetCooldown(BRD.BurstShot).CooldownRemaining >= BRD.GDC)
                {
                    return BRD.PitchPerfect;
                }

                if (!IsOnCooldown(BRD.PitchPerfect) && level >= BRD.Levels.PitchPerfect &&
                    gauge2.Song == Song.WANDERER && gauge2.Repertoire >= 2 &&
                    GetCooldown(BRD.BurstShot).CooldownRemaining >= BRD.GDC && gauge2.SongTimer <= 6000)
                {
                    return BRD.PitchPerfect;
                }

                if (!IsOnCooldown(BRD.EmpyrealArrow) && level >= BRD.Levels.EmpyrealArrow)
                {
                    return BRD.EmpyrealArrow;
                }


                if (!IsOnCooldown(BRD.Sidewinder) && level >= BRD.Levels.Sidewinder)
                {
                    return BRD.Sidewinder;
                }
            }

            return actionID;
        }
    }

    internal class BardBloodletterFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.Disabled; // BardBloodletterFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.Bloodletter)
            {
                if (level >= BRD.Levels.Sidewinder)
                    return CalcBestAction(actionID, BRD.Bloodletter, BRD.EmpyrealArrow, BRD.Sidewinder);

                if (level >= BRD.Levels.EmpyrealArrow)
                    return CalcBestAction(actionID, BRD.Bloodletter, BRD.EmpyrealArrow);

                if (level >= BRD.Levels.Bloodletter)
                    return BRD.Bloodletter;
            }

            return actionID;
        }
    }

    internal class BardRainOfDeathFeature : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } =
            CustomComboPreset.Disabled; // BardRainOfDeathFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.RainOfDeath)
            {
                if (level >= BRD.Levels.Sidewinder)
                    return CalcBestAction(actionID, BRD.RainOfDeath, BRD.EmpyrealArrow, BRD.Sidewinder);

                if (level >= BRD.Levels.EmpyrealArrow)
                    return CalcBestAction(actionID, BRD.RainOfDeath, BRD.EmpyrealArrow);

                if (level >= BRD.Levels.RainOfDeath)
                    return BRD.RainOfDeath;
            }

            return actionID;
        }
    }
}