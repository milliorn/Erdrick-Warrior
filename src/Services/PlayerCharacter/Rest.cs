using Anvil.API;
using Anvil.Services;

namespace Services.PlayerCharacter
{
	[ServiceBinding(typeof(Rest))]
	public class Rest
	{
		//private static readonly Logger Log = LogManager.GetCurrentClassLogger();
		public Rest() => NwModule.Instance.OnPlayerRest += rest =>
		{
			switch (rest.RestEventType)
			{
				case RestEventType.Started:
					rest.Player.FadeToBlack((float)0.003);
					break;
				case RestEventType.Invalid:
					break;
				case RestEventType.Finished:
				case RestEventType.Cancelled:
					rest.Player.FadeFromBlack((float)0.003);
					Module.Extensions.SaveCharacter(rest.Player);
					break;
				default: break;
			}
		};
	}
}
