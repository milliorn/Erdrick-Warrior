//using NLog;
using NWN.API;
using NWN.API.Constants;
using NWN.Services;
using System;

namespace Services.PlayerCharacter
{
    [ServiceBinding(typeof(LevelUp))]

    public class LevelUp
    {
        public LevelUp() => NwModule.Instance.OnPlayerLevelUp += async levelUp =>
            {
                Module.Extensions.SaveCharacter(levelUp.Player);
                levelUp.Player.ForceRest();
                await NwTask.Delay(TimeSpan.FromSeconds(0.0));
                levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.ImpLightningS));

                if (levelUp.Player.Level % 5 == 0)
                {
                    await NwTask.Delay(TimeSpan.FromSeconds(0.0));
                    levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfWailOBanshees));
                }

                switch (levelUp.Player.GoodEvilAlignment)
                {
                    case Alignment.Evil:
                        await NwTask.Delay(TimeSpan.FromSeconds(0.2));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosEvil10));
                        await NwTask.Delay(TimeSpan.FromSeconds(0.4));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosEvil20));
                        await NwTask.Delay(TimeSpan.FromSeconds(0.6));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosEvil30));
                        break;
                    case Alignment.Good:
                        await NwTask.Delay(TimeSpan.FromSeconds(0.2));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosHoly10));
                        await NwTask.Delay(TimeSpan.FromSeconds(0.4));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosHoly20));
                        await NwTask.Delay(TimeSpan.FromSeconds(0.6));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosHoly30));
                        break;
                    default:
                        await NwTask.Delay(TimeSpan.FromSeconds(0.2));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosNormal10));
                        await NwTask.Delay(TimeSpan.FromSeconds(0.4));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosNormal20));
                        await NwTask.Delay(TimeSpan.FromSeconds(0.6));
                        levelUp.Player.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.FnfLosNormal30));
                        break;
                }
            };
    }
}
