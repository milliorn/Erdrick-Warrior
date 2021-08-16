//using NLog;
using Anvil.API;


namespace Services.TwoHand
{
    public static class Extensions
    {
        //private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static void AddBuff(this NwCreature creature)
        {
            Effect acBoost = Effect.ACIncrease(5, ACBonus.ShieldEnchantment);
            acBoost.Tag = "AC_BOOST";
            creature.ApplyEffect(EffectDuration.Permanent, acBoost);
            creature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.ImpAcBonus));
        }

        public static void RemoveBuff(this NwCreature creature)
        {
            foreach (Effect effect in creature.ActiveEffects)
            {
                if (effect.Tag == "AC_BOOST")
                {
                    creature.RemoveEffect(effect);
                }
            }
        }

        public static bool StopScript(this NwCreature creature) => creature.CheckCreatureSize() || creature.HasShieldEquipped();

        public static bool CheckCreaturekSizeAndWeapon(this NwCreature creature) => creature.Size.Equals(CreatureSize.Medium) && HasTwoHandLargeWeapon(creature) ||
            creature.Size.Equals(CreatureSize.Small) && HasMediumWeapon(creature) ||
            creature.Size.Equals(CreatureSize.Tiny) && HasSmallmWeapon(creature);

        public static bool CheckCreatureSize(this NwCreature creature) => creature.Size is
            CreatureSize.Huge or
            CreatureSize.Invalid or
            CreatureSize.Large;

        public static bool HasShieldEquipped(this NwCreature creature) => creature.GetItemInSlot(InventorySlot.LeftHand)?.BaseItemType is
            BaseItemType.LargeShield or
            BaseItemType.SmallShield or
            BaseItemType.TowerShield;

        private static bool HasSmallmWeapon(NwCreature creature) => creature.GetItemInSlot(InventorySlot.RightHand)?.BaseItemType is
            BaseItemType.Handaxe or
            BaseItemType.Kama or
            BaseItemType.LightCrossbow or
            BaseItemType.LightHammer or
            BaseItemType.LightMace or
            BaseItemType.Shortsword or
            BaseItemType.Sickle or
            BaseItemType.Sling or
            BaseItemType.ThrowingAxe;

        private static bool HasMediumWeapon(NwCreature creature) => creature.GetItemInSlot(InventorySlot.RightHand)?.BaseItemType is
            BaseItemType.Battleaxe or
            BaseItemType.Bastardsword or
            BaseItemType.Club or
            BaseItemType.DwarvenWaraxe or
            BaseItemType.HeavyCrossbow or
            BaseItemType.Katana or
            BaseItemType.Morningstar or
            BaseItemType.LightFlail or
            BaseItemType.Longsword or
            BaseItemType.MagicStaff or
            BaseItemType.Rapier or
            BaseItemType.Scimitar or
            BaseItemType.Shortbow or
            BaseItemType.Warhammer;

        private static bool HasTwoHandLargeWeapon(NwCreature creature) => creature.GetItemInSlot(InventorySlot.RightHand)?.BaseItemType is
            BaseItemType.DireMace or
            BaseItemType.Doubleaxe or
            BaseItemType.Greataxe or
            BaseItemType.Greatsword or
            BaseItemType.Halberd or
            BaseItemType.HeavyFlail or
            BaseItemType.Longbow or
            BaseItemType.Quarterstaff or
            BaseItemType.Scythe or
            BaseItemType.ShortSpear or
            BaseItemType.Trident or
            BaseItemType.TwoBladedSword;
    }
}