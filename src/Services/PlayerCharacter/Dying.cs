using System;
using System.Threading.Tasks;
//using NLog;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Services.PlayerCharacter
{
    [ServiceBinding(typeof(Dying))]
    public class Dying
    {
        //private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Dying() => NwModule.Instance.OnPlayerDying += async dying =>
        {
            if (dying.Player is NwPlayer)
            {
                await BleedAsync(dying);
            }
            else
            {
                dying.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.Death(true));
            }
        };

        private async Task BleedAsync(ModuleEvents.OnPlayerDying dying)
        {
            await NwTask.WhenAny(NwTask.Run(async () =>
            {
                await NwTask.Delay(TimeSpan.FromSeconds(1));
            }));

            dying.Player.ScreamOnDying();

            int stabilize = new Random(0).Next(1, 10);
            dying.Player.SendServerMessage($"Stabilize roll:{stabilize.ToString().ColorString(ColorConstants.White)}.".ColorString(ColorConstants.Orange));

            if (dying.Player.ControlledCreature.HP <= -127)
            {
                dying.Player.PlayerHasDied();
                return;
            }
            else if (stabilize == 1)
            {
                dying.Player.PlayerHasStabilized();
                return;
            }
            else
            {
                await BleedAsync(dying);
            }
        }
    }
}