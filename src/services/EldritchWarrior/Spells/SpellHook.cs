using System.Linq;
using Anvil.API;

using Anvil.API.Events;
using Anvil.Services;

namespace Services.Spells
{
    [ServiceBinding(typeof(SpellHook))]
    public class SpellHook
    {
        [ScriptHandler("spellhook")]
        private void OnSpellHooked(CallInfo callInfo)
        {
            SpellEvents.OnSpellCast spellCast = new SpellEvents.OnSpellCast();
            
            if (spellCast.Caster.IsPlayerControlled(out NwPlayer player))
            {
                Spell spell = spellCast.Spell;

                ReplenishCantrips(spellCast);
                
                foreach (var _ in spellCast.Caster.Area.LocalVariables.Where(things => things.Name == "NO_CASTING" && things.Equals(1) && spellCast.Harmful))
                {
                    player.SendServerMessage($"{"NO".ColorString(ColorConstants.Red)} {"offensive spellcasting".ColorString(ColorConstants.Orange)} in this area.");
                }

                //Bugged
                BuffPetsAsync(spellCast, player, spell);
            }
        }

        private static void BuffPetsAsync(SpellEvents.OnSpellCast spellCast, NwPlayer player, Spell spell)
        {
            foreach (var pet in player.ControlledCreature.Faction.GetMembers())
            {
                switch (pet.AssociateType)
                {
                    case AssociateType.AnimalCompanion:
                        System.Console.WriteLine(pet.Name);
                        //await player.ControlledCreature.GetAssociate(AssociateType.AnimalCompanion).ActionCastSpellAt(spellCast.Spell, player.ControlledCreature.GetAssociate(AssociateType.AnimalCompanion), spellCast.MetaMagicFeat, true, 0, ProjectilePathType.Default, true);
                        break;
                    case AssociateType.Dominated:
                        System.Console.WriteLine(pet.Name);
                        //await player.ControlledCreature.GetAssociate(AssociateType.Dominated).ActionCastSpellAt(spellCast.Spell, player.ControlledCreature.GetAssociate(AssociateType.Dominated), spellCast.MetaMagicFeat, true, 0, ProjectilePathType.Default, true);
                        break;
                    case AssociateType.Familiar:
                        System.Console.WriteLine(pet.Name);
                        //await player.ControlledCreature.GetAssociate(AssociateType.Familiar).ActionCastSpellAt(spellCast.Spell, player.ControlledCreature.GetAssociate(AssociateType.Familiar), spellCast.MetaMagicFeat, true, 0, ProjectilePathType.Default, true);
                        break;
                    case AssociateType.Henchman:
                        System.Console.WriteLine(pet.Name);
                        //await player.ControlledCreature.GetAssociate(AssociateType.Henchman).ActionCastSpellAt(spellCast.Spell, player.ControlledCreature.GetAssociate(AssociateType.Henchman), spellCast.MetaMagicFeat, true, 0, ProjectilePathType.Default, true);
                        break;
                    case AssociateType.Summoned:
                        System.Console.WriteLine(pet.Name);
                        //await player.ControlledCreature.GetAssociate(AssociateType.Summoned).ActionCastSpellAt(spellCast.Spell, player.ControlledCreature.GetAssociate(AssociateType.Summoned), spellCast.MetaMagicFeat, true, 0, ProjectilePathType.Default, true);
                        break;
                }
            }
        }

        private static void ReplenishCantrips(SpellEvents.OnSpellCast spellCast)
        {
            switch (spellCast.Spell)
            {
                case Spell.AcidSplash:
                case Spell.CureMinorWounds:
                case Spell.Daze:
                case Spell.ElectricJolt:
                case Spell.Flare:
                case Spell.InflictMinorWounds:
                case Spell.Light:
                case Spell.RayOfFrost:
                case Spell.Resistance:
                case Spell.Virtue:
                    break;
            }
        }
    }
}