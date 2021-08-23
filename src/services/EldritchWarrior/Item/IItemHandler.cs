using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace EldritchWarrior.Item
{
    public interface IItemHandler
    {
        public string Tag { get; }

        public void HandleActivateItem(ModuleEvents.OnActivateItem activateItem);
    }

    [ServiceBinding(typeof(ItemHandlerService))]
    public class ItemHandlerService
    {
        private readonly Dictionary<string, IItemHandler> itemHandlers = new Dictionary<string, IItemHandler>();

        public ItemHandlerService(IEnumerable<IItemHandler> handlers)
        {
            foreach (IItemHandler itemHandler in handlers)
            {
                itemHandlers[itemHandler.Tag] = itemHandler;
            }

            NwModule.Instance.OnActivateItem += OnActivateItem;
        }

        private void OnActivateItem(ModuleEvents.OnActivateItem activateItem)
        {
            if (itemHandlers.TryGetValue(activateItem.ActivatedItem.Tag, out var handler))
            {
                handler.HandleActivateItem(activateItem);
            }
        }
    }
}