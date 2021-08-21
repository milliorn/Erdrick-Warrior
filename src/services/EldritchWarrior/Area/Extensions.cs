using System.Linq;
//using NLog;
using Anvil.API;
using Anvil.API.Events;

namespace Services.Area
{
    public static class Extensions
    {
        public static void CleanupCreatures(this AreaEvents.OnExit obj)
        {
            foreach (NwGameObject trash in obj.Area.Objects.Where(t => t is NwCreature))
            {
                if (trash is NwCreature creature)
                {
                    foreach (NwItem item in creature.Inventory.Items)
                    {
                        item.Destroy();
                    }
                }
                trash.Destroy();
            }
        }

        public static void CleanupItems(this AreaEvents.OnExit obj)
        {
            foreach (NwGameObject trash in obj.Area.Objects.Where(t => t is NwItem))
            {
                if (trash is NwCreature creature)
                {
                    foreach (NwItem item in creature.Inventory.Items)
                    {
                        item.Destroy();
                    }
                }
                trash.Destroy();
            }
        }

        public static void CleanupPlaceables(this AreaEvents.OnExit obj)
        {
            foreach (var item in obj.Area.FindObjectsOfTypeInArea<NwPlaceable>().Where(i => i.HasInventory))
            {
                while (item.Inventory.Items.Any())
                {
                    item.Destroy();
                }
            }
        }

        public static async void CloseDoors(this AreaEvents.OnExit obj)
        {
            foreach (var door in obj.Area.FindObjectsOfTypeInArea<NwDoor>().Where(d => d.IsOpen))
            {
                await door.Close();
            }
        }
    }
}