using Anvil.API;
using Anvil.Services;

namespace Services.Item
{
    [ServiceBinding(typeof(UnEquip))]
    public class UnEquip
    {
        //private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public UnEquip()
        {
            NwModule.Instance.OnPlayerUnequipItem += unEquip =>
            {
                if (unEquip.UnequippedBy.IsPlayerControlled)
                {
                    unEquip.Item.PrintGPValueOnItem();
                }
            };
        }
    }
}