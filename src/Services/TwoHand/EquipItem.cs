using Anvil.API;
using Anvil.Services;

namespace Services.TwoHand
{
	[ServiceBinding(typeof(EquipItem))]
	public class EquipItem
	{
		public EquipItem()
		{
			NwModule.Instance.OnPlayerEquipItem += equipItem =>
			{
				if (equipItem.Item is NwItem && equipItem.Player.ControllingPlayer is NwPlayer player && (!player.ControlledCreature.CheckCreatureSize() || 
                    !player.ControlledCreature.HasShieldEquipped() && !player.ControlledCreature.CheckCreaturekSizeAndWeapon()))
				{
					player.ControlledCreature.AddBuff();
				}
			};
		}
	}
}
