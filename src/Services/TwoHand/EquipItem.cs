using NWN.API;
using NWN.Services;

namespace Services.TwoHand
{
    [ServiceBinding(typeof(EquipItem))]
    public class EquipItem
    {
        public static void OnEquip() => NwModule.Instance.OnPlayerEquipItem += equipItem =>
        {
            if (equipItem.Player is NwPlayer && !equipItem.Player.CheckCreatureSize() || !equipItem.Player.HasShieldEquipped() && equipItem.Player.CheckCreaturekSizeAndWeapon())
            {
                equipItem.Player.AddBuff();
            }
        };
    }
}
