//using NLog;
using Anvil.API;

namespace Services.Item
{
    public class UnAcquire
    {
        public UnAcquire()
        {
            NwModule.Instance.OnUnacquireItem += unAcquire =>
            {
                if (unAcquire.LostBy is NwPlayer)
                {
                    unAcquire.Item.PrintGPValueOnItem();
                    unAcquire.Item.RemoveAllTemporaryItemProperties();
                }
            };
        }
    }
}