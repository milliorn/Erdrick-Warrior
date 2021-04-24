using NWN.API;

using NWN.Services;

namespace Services.TwoHand
{
    [ServiceBinding(typeof(UnEquipItem))]
    public class UnEquipItem
    {
        public static void OnUnEquip() => NwModule.Instance.OnPlayerUnequipItem += unequipItem =>
        {
            if (unequipItem.UnequippedBy is NwPlayer)
            {
                unequipItem.UnequippedBy.RemoveBuff();
            }
        };
    }
}
