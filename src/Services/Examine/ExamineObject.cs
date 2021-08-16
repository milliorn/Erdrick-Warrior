using Anvil.API;

using Anvil.Services;

namespace Services.Examine
{
    [ServiceBinding(typeof(ExamineObject))]
    public class ExamineObject
    {
        public ExamineObject() => NwModule.Instance.OnExamineObject += Examine =>
        {
            var examinedBy = Examine.ExaminedBy;

            if (Examine.ExaminedObject is NwCreature creature && examinedBy.ControlledCreature.IsReactionTypeHostile(creature) && examinedBy.ControlledCreature.GetSkillRank(Skill.Lore) > creature.Level)
            {
                creature.Description = PrintCRValue(creature);
            }
        };

        private static string PrintCRValue(NwCreature npc) => $"CR Value: {npc.ChallengeRating}\n\nSTR: {npc.GetAbilityScore(Ability.Strength)}\nDEX: {npc.GetAbilityScore(Ability.Dexterity)}\nCON: {npc.GetAbilityScore(Ability.Constitution)}\nINT: {npc.GetAbilityScore(Ability.Intelligence)}\nWIS: {npc.GetAbilityScore(Ability.Wisdom)}\nCHA: {npc.GetAbilityScore(Ability.Charisma)}\nAC: {npc.AC}\nHP: {npc.HP}\nBAB: {npc.BaseAttackBonus}\nFortitude: {npc.GetBaseSavingThrow(SavingThrow.Fortitude)}\nReflex: {npc.GetBaseSavingThrow(SavingThrow.Reflex)}\nWill: {npc.GetBaseSavingThrow(SavingThrow.Will)}\nSR: {npc.SpellResistance}\n\n{npc.OriginalDescription}";
    }
}