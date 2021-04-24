using NWN.API;
using NWN.Services;

namespace Services.TwoHand
{
    [ServiceBinding(typeof(UnEquipItem))]
    public class UnEquipItem
    {
        public UnEquipItem()
        {
            NwModule.Instance.OnPlayerUnequipItem += unequipItem =>
            {
                if (unequipItem.Item is NwItem && unequipItem.UnequippedBy is NwPlayer player)
                {
                    player.RemoveBuff();
                }
            };
        }
    }
}