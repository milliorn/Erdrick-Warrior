using System.Linq;
using NLog;
using NWN.API;
using NWN.API.Events;

using NWN.Services;

namespace Services.Area
{
    [ServiceBinding(typeof(CleanMap))]
    public class CleanMap
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [ScriptHandler("area_cleanup")]
        public void Exit(CallInfo callinfo)
        {
            if (callinfo.TryGetEvent(out AreaEvents.OnExit onLeave) && !onLeave.Area.FindObjectsOfTypeInArea<NwPlayer>().Any())
            {
                onLeave.CleanupCreatures();
                onLeave.CleanupItems();
                onLeave.CleanupPlaceables();
                onLeave.CloseDoors();
            }
        }
    }
}