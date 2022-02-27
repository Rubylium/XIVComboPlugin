using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.JobGauge.Types;
using Dalamud.Logging;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class BRD
    {
        public const byte ClassID = 5;
        public const uint GCD_SKILL = HeavyShot;
        public const byte JobID = 23;
        public const double GDC = 0.55;
        public const double DOT_TIME_LEFT = 5;
        public const int DOT_TIME_LEFT_TO_ADD = 15;

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
                HeadGaze = 17680,
                Concentrate = 18955,
                Recuperate = 18928,
                EmpyrealArrow = 8838;
        }

        public static class BlacklistedBuffMobs
        {
            public const string
                p3normalAds = "Sunbird";
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
                CantSilence = 1353,
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
        List<string> blackListedBuffMobs = new List<string>() {"Sunbird", "SunBird", "sunbird"};
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BrdAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            unsafe
            {
                if (actionID == BRD.HeavyShot || actionID == BRD.BurstShot && HasTarget())
                {
                    var gauge2 = GetJobGauge<BRDGauge>();
                    var gauge = GetJobGauge<BRDGauge>();
                    var codaCount = 0;
                    var sec_to_add = 0;
                    var canUseBuffWindow = true;

                    foreach (string mobName in blackListedBuffMobs)
                    {
                        if (GetTargetName() == mobName)
                        {
                            canUseBuffWindow = false;
                        }
                    }

                    foreach (Song song in gauge2.Coda)
                    {
                        if (song == Song.NONE)
                        {
                            codaCount = codaCount + 1;
                        }
                    }

                    if (gauge.Song == Song.WANDERER)
                    {
                        sec_to_add = BRD.DOT_TIME_LEFT_TO_ADD;
                    }


                    if (IsUnderGcd(BRD.GCD_SKILL) && HasTarget())
                    {
                        if (level >= BRD.Levels.StraightShot &&
                            HasEffect(BRD.Buffs.StraightShotReady))
                        {
                            // Refulgent Arrow 
                            //XIVComboExpandedPlugin.ActionManager.UseAction(ActionType.Spell, OriginalHook(BRD.StraightShot), CurrentTarget.ObjectId);
                            return OriginalHook(BRD.StraightShot);
                        }


                        if (gauge.SoulVoice >= 100 &&
                            gauge.Song == Song.MAGE && gauge.SongTimer >= 21000)
                        {
                            //XIVComboExpandedPlugin.ActionManager.UseAction(ActionType.Spell,OriginalHook(BRD.ApexArrow), CurrentTarget.ObjectId);
                            return BRD.ApexArrow;
                        }


                        if (gauge.SoulVoice >= 80 &&
                            HasEffect(BRD.Buffs.RagingStrike) &&
                            HasEffect(BRD.Buffs.BattleVoice))
                        {
                            //XIVComboExpandedPlugin.ActionManager.UseAction(ActionType.Spell,OriginalHook(BRD.ApexArrow), CurrentTarget.ObjectId);
                            return BRD.ApexArrow;
                        }


                        if (level >= BRD.Levels.BlastShot &&
                            HasEffect(BRD.Buffs.BlastShotReady))
                        {
                            //XIVComboExpandedPlugin.ActionManager.UseAction(ActionType.Spell,OriginalHook(BRD.ApexArrow), CurrentTarget.ObjectId);
                            return BRD.BlastArrow;
                        }


                        if (!HasEffect(BRD.Buffs.StraightShotReady))
                        {
                            var windbite = FindTargetEffect(BRD.Debuffs.Windbite);
                            var stormbite = FindTargetEffect(BRD.Debuffs.Stormbite);

                            var causticBite = FindTargetEffect(BRD.Debuffs.CausticBite);
                            var VenomousBite = FindTargetEffect(BRD.Debuffs.VenomousBite);

                            if (level >= BRD.Levels.IronJaws)
                            {
                                if (TargetHasEffect(BRD.Debuffs.VenomousBite) && VenomousBite != null &&
                                    VenomousBite.RemainingTime <= BRD.DOT_TIME_LEFT + sec_to_add)
                                {
                                    return BRD.IronJaws;
                                }

                                if (TargetHasEffect(BRD.Debuffs.CausticBite) && causticBite != null &&
                                    causticBite.RemainingTime <= BRD.DOT_TIME_LEFT + sec_to_add)
                                {
                                    return BRD.IronJaws;
                                }

                                if (TargetHasEffect(BRD.Debuffs.Windbite) && windbite != null &&
                                    windbite.RemainingTime <= BRD.DOT_TIME_LEFT + sec_to_add)
                                {
                                    return BRD.IronJaws;
                                }

                                if (TargetHasEffect(BRD.Debuffs.Stormbite) && stormbite != null &&
                                    stormbite.RemainingTime <= BRD.DOT_TIME_LEFT + sec_to_add)
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


                            if (level >= BRD.Levels.Stormbite && stormbite != null &&
                                stormbite.RemainingTime <= BRD.DOT_TIME_LEFT + sec_to_add)
                            {
                                if (level >= BRD.Levels.IronJaws)
                                    return BRD.IronJaws;
                                return BRD.Stormbite;
                            }


                            if (level < BRD.Levels.Stormbite && level >= BRD.Levels.Windbite && windbite != null &&
                                windbite.RemainingTime <= BRD.DOT_TIME_LEFT + sec_to_add)
                            {
                                if (level >= BRD.Levels.IronJaws)
                                    return BRD.IronJaws;
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


                            if (level >= BRD.Levels.CausticBite && causticBite != null &&
                                causticBite.RemainingTime <= BRD.DOT_TIME_LEFT + sec_to_add)
                            {
                                if (level >= BRD.Levels.IronJaws)
                                    return BRD.IronJaws;
                                return BRD.CausticBite;
                            }


                            if (level < BRD.Levels.CausticBite && level >= BRD.Levels.VenomousBite &&
                                VenomousBite != null &&
                                VenomousBite.RemainingTime <= BRD.DOT_TIME_LEFT + sec_to_add)
                            {
                                if (level >= BRD.Levels.IronJaws)
                                    return BRD.IronJaws;
                                return BRD.VenomousBite;
                            }
                        }


                        //XIVComboExpandedPlugin.ActionManager.UseAction(ActionType.Spell, OriginalHook(BRD.BurstShot, CurrentTarget.ObjectId), ;
                        //XIVComboExpandedPlugin.ActionManager.UseAction(ActionType.Spell, OriginalHook(BRD.StraightShot), CurrentTarget.ObjectId);

                        return OriginalHook(BRD.BurstShot);
                    }


                    if (!IsUnderGcd(BRD.GCD_SKILL))
                    {
                        if (level >= ALL.Levels.HeadGraze && CanInterruptEnemy() &&
                            !GetCooldown(ALL.HeadGraze).IsCooldown)
                        {
                            return ALL.HeadGraze;
                        }

                        if (canUseBuffWindow)
                        {
                            if (!IsOnCooldown(BRD.WandererMinet) && level >= BRD.Levels.WandererMinet &&
                                gauge.Song == Song.NONE && gauge.SongTimer <= 2000)
                            {
                                return BRD.WandererMinet;
                            }


                            if (!IsOnCooldown(BRD.Mageballad) && level >= BRD.Levels.Mageballad &&
                                gauge.Song == Song.WANDERER &&
                                gauge.SongTimer <= 2000)
                            {
                                return BRD.Mageballad;
                            }

                            if (level < BRD.Levels.WandererMinet)
                            {
                                if (!IsOnCooldown(BRD.Mageballad) && level >= BRD.Levels.Mageballad &&
                                    gauge.Song == Song.NONE &&
                                    gauge.SongTimer <= 2000)
                                {
                                    return BRD.Mageballad;
                                }
                            }

                            if (level >= BRD.Levels.ArmyPeon && !IsOnCooldown(BRD.ArmyPeon) &&
                                gauge.Song == Song.MAGE &&
                                gauge.SongTimer <= 11000)
                            {
                                return BRD.ArmyPeon;
                            }

                            if (!IsOnCooldown(BRD.ArmyPeon) && level >= BRD.Levels.ArmyPeon && gauge.Song == Song.NONE)
                            {
                                return BRD.ArmyPeon;
                            }
                        }


                        if (canUseBuffWindow)
                        {
                            if (!IsOnCooldown(BRD.RagingStrikes) && level >= BRD.Levels.RagingStrikes &&
                                GetCooldown(BRD.RadiantFinal).CooldownRemaining <= 4 &&
                                GetCooldown(BRD.BattleVoice).CooldownRemaining <= 4)
                                return BRD.RagingStrikes;


                            if (level < BRD.Levels.RadiantFinal && level >= BRD.Levels.Barrage)
                            {
                                if (!IsOnCooldown(BRD.RagingStrikes) && level >= BRD.Levels.RagingStrikes &&
                                    GetCooldown(BRD.BattleVoice).CooldownRemaining <= 4)
                                    return BRD.RagingStrikes;

                                if (level < BRD.Levels.BattleVoice)
                                {
                                    if (!IsOnCooldown(BRD.RagingStrikes) && level >= BRD.Levels.RagingStrikes)
                                        return BRD.RagingStrikes;
                                }
                            }

                            if (codaCount <= 2 && !IsOnCooldown(BRD.RadiantFinal) && level >= BRD.Levels.RadiantFinal &&
                                HasEffect(BRD.Buffs.RagingStrike) && FindEffect(BRD.Buffs.RagingStrike) != null &&
                                FindEffect(BRD.Buffs.RagingStrike).RemainingTime <= 17)
                            {
                                return BRD.RadiantFinal;
                            }


                            if (!IsOnCooldown(BRD.BattleVoice) && level >= BRD.Levels.BattleVoice &&
                                FindEffect(BRD.Buffs.RagingStrike) != null &&
                                FindEffect(BRD.Buffs.RagingStrike).RemainingTime <= 17)
                            {
                                if (gauge.Song != Song.NONE)
                                {
                                    return BRD.BattleVoice;
                                }
                            }
                        }


                        if (canUseBuffWindow)
                        {
                            if (level >= BRD.Levels.WandererMinet && !IsOnCooldown(BRD.WandererMinet) &&
                                gauge.Song == Song.ARMY)
                            {
                                return BRD.WandererMinet;
                            }
                        }


                        if (!IsOnCooldown(BRD.EmpyrealArrow) && level >= BRD.Levels.EmpyrealArrow)
                        {
                            return BRD.EmpyrealArrow;
                        }

                        if (!IsOnCooldown(BRD.PitchPerfect) && level >= BRD.Levels.PitchPerfect &&
                            gauge.Song == Song.WANDERER && gauge.Repertoire >= 3)
                        {
                            return BRD.PitchPerfect;
                        }

                        if (!IsOnCooldown(BRD.PitchPerfect) && level >= BRD.Levels.PitchPerfect &&
                            gauge.Song == Song.WANDERER && gauge.Repertoire >= 2 && HasEffect(BRD.Buffs.BattleVoice) &&
                            FindEffect(BRD.Buffs.BattleVoice) != null &&
                            FindEffect(BRD.Buffs.BattleVoice).RemainingTime <= 6)
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

                        if (level >= BRD.Levels.Bloodletter &&
                            GetCooldown(BRD.Bloodletter).RemainingCharges == 3)
                        {
                            if (HasEffect(BRD.Buffs.RadiantFinal) || HasEffect(BRD.Buffs.RagingStrike))
                            {
                                return BRD.Bloodletter;
                            }

                            if (gauge.Song == Song.WANDERER)
                            {
                                if (HasEffect(BRD.Buffs.RagingStrike))
                                {
                                    return BRD.Bloodletter;
                                }

                                if (GetCooldown(BRD.RagingStrikes).CooldownRemaining > 90)
                                {
                                    return BRD.Bloodletter;
                                }
                            }
                        }


                        if (!IsOnCooldown(BRD.Barrage) && level >= BRD.Levels.Barrage &&
                            !HasEffect(BRD.Buffs.StraightShotReady) && HasEffect(BRD.Buffs.RadiantFinal) &&
                            HasEffect(BRD.Buffs.RagingStrike) && HasEffect(BRD.Buffs.BattleVoice))
                        {
                            return BRD.Barrage;
                        }

                        if (level < BRD.Levels.RadiantFinal && level >= BRD.Levels.Barrage)
                        {
                            if (!IsOnCooldown(BRD.Barrage) && level >= BRD.Levels.Barrage &&
                                !HasEffect(BRD.Buffs.StraightShotReady) && HasEffect(BRD.Buffs.RagingStrike) &&
                                HasEffect(BRD.Buffs.BattleVoice))
                            {
                                return BRD.Barrage;
                            }

                            if (level < BRD.Levels.BattleVoice)
                            {
                                if (!IsOnCooldown(BRD.Barrage) && level >= BRD.Levels.Barrage &&
                                    !HasEffect(BRD.Buffs.StraightShotReady) && HasEffect(BRD.Buffs.RagingStrike))
                                {
                                    return BRD.Barrage;
                                }
                            }
                        }


                        //if (canUseBuffWindow)
                        //{
                        if (!IsOnCooldown(BRD.Sidewinder) && level >= BRD.Levels.Sidewinder)
                        {
                            if (GetCooldown(BRD.RagingStrikes).CooldownRemaining > 15 &&
                                !HasEffect(BRD.Buffs.RagingStrike) &&
                                GetCooldown(BRD.RagingStrikes).CooldownRemaining < 115)
                                return BRD.Sidewinder;

                            if (HasEffect(BRD.Buffs.RadiantFinal) && HasEffect(BRD.Buffs.RagingStrike) &&
                                HasEffect(BRD.Buffs.BattleVoice))
                                return BRD.Sidewinder;

                            if (level < BRD.Levels.RadiantFinal && level >= BRD.Levels.Sidewinder)
                            {
                                if (HasEffect(BRD.Buffs.RagingStrike) &&
                                    HasEffect(BRD.Buffs.BattleVoice))
                                {
                                    return BRD.Sidewinder;
                                }

                                if (level < BRD.Levels.BattleVoice)
                                {
                                    if (HasEffect(BRD.Buffs.RagingStrike))
                                    {
                                        return BRD.Sidewinder;
                                    }
                                }
                            }
                        }
                        //}


                        if (LocalPlayer != null)
                        {
                            if ((LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp <= 40 &&
                                IsOffCooldown(ALL.SecondWind) && level >= ALL.Levels.SecondWind)
                            {
                                return ALL.SecondWind;
                            }
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
                                if (GetCooldown(BRD.RagingStrikes).CooldownRemaining > 15 &&
                                    GetCooldown(BRD.Bloodletter).RemainingCharges >= 2)
                                {
                                    return BRD.Bloodletter;
                                }
                            }

                            if (gauge.Song == Song.MAGE)
                            {
                                return BRD.Bloodletter;
                            }

                            if (gauge.Song == Song.WANDERER)
                            {
                                if (HasEffect(BRD.Buffs.RagingStrike))
                                {
                                    return BRD.Bloodletter;
                                }

                                if (GetCooldown(BRD.RagingStrikes).CooldownRemaining > 90)
                                {
                                    return BRD.Bloodletter;
                                }
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
                                if (IsEnemyCasting() && GetEnemyCastingTimeRemaining() <= 5 &&
                                    GetEnemyCastingTimeRemaining() >= 1)
                                    //PluginLog.Debug("Casting type: " + GetCastingType() + " - " + GetCastingName());
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
                    }
                }

                if (actionID == BRD.PvpSkills.BurstShot)
                {
                    var gauge = GetJobGauge<BRDGauge>();

                    if (GetPlayerHealth() <= 30 &&
                        GetCooldown(BRD.PvpSkills.Potion).RemainingCharges > 0)
                    {
                        return BRD.PvpSkills.Potion;
                    }

                    if (!IsOnCooldown(BRD.PvpSkills.HeadGaze) && GetTargetHealth() <= 25 &&
                        !TargetHasEffect(BRD.Debuffs.CantSilence))
                    {
                        return BRD.PvpSkills.HeadGaze;
                    }


                    if (!IsUnderGcd(BRD.PvpSkills.BurstShot))
                    {
                        if (gauge.SongTimer <= 1)
                        {
                            if (!IsOnCooldown(BRD.PvpSkills.WanderMinuet))
                                return BRD.PvpSkills.WanderMinuet;
                            if (!IsOnCooldown(BRD.PvpSkills.ArmyPeon))
                                return BRD.PvpSkills.ArmyPeon;
                        }

                        if (!IsOnCooldown(BRD.PvpSkills.Nature) &&
                            (LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp < 80)
                            return BRD.PvpSkills.Nature;

                        if (!IsOnCooldown(BRD.PvpSkills.Shadowbite))
                            return BRD.PvpSkills.Shadowbite;
                        if (!IsOnCooldown(BRD.PvpSkills.Sidewinder))
                            return BRD.PvpSkills.Sidewinder;
                        if (GetCooldown(BRD.PvpSkills.EmpyrealArrow).RemainingCharges > 0)
                            return BRD.PvpSkills.EmpyrealArrow;

                        if (!IsOnCooldown(BRD.PvpSkills.PitchPerfect) && gauge.SongTimer >= 0 &&
                            gauge.Repertoire == 3 &&
                            gauge.Song == Song.WANDERER)
                            return BRD.PvpSkills.PitchPerfect;

                        if (!IsOnCooldown(BRD.PvpSkills.PitchPerfect) && gauge.SongTimer >= 0 &&
                            gauge.Song == Song.WANDERER)
                        {
                            if (gauge.Repertoire == 3)
                            {
                                return BRD.PvpSkills.PitchPerfect;
                            }


                            if (gauge.SongTimer < 6000)
                            {
                                return BRD.PvpSkills.PitchPerfect;
                            }
                        }


                        if ((System.Numerics.Vector3.Distance(CurrentTarget.Position, LocalPlayer.Position) -
                             CurrentTarget.HitboxRadius) <= 6.0 && IsOffCooldown(BRD.PvpSkills.RepellingShot))
                        {
                            return BRD.PvpSkills.RepellingShot;
                        }

                        if (gauge.SoulVoice >= 50 && !IsOnCooldown(BRD.PvpSkills.ApexArrow) &&
                            !GetCooldown(BRD.PvpSkills.Concentrate).IsCooldown)
                            return BRD.PvpSkills.Concentrate;


                        if (!IsOnCooldown(BRD.PvpSkills.Recuperate) && GetPlayerHealth() <= 10)
                        {
                            return BRD.PvpSkills.Recuperate;
                        }
                    }

                    if (gauge.SoulVoice >= 50 && !IsOnCooldown(BRD.PvpSkills.ApexArrow))
                        return BRD.PvpSkills.ApexArrow;
                    return BRD.PvpSkills.BurstShot;
                }
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
        List<string> blackListedBuffMobs = new List<string>() {"Sunbird"};

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == BRD.QuickNock || actionID == BRD.Ladonsbite)
            {
                var gauge2 = GetJobGauge<BRDGauge>();
                var gauge = GetJobGauge<BRDGauge>();
                var codaCount = 0;
                var sec_to_add = 0;
                var canUseBuffWindow = true;

                foreach (string mobName in blackListedBuffMobs)
                {
                    if (GetTargetName() == mobName)
                    {
                        canUseBuffWindow = false;
                    }
                }

                foreach (Song song in gauge2.Coda)
                {
                    if (song == Song.NONE)
                    {
                        codaCount = codaCount + 1;
                    }
                }

                if (gauge.Song == Song.WANDERER)
                {
                    sec_to_add = BRD.DOT_TIME_LEFT_TO_ADD;
                }


                if (IsUnderGcd(BRD.GCD_SKILL))
                {
                    if (level >= BRD.Levels.Shadowbite &&
                        HasEffect(BRD.Buffs.ShadowbiteReady))
                        // Refulgent Arrow
                        return OriginalHook(BRD.Shadowbite);


                    if (gauge.SoulVoice >= 80 &&
                        gauge.Song == Song.MAGE && gauge.SongTimer >= 21000)
                        return BRD.ApexArrow;

                    if (gauge.SoulVoice >= 80 &&
                        HasEffect(BRD.Buffs.RagingStrike) &&
                        HasEffect(BRD.Buffs.BattleVoice))
                        return BRD.ApexArrow;

                    if (level >= BRD.Levels.BlastShot &&
                        HasEffect(BRD.Buffs.BlastShotReady))
                        return BRD.BlastArrow;

                    return OriginalHook(BRD.QuickNock);
                }


                if (!IsUnderGcd(BRD.GCD_SKILL))
                {
                    if (level >= ALL.Levels.HeadGraze && CanInterruptEnemy() &&
                        !GetCooldown(ALL.HeadGraze).IsCooldown)
                    {
                        return ALL.HeadGraze;
                    }

                    if (canUseBuffWindow)
                    {
                        if (!IsOnCooldown(BRD.WandererMinet) && level >= BRD.Levels.WandererMinet &&
                            gauge.Song == Song.NONE && gauge.SongTimer <= 2000)
                        {
                            return BRD.WandererMinet;
                        }


                        if (!IsOnCooldown(BRD.Mageballad) && level >= BRD.Levels.Mageballad &&
                            gauge.Song == Song.NONE &&
                            gauge.SongTimer <= 12000)
                        {
                            return BRD.Mageballad;
                        }

                        if (level >= BRD.Levels.ArmyPeon && !IsOnCooldown(BRD.ArmyPeon) &&
                            gauge.Song == Song.MAGE &&
                            gauge.SongTimer <= 11000)
                        {
                            return BRD.ArmyPeon;
                        }

                        if (!IsOnCooldown(BRD.ArmyPeon) && level >= BRD.Levels.ArmyPeon && gauge.Song == Song.NONE)
                        {
                            return BRD.ArmyPeon;
                        }
                    }


                    if (canUseBuffWindow)
                    {
                        if (!IsOnCooldown(BRD.RagingStrikes) && level >= BRD.Levels.RagingStrikes &&
                            GetCooldown(BRD.RadiantFinal).CooldownRemaining <= 4 &&
                            GetCooldown(BRD.BattleVoice).CooldownRemaining <= 4)
                            return BRD.RagingStrikes;


                        if (level < BRD.Levels.RadiantFinal && level >= BRD.Levels.Barrage)
                        {
                            if (!IsOnCooldown(BRD.RagingStrikes) && level >= BRD.Levels.RagingStrikes &&
                                GetCooldown(BRD.BattleVoice).CooldownRemaining <= 4)
                                return BRD.RagingStrikes;

                            if (level < BRD.Levels.BattleVoice)
                            {
                                if (!IsOnCooldown(BRD.RagingStrikes) && level >= BRD.Levels.RagingStrikes)
                                    return BRD.RagingStrikes;
                            }
                        }


                        if (codaCount <= 2 && !IsOnCooldown(BRD.RadiantFinal) && level >= BRD.Levels.RadiantFinal &&
                            HasEffect(BRD.Buffs.RagingStrike) && FindEffect(BRD.Buffs.RagingStrike) != null &&
                            FindEffect(BRD.Buffs.RagingStrike).RemainingTime <= 17)
                        {
                            return BRD.RadiantFinal;
                        }


                        if (!IsOnCooldown(BRD.BattleVoice) && level >= BRD.Levels.BattleVoice &&
                            FindEffect(BRD.Buffs.RagingStrike) != null &&
                            FindEffect(BRD.Buffs.RagingStrike).RemainingTime <= 17)
                        {
                            if (gauge.Song != Song.NONE)
                            {
                                return BRD.BattleVoice;
                            }
                        }
                    }


                    if (canUseBuffWindow)
                    {
                        if (level >= BRD.Levels.WandererMinet && !IsOnCooldown(BRD.WandererMinet) &&
                            gauge.Song == Song.ARMY)
                        {
                            return BRD.WandererMinet;
                        }
                    }


                    if (!IsOnCooldown(BRD.EmpyrealArrow) && level >= BRD.Levels.EmpyrealArrow)
                    {
                        return BRD.EmpyrealArrow;
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


                    if (!IsOnCooldown(BRD.Barrage) && level >= BRD.Levels.Barrage &&
                        !HasEffect(BRD.Buffs.StraightShotReady) && HasEffect(BRD.Buffs.RadiantFinal) &&
                        HasEffect(BRD.Buffs.RagingStrike) && HasEffect(BRD.Buffs.BattleVoice))
                    {
                        return BRD.Barrage;
                    }

                    if (level < BRD.Levels.RadiantFinal && level >= BRD.Levels.Barrage)
                    {
                        if (!IsOnCooldown(BRD.Barrage) && level >= BRD.Levels.Barrage &&
                            !HasEffect(BRD.Buffs.StraightShotReady) && HasEffect(BRD.Buffs.RagingStrike) &&
                            HasEffect(BRD.Buffs.BattleVoice))
                        {
                            return BRD.Barrage;
                        }

                        if (level < BRD.Levels.BattleVoice)
                        {
                            if (!IsOnCooldown(BRD.Barrage) && level >= BRD.Levels.Barrage &&
                                !HasEffect(BRD.Buffs.StraightShotReady) && HasEffect(BRD.Buffs.RagingStrike))
                            {
                                return BRD.Barrage;
                            }
                        }
                    }


                    if (!IsOnCooldown(BRD.Sidewinder) && level >= BRD.Levels.Sidewinder)
                    {
                        if (GetCooldown(BRD.RagingStrikes).CooldownRemaining > 15 &&
                            !HasEffect(BRD.Buffs.RagingStrike) &&
                            GetCooldown(BRD.RagingStrikes).CooldownRemaining < 115)
                            return BRD.Sidewinder;

                        if (HasEffect(BRD.Buffs.RadiantFinal) && HasEffect(BRD.Buffs.RagingStrike) &&
                            HasEffect(BRD.Buffs.BattleVoice))
                            return BRD.Sidewinder;

                        if (level < BRD.Levels.RadiantFinal && level >= BRD.Levels.Sidewinder)
                        {
                            if (HasEffect(BRD.Buffs.RagingStrike) &&
                                HasEffect(BRD.Buffs.BattleVoice))
                            {
                                return BRD.Sidewinder;
                            }

                            if (level < BRD.Levels.BattleVoice)
                            {
                                if (HasEffect(BRD.Buffs.RagingStrike))
                                {
                                    return BRD.Sidewinder;
                                }
                            }
                        }
                    }

                    if ((LocalPlayer.CurrentHp * 100) / LocalPlayer.MaxHp <= 40 &&
                        IsOffCooldown(ALL.SecondWind) && level >= ALL.Levels.SecondWind)
                    {
                        return ALL.SecondWind;
                    }


                    if (level >= BRD.Levels.RainOfDeath &&
                        GetCooldown(BRD.RainOfDeath).RemainingCharges > 0)
                    {
                        if (HasEffect(BRD.Buffs.RadiantFinal) || HasEffect(BRD.Buffs.RagingStrike))
                        {
                            return BRD.RainOfDeath;
                        }

                        if (gauge.Song == Song.ARMY && GetCooldown(BRD.RainOfDeath).RemainingCharges >= 2)
                        {
                            if (GetCooldown(BRD.RagingStrikes).CooldownRemaining > 10 &&
                                GetCooldown(BRD.RainOfDeath).RemainingCharges >= 2)
                            {
                                return BRD.RainOfDeath;
                            }
                        }

                        if (gauge.Song == Song.MAGE)
                        {
                            return BRD.RainOfDeath;
                        }

                        if (gauge.Song == Song.WANDERER)
                        {
                            return BRD.RainOfDeath;
                        }

                        if (gauge.Song == Song.NONE)
                        {
                            return BRD.RainOfDeath;
                        }
                    }

                    if (level >= BRD.Levels.NatureMinne)
                    {
                        if (IsOffCooldown(BRD.NatureMinne) && GetPlayerHealth() <= 50)
                        {
                            return BRD.NatureMinne;
                        }
                    }
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