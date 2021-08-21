using System;
using System.Globalization;
using System.Linq;
using Anvil.API;

using Anvil.Services;

namespace Services.Module
{
    [ServiceBinding(typeof(Load))]
    public class Load
    {
        private static readonly int hours = 24;

        public Load(SchedulerService scheduler, MonkWeapons monkWeapons)
        {
            scheduler.ScheduleRepeating(InitAreaEnviroment, TimeSpan.FromHours(1));
            scheduler.ScheduleRepeating(ServerMessageEveryHour, TimeSpan.FromHours(1));
            scheduler.Schedule(ServerMessage1439, TimeSpan.FromMinutes(1439));

            /* NWNX */
            NwServer.Instance.ServerInfo.PlayOptions.RestoreSpellUses = true;

            /* Set Fog Color an Amount in all outdoor areas */
            InitAreaEnviroment();
            InitModuleSwitches();

            monkWeapons.Init();
            //LoadDiscord();
            /* Print to console when we boot*/
            PrintBootTime();
        }

        private static void PrintBootTime() => Console.WriteLine($"SERVER LOADED:{DateTime.Now.ToString(@"yyyy/MM/dd hh:mm:ss tt", new CultureInfo("en-US"))}");
        private static async void ServerMessage1439() => await NwModule.Instance.SpeakString($"Server reset in {"1".ColorString(ColorConstants.White)} minute.".ColorString(ColorConstants.Purple), TalkVolume.Shout);

        private static void InitModuleSwitches()
        {
            /* Module Switches */
            NwModule.Instance.GetObjectVariable<LocalVariableBool>("X2_L_STOP_EXPERTISE_ABUSE").Value = true;
            NwModule.Instance.GetObjectVariable<LocalVariableBool>("X2_L_NOTREASURE").Value = true;
            NwModule.Instance.GetObjectVariable<LocalVariableBool>("X3_MOUNTS_EXTERNAL_ONLY").Value = true;
            NwModule.Instance.GetObjectVariable<LocalVariableBool>("X3_MOUNTS_NO_UNDERGROUND").Value = true;
            NwModule.Instance.GetObjectVariable<LocalVariableBool>("X2_S_UD_SPELLSCRIPT").Value = true;
            NwModule.Instance.GetObjectVariable<PersistentVariableBool>("string").Value = true;//GetObjectVariable<LocalVariableBool>("X2_S_UD_SPELLSCRIPT").Value = true;
        }

        private static FogColor AreaSetFogColor(Random random)
        {
            var values = Enum.GetValues(typeof(FogColor));
            return (FogColor)values.GetValue(random.Next(values.Length))!;
        }

        private static void InitAreaEnviroment()
        {
            // Instantiate random number generator using system-supplied value as seed.
            Random random = new();

            /* Iterate all areas in module */
            foreach (NwArea area in NwModule.Instance.Areas.Where(area => !area.IsInterior))
            {
                area.SetFogColor(FogType.All, AreaSetFogColor(random));
                area.SetFogAmount(FogType.All, random.Next(1, 12));
                area.SkyBox = (Skybox)random.Next(Enum.GetNames(typeof(Skybox)).Length);
                area.Weather = (WeatherType)random.Next(Enum.GetNames(typeof(Skybox)).Length);
                AreaSkyBoxStormIcy(area);
            }
        }

        private static void AreaSkyBoxStormIcy(NwArea area)
        {
            if (area.SkyBox == Skybox.GrassStorm)
            {
                area.Weather = WeatherType.Rain;
            }

            if (area.SkyBox == Skybox.Icy)
            {
                area.Weather = WeatherType.Snow;
            }
        }

        /* private static void LoadDiscord()
			{
				// Create an instance of the class DiscordWebhookClient with your Discord webhook URL.
				var client = new DiscordWebhookClient("https://discord.com/api/webhooks/800544826633355295/CHSdo8brPx3EEHVPoGwnKaeHYVwzoIu9NbZBe3eM164gHGP_T6_nr0gk3vKyIj07a5vC");
				// Create your DiscordMessage with all parameters of your message.
				var message = new DiscordMessage(
					"Discord Webhook Client sent this message! " + DiscordEmoji.Grinning,
					username: "Username",
					avatarUrl: "https://avatars3.githubusercontent.com/u/24236993?s=460&v=4",
					tts: false,
					embeds: new[]
					{
						new DiscordMessageEmbed(
							"Embed title " + DiscordEmoji.Thumbsup,
							color: 0,
							author: new DiscordMessageEmbedAuthor("Embed 1 author name"),
							url: "https://github.com/jlnpinheiro/discord-webhook-client/",
							description: "This is a embed description.",
							fields: new[]
							{
								new DiscordMessageEmbedField("Field 1 name", "Field 1 value"),
								new DiscordMessageEmbedField("Field 2 name", "Field 2 value")
							},
							thumbnail: new DiscordMessageEmbedThumbnail("https://avatars3.githubusercontent.com/u/24236993?s=460&v=4"),
							image: new DiscordMessageEmbedImage("https://avatars3.githubusercontent.com/u/24236993?s=460&v=4"),
							footer: new DiscordMessageEmbedFooter("This is a embed footer text", "https://avatars3.githubusercontent.com/u/24236993?s=460&v=4")
						)
					}
				);
				// Send the message!
				client.SendToDiscord(message);
			}*/
        private static async void ServerMessageEveryHour()
        {
            switch (hours)
            {
                case >= 2:
                    await NwModule.Instance.SpeakString($"Server reset in {"hours".ColorString(ColorConstants.White)}".ColorString(ColorConstants.Purple), TalkVolume.Shout);
                    break;
                case 1:
                    await NwModule.Instance.SpeakString($"Server reset in {"hour".ColorString(ColorConstants.White)}".ColorString(ColorConstants.Purple), TalkVolume.Shout);
                    break;
                default:
                    NwModule.Instance.ExportAllCharacters();
                    Console.WriteLine($"Server reset.");
                    NwServer.Instance.ShutdownServer();
                    break;
            }
        }
    }
}
