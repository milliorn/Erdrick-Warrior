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
            var pc = (NwPlayer)obj.Disturber;

            if (obj.DisturbedItem.BaseItemType == BaseItemType.Gold)
            {
                GiveGoldEqually(pc, obj.DisturbedItem.StackSize);
                CloneDestroy(obj, pc);
            }
            else if (obj.DisturbedItem.HasInventory)
            {
                pc.FloatingTextString($"{pc.ControlledCreature.Name} cannot sell inventory items {pc.ControlledCreature.Name.ColorString(Color.WHITE)}!".ColorString(Color.ORANGE));
                CloneDestroy(obj, pc);

            }
            else if (obj.DisturbedItem.PlotFlag)
            {
                pc.FloatingTextString($"{pc.ControlledCreature.Name} cannot sell plot items {pc.ControlledCreature.Name.ColorString(Color.WHITE)}!".ColorString(Color.ORANGE));
                CloneDestroy(obj, pc);
            }
            else
            {
                int itemValue = obj.DisturbedItem.GoldValue / 10 > 0 ? obj.DisturbedItem.GoldValue / 10 : 1;
                pc.FloatingTextString($"{pc.ControlledCreature.Name} sold {pc.ControlledCreature.Name.ColorString(Color.WHITE)} for {itemValue}!".ColorString(Color.GREEN));
                GiveGoldEqually(pc, itemValue);
            }
        }

        private static void GiveGoldEqually(NwPlayer pc, int itemValue)
        {
            int goldDivided = itemValue / pc.PartyMembers.Count<NwPlayer>();

            if (pc.PartyMembers.Count() == 1) return;

            pc.FloatingTextString($"{goldDivided.ToString().ColorString(Color.WHITE)}gp given to each player after splitting {itemValue.ToString().ColorString(Color.WHITE)} from {pc.ControlledCreature.Name.ColorString(Color.WHITE)}.");
            foreach (NwPlayer player in pc.PartyMembers)
            {
                logger.Info(pc.ControlledCreature.Name);
                player.ControlledCreature.GiveGold(goldDivided, true);
            }
        }

        private static void CloneDestroy(PlaceableEvents.OnDisturbed obj, NwPlayer pc) => obj.DisturbedItem.Clone(pc).Destroy();
    }
}