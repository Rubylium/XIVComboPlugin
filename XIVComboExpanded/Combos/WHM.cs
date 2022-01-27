using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedPlugin.Combos
{
    internal static class WHM
    {
        public const byte ClassID = 6;
        public const byte JobID = 24;

        public const uint
            Cure = 120,
            Stone = 119,
            Glare = 16533,
            Glare3 = 25859,
            Dia = 16532,
            Aero = 121,
            Aero2 = 132,
            Medica = 124,
            Cure2 = 135,
            PresenceOfMind = 136,
            Holy = 139,
            Benediction = 140,
            Asylum = 3569,
            Tetragrammaton = 3570,
            Assize = 3571,
            PlenaryIndulgence = 7433,
            AfflatusSolace = 16531,
            AfflatusRapture = 16534,
            AfflatusMisery = 16535,
            Temperance = 16536,
            Holy3 = 25860,
            Regen = 137,
            Aquaveil = 25861,
            LiturgyOfTheBell = 25862;

        public static class Buffs
        {
            public const ushort
                Freecure = 155;
        }

        public static class Debuffs
        {
            public const ushort
                Aero = 143,
                Aero2 = 144,
                Aero3 = 798,
                Dia = 1871;
        }

        public static class Levels
        {
            public const byte
                Cure2 = 30,
                Stone = 1,
                Glare = 72,
                Glare3 = 82,
                Aero = 4,
                Aero2 = 46,
                Dia = 72,
                Regen = 35,
                Benediction = 50,
                Tetragrammaton = 60,
                AfflatusSolace = 52,
                AfflatusMisery = 74,
                AfflatusRapture = 76;
        }
    }


    internal class WhiteMageCure2 : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WhmAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.Cure)
            {
                var gauge = GetJobGauge<WHMGauge>();

                if (level >= ALL.Levels.LucidDream && GetPlayerMana() <= 40 && IsOffCooldown(ALL.LucidDream))
                    return ALL.LucidDream;

                if (level >= WHM.Levels.AfflatusSolace && gauge.Lily > 0 && GetTargetHealth() <= 60)
                    return WHM.AfflatusSolace;

                if (level >= WHM.Levels.Regen && GetTargetHealth() >= 90 && IsOffCooldown(WHM.Regen))
                    return OriginalHook(WHM.Regen);

                if (level >= WHM.Levels.Tetragrammaton && GetTargetHealth() <= 35 && IsOffCooldown(WHM.Tetragrammaton))
                    return WHM.Tetragrammaton;

                if (level >= WHM.Levels.Benediction && GetTargetHealth() <= 35 && GetTargetHealth() > 0 &&
                    IsOffCooldown(WHM.Benediction))
                    return WHM.Benediction;

                if (level >= WHM.Levels.Cure2 && HasEffect(WHM.Buffs.Freecure))
                    return WHM.Cure2;
            }

            if (actionID == WHM.Stone)
            {
                var gauge = GetJobGauge<WHMGauge>();
                if (level >= ALL.Levels.LucidDream && GetPlayerMana() <= 40 && IsOffCooldown(ALL.LucidDream))
                    return ALL.LucidDream;

                if (level >= WHM.Levels.AfflatusMisery && gauge.BloodLily == 3)
                    return WHM.AfflatusMisery;

                if (level >= WHM.Levels.Dia)
                {
                    if (!TargetHasEffect(WHM.Debuffs.Dia))
                    {
                        return OriginalHook(WHM.Dia);
                    }

                    if (FindTargetEffect(WHM.Debuffs.Dia).RemainingTime <= 10)
                    {
                        return OriginalHook(WHM.Dia);
                    }
                }

                if (level < WHM.Levels.Dia)
                {
                    if (level >= WHM.Levels.Aero2 && !TargetHasEffect(WHM.Debuffs.Aero2))
                        return OriginalHook(WHM.Aero);

                    if (level < WHM.Levels.Aero2 && level >= WHM.Levels.Aero && !TargetHasEffect(WHM.Debuffs.Aero))
                        return OriginalHook(WHM.Aero);


                    if (level >= WHM.Levels.Aero2 && FindTargetEffect(WHM.Debuffs.Aero).RemainingTime <= 10)
                        return OriginalHook(WHM.Aero);

                    if (level < WHM.Levels.Aero2 && level >= WHM.Levels.Aero &&
                        FindTargetEffect(WHM.Debuffs.Aero2).RemainingTime <= 10)
                        return OriginalHook(WHM.Aero);
                }


                return OriginalHook(WHM.Stone);
            }

            return actionID;
        }
    }

    internal class WhiteMageAfflatusSolace : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WhiteMageSolaceMiseryFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.AfflatusSolace)
            {
                var gauge = GetJobGauge<WHMGauge>();

                if (level >= WHM.Levels.AfflatusMisery && gauge.BloodLily == 3)
                    return WHM.AfflatusMisery;
            }

            return actionID;
        }
    }

    internal class WhiteMageAfflatusRapture : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WhiteMageRaptureMiseryFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.AfflatusRapture)
            {
                var gauge = GetJobGauge<WHMGauge>();

                if (level >= WHM.Levels.AfflatusMisery && gauge.BloodLily == 3 && HasTarget())
                    return WHM.AfflatusMisery;
            }

            return actionID;
        }
    }

    internal class WhiteMageHoly : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WhiteMageHolyMiseryFeature;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.Holy || actionID == WHM.Holy3)
            {
                var gauge = GetJobGauge<WHMGauge>();

                if (level >= WHM.Levels.AfflatusMisery && gauge.BloodLily == 3 && HasTarget())
                    return WHM.AfflatusMisery;
            }

            return actionID;
        }
    }


    internal class WhiteMageMedica : CustomCombo
    {
        protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.WhmAny;

        protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
        {
            if (actionID == WHM.Medica)
            {
                var gauge = GetJobGauge<WHMGauge>();

                if (IsEnabled(CustomComboPreset.WhiteMageRaptureMiseryFeature))
                {
                    if (level >= WHM.Levels.AfflatusMisery && gauge.BloodLily == 3 && HasTarget())
                        return WHM.AfflatusMisery;
                }

                if (IsEnabled(CustomComboPreset.WhiteMageAfflatusFeature))
                {
                    if (level >= WHM.Levels.AfflatusRapture && gauge.Lily > 0)
                        return WHM.AfflatusRapture;
                }
            }

            return actionID;
        }
    }
}