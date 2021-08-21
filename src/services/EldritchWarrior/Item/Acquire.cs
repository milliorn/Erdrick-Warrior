//using NLog;
using Anvil.API;
using Anvil.Services;

namespace Services.Item
{
    [ServiceBinding(typeof(Acquire))]

    public class Acquire
    {
        //private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Acquire()
        {
            NwModule.Instance.OnAcquireItem += acquireItem =>
            {
                if (acquireItem.Item is NwItem item)
                {
                    item.PrintGPValueOnItem();
                    item.RemoveAllTemporaryItemProperties();
                    item.NotifyLoot();
                }

                /* This is to short circuit the rest of this code if we are DM */
                if (acquireItem.AcquiredBy.IsPlayerControlled(out NwPlayer player))
                {
                    if (player.IsDM) return;
                }

                if (acquireItem.AcquiredBy.IsPlayerControlled(out NwPlayer playerA) && acquireItem.AcquiredBy.IsPlayerControlled(out NwPlayer playerB))
                {
                    playerA.FixBarterExploit(playerB);
                }
            };
        }
    }
}