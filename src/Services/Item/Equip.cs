using Anvil.API;
using Anvil.Services;

namespace Services.Item
{
    [ServiceBinding(typeof(Equip))]
    public class Equip
    {
        //private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public Equip()
        {
            NwModule.Instance.OnPlayerEquipItem += equip =>
            {
                if (equip.Player.ControllingPlayer is NwPlayer nwPlayer)
                {
                    equip.Item.PrintGPValueOnItem();
                    Module.Extensions.SaveCharacter(nwPlayer);
                }
            };
        }
    }
}