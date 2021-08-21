using Anvil.API.Events;
using Anvil.Services;

namespace Services.Store
{
    [ServiceBinding(typeof(Infinite))]

    public class Infinite
    {
        [ScriptHandler("nw_d1_startstore")]
        public static void SetupStore(StoreEvents.OnOpen obj)
        {
            foreach (var item in obj.Store.Items)
            {
                item.Infinite = true;
                item.Stolen = false;
            }
        }
    }
}