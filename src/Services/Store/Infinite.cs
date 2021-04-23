using NWN.API.Events;
using NWN.Services;

namespace Services.Store
{
    [ServiceBinding(typeof(Infinite))]

    public class Infinite
    {
        [ScriptHandler("store_infinite")]
        public static void SetupStore(StoreEvents.OnOpen obj)
        {
            foreach (var item in obj.Player.Inventory.Items)
            {
                item.Infinite = true;
                item.Stolen = true;
            }
        }
    }
}