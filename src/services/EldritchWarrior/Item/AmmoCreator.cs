using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace EldritchWarrior.Item
{
	[ServiceBinding(typeof(IItemHandler))]
	public class AmmoCreator : IItemHandler
	{
		public string Tag => "itm_ammo_creator";

		public void HandleActivateItem(ModuleEvents.OnActivateItem item)
		{
			if (item.TargetObject is NwItem itemTarget)
			{
				switch (itemTarget.BaseItemType)
				{
					case BaseItemType.Armor:
					case BaseItemType.Bolt:
					case BaseItemType.Bullet:
						itemTarget.StackSize = 99;
						itemTarget.PlotFlag = true;
						break;
					case BaseItemType.Dart:
					case BaseItemType.Shuriken:
					case BaseItemType.ThrowingAxe:
						itemTarget.StackSize = 50;
						itemTarget.PlotFlag = true;
						break;
					case BaseItemType.Grenade:
						itemTarget.StackSize = 10;
						itemTarget.PlotFlag = true;
						break;
					default:
						NwCreature activator = item.ItemActivator;
						NwPlayer player = activator.LoginPlayer;
						player.DisplayFloatingTextStringOnCreature(activator, "The target must be Ammunition type item!");
						break;
				}
			}
		}
	}
}
