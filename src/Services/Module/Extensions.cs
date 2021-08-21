using System.Collections.Generic;
using Anvil.API;

namespace Services.Module
{
    public static class Extensions
    {
        /* List of DM Public Keys */
        public static readonly Dictionary<string, string> DMList = new()
        {
            { "QR4JFL9A", "milliorn" },
            { "QRMXQ6GM", "milliorn" },
        };

        public static void SaveCharacter(this NwPlayer player)
        {
            player.ExportCharacter();
            player.SendServerMessage($"{player.BicFileName.ColorString(ColorConstants.Green)}.bic file has been saved.".ColorString(ColorConstants.White));
        }
    }
}