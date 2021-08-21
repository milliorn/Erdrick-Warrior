//using NLog;
using Anvil.API;

using Anvil.API.Events;
using Anvil.Services;
using System;
using System.Threading.Tasks;

namespace Services.PlayerCharacter
{
    [ServiceBinding(typeof(LevelUp))]

    public class LevelUp
    {
        public LevelUp() => NwModule.Instance.OnPlayerLevelUp += async levelUp =>
        {
            await OnLevelUp(levelUp);
        };

        private static async Task OnLevelUp(ModuleEvents.OnPlayerLevelUp levelUp)
        {
            Module.Extensions.SaveCharacter(levelUp.Player);
            levelUp.Player.ControlledCreature.ForceRest();
            await NwTask.Delay(TimeSpan.FromSeconds(0.0));
            levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.ImpLightningS));

            if (levelUp.Player.ControlledCreature.Level % 5 == 0)
            {
                await NwTask.Delay(TimeSpan.FromSeconds(0.0));
                levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfWailOBanshees));
            }

            switch (levelUp.Player.ControlledCreature.GoodEvilAlignment)
            {
                case Alignment.Evil:
                    await NwTask.Delay(TimeSpan.FromSeconds(0.2));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosEvil10));
                    await NwTask.Delay(TimeSpan.FromSeconds(0.4));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosEvil20));
                    await NwTask.Delay(TimeSpan.FromSeconds(0.6));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosEvil30));
                    break;
                case Alignment.Good:
                    await NwTask.Delay(TimeSpan.FromSeconds(0.2));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosHoly10));
                    await NwTask.Delay(TimeSpan.FromSeconds(0.4));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosHoly20));
                    await NwTask.Delay(TimeSpan.FromSeconds(0.6));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosHoly30));
                    break;
                default:
                    await NwTask.Delay(TimeSpan.FromSeconds(0.2));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosNormal10));
                    await NwTask.Delay(TimeSpan.FromSeconds(0.4));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosNormal20));
                    await NwTask.Delay(TimeSpan.FromSeconds(0.6));
                    levelUp.Player.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosNormal30));
                    break;
            }
        }
    }
}
