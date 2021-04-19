using NWN.API;
using NWN.API.Events;
using NWN.Services;

namespace Services.Module
{
    [ServiceBinding(typeof(Rest))]
    public class Rest
    {
        //private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public Rest() => NwModule.Instance.OnPlayerRest += rest =>
        {
            switch (rest.RestEventType)
            {
                case NWN.API.Constants.RestEventType.Started:
                    rest.Player.FadeToBlack((float)0.003);
                    break;
                case NWN.API.Constants.RestEventType.Invalid:
                    break;
                case NWN.API.Constants.RestEventType.Finished:
                case NWN.API.Constants.RestEventType.Cancelled:
                    rest.Player.FadeFromBlack((float)0.003);
                    rest.Player.SaveCharacter();
                    break;
                default: break;
            }
        };
    }
}