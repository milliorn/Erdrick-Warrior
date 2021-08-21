using NLog;

using Anvil.API;
using Anvil.Services;

namespace Services.Client
{
    [ServiceBinding(typeof(Leave))]
    public class Leave
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Leave() => NwModule.Instance.OnClientLeave += leave =>
        {
            if (leave.Player is NwPlayer player)
            {
                player.PrintLogout();

                if (player.IsDM) return;

                player.DeathLog();
                //player.StoreHitPoints();
            }
        };
    }
}