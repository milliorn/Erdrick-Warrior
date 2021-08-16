using System.Linq;
using Anvil.API;
using NWN.API.Constants;
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

            if (spellCast.Caster is NwPlayer player)
            {
                Spell spell = spellCast.Spell;

                ReplenishCantrips(spellCast);

                if (player.ControlledCreature.Area.GetLocalVariable<int>("NO_CASTING").Value == 1 && spellCast.Harmful)
                {
                    //SetModuleOverrideSpellScriptFinished
                    player.SendServerMessage($"{"NO".ColorString(Color.RED)} {"offensive spellcasting".ColorString(Color.ORANGE)} in this area.");
                }

                //Bugged
                BuffPetsAsync(spellCast, player, spell);
            }
        }

        private static void BuffPetsAsync(SpellEvents.OnSpellCast spellCast, NwPlayer player, Spell spell)
        {
            foreach (var pet in player.ControlledCreature.Faction.GetMembers<NwPlayer>().Where(p => !p.IsDM && !p.IsPlayerDM))
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