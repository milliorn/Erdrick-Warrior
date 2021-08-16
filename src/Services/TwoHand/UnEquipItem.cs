using Anvil.API;
using Anvil.Services;

namespace Services.TwoHand
{
    [ServiceBinding(typeof(UnEquipItem))]
    public class UnEquipItem
    {
        public UnEquipItem()
        {
            NwModule.Instance.OnPlayerUnequipItem += unequipItem =>
            {
                if (unequipItem.Item is NwItem && unequipItem.UnequippedBy.ControllingPlayer is NwPlayer player)
                {
                    player.ControlledCreature.RemoveBuff();
                }
            };
        }
    }
}