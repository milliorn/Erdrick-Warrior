using NWN.API;
using NWN.Services;

namespace Services.TwoHand
{
    [ServiceBinding(typeof(EquipItem))]
    public class EquipItem
    {
        public EquipItem()
        {
            NwModule.Instance.OnPlayerEquipItem += equipItem =>
            {
                if (equipItem.Item is NwItem && equipItem.Player is NwPlayer player && (!player.CheckCreatureSize() || !player.HasShieldEquipped() && !player.CheckCreaturekSizeAndWeapon()))
                {
                    player.AddBuff();
                }
            };
        }
    }
}
