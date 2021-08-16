using System.Linq;
using Anvil.API;

namespace Services.Item
{
    public static class Extensions
    {
        public static bool HasTemporaryItemProperty(this NwItem nwItem) => nwItem.ItemProperties.Any(x => x.DurationType == EffectDuration.Temporary);
        public static void NotifyLoot(this NwItem acquireItem) => SendLootMessageToParty(acquireItem, $"{acquireItem.Possessor.Name.ColorString(Color.PINK)} obtained {acquireItem.BaseItemType.ToString().ColorString(Color.WHITE)}.", 40);

        public static string PrintGPValueOnItem(this NwItem nwItem)
            => nwItem.PlotFlag
            ? nwItem.OriginalDescription
            : (nwItem.Description = $"{"Gold Piece Value:".ColorString(Color.YELLOW)}{nwItem.GoldValue.ToString().ColorString(Color.ORANGE)}\n\n{nwItem.OriginalDescription}");

        public static void RemoveAllTemporaryItemProperties(this NwItem nwItem)
        {
            foreach (NWN.API.ItemProperty property in nwItem.ItemProperties.Where(x => x.DurationType == EffectDuration.Temporary))
            {
                nwItem.RemoveItemProperty(property);
            }
        }

        public static void SendMessageToAllPartyWithinDistance(this NwPlayer nwPlayer, string message, float distance)
        {
            foreach (NwPlayer member in nwPlayer.PartyMembers.Where(member => member.ControlledCreature.Distance(nwPlayer) == distance))
            {
                member.SendServerMessage(message);
            }
        }

        public static void SendLootMessageToParty(this NwItem acquireItem, string message, float distance)
        {
            if (acquireItem.Possessor is NwPlayer player && !player.IsDM)
            {
                player.SendMessageToAllPartyWithinDistance(message, distance);
                player.SendServerMessage(message);
            }
        }

        /*
            If you are bartering items with another player and you have items in the temporary inventories
            that someone dropped to offer you and you logout and login you will dupe those items. This is part of the
            code to stop bartering exploits that are similar to the one above but it doesn't stop that exploit. Basically
            you just create an invalid state or force one to put an item in to dupe it.
        */
        public static void FixBarterExploit(this NwPlayer playerA, NwPlayer playerB)
        {
            Module.Extensions.SaveCharacter(playerA);
            Module.Extensions.SaveCharacter(playerB);
        }
    }
}