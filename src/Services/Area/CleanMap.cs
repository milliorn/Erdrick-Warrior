using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using NLog;

namespace Services.Area
{
    [ServiceBinding(typeof(CleanMap))]
    public class CleanMap
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [ScriptHandler("area_cleanup")]
        public void Exit(CallInfo callinfo)
        {
            if (callinfo.TryGetEvent(out AreaEvents.OnExit onLeave) && onLeave.Area.FindObjectsOfTypeInArea<NwCreature>().Any(x => x.IsLoginPlayerCharacter))
            {
                onLeave.CleanupCreatures();
                onLeave.CleanupItems();
                onLeave.CleanupPlaceables();
                onLeave.CloseDoors();
            }
        }
    }
}
