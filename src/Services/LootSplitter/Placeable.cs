using System.Linq;
using NLog;
using Anvil.API;

using Anvil.API.Events;
using Anvil.Services;

namespace Services.LootSplitter
{
    [ServiceBinding(typeof(Placeable))]
    public class Placeable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [ScriptHandler("plc_sell_loot")]
        public void Disturbed(CallInfo callinfo)
        {
            //Process only objects added
            if (callinfo.TryGetEvent(out PlaceableEvents.OnDisturbed obj))
            {
                switch (obj.DisturbType)
                {
                    case InventoryDisturbType.Added:
                        DestroyItemForGold(obj);
                        break;
                    case InventoryDisturbType.Removed:
                    case InventoryDisturbType.Stolen:
                        break;
                }
            }
        }

        private static void DestroyItemForGold(PlaceableEvents.OnDisturbed obj)
        {
            var pc = obj.Disturber;

            if (obj.DisturbedItem.BaseItemType == BaseItemType.Gold)
            {
                GiveGoldEqually(pc.ControllingPlayer, obj.DisturbedItem.StackSize);
                CloneDestroy(obj);
            }
            else if (obj.DisturbedItem.HasInventory)
            {
                pc.ControllingPlayer.FloatingTextString($"{pc.Name} cannot sell inventory items {pc.Name.ColorString(ColorConstants.White)}!".ColorString(ColorConstants.Orange));
                CloneDestroy(obj);

            }
            else if (obj.DisturbedItem.PlotFlag)
            {
                pc.ControllingPlayer.FloatingTextString($"{pc.Name} cannot sell plot items {pc.Name.ColorString(ColorConstants.White)}!".ColorString(ColorConstants.Orange));
                CloneDestroy(obj);
            }
            else
            {
                int itemValue = obj.DisturbedItem.GoldValue / 10 > 0 ? obj.DisturbedItem.GoldValue / 10 : 1;
                pc.ControllingPlayer.FloatingTextString($"{pc.Name} sold {pc.Name.ColorString(ColorConstants.White)} for {itemValue}!".ColorString(ColorConstants.Green));
                GiveGoldEqually(pc.ControllingPlayer, itemValue);
            }
        }

        private static void GiveGoldEqually(NwPlayer pc, int itemValue)
        {
            int goldDivided = itemValue / pc.PartyMembers.Count<NwPlayer>();

            if (pc.PartyMembers.Count() == 1) return;

            pc.FloatingTextString($"{goldDivided.ToString().ColorString(ColorConstants.White)}gp given to each player after splitting {itemValue.ToString().ColorString(ColorConstants.White)} from {pc.ControlledCreature.Name.ColorString(ColorConstants.White)}.");
            foreach (NwPlayer player in pc.PartyMembers)
            {
                logger.Info(pc.ControlledCreature.Name);
                player.ControlledCreature.GiveGold(goldDivided, true);
            }
        }

        private static void CloneDestroy(PlaceableEvents.OnDisturbed obj) => obj.DisturbedItem.Clone(obj.Disturber).Destroy();
    }
}