using Anvil.API;

using Anvil.Services;
using NLog;

namespace Services.Module
{
    [ServiceBinding(typeof(MonkWeapons))]
    public class MonkWeapons
    {
        private WeaponService? WeaponService;
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void Init()
        {
            WeaponService.SetWeaponIsMonkWeapon(BaseItemType.Dart);
            WeaponService.SetWeaponIsMonkWeapon(BaseItemType.Handaxe);
            WeaponService.SetWeaponIsMonkWeapon(BaseItemType.LightHammer);
            WeaponService.SetWeaponIsMonkWeapon(BaseItemType.LightMace);
            WeaponService.SetWeaponIsMonkWeapon(BaseItemType.Sickle);
            WeaponService.SetWeaponIsMonkWeapon(BaseItemType.Shuriken);
            WeaponService.SetWeaponIsMonkWeapon(BaseItemType.Quarterstaff);

            WeaponService.SetWeaponUnarmed(BaseItemType.Dart);
            WeaponService.SetWeaponUnarmed(BaseItemType.Shuriken);
            WeaponService.SetWeaponUnarmed(BaseItemType.Grenade);
            WeaponService.SetWeaponUnarmed(BaseItemType.ThrowingAxe);
        }
    }
}
