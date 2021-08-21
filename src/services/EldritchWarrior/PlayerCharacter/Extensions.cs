using System;
using Anvil.API;


namespace Services.PlayerCharacter
{
    public static class Extensions
    {

        public static void PlayerHasStabilized(this NwPlayer pc)
        {
            pc.ControlledCreature.PlayVoiceChat(VoiceChatType.GuardMe);
            pc.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.ImpHealingS));
            pc.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.Resurrection());
            pc.SendServerMessage($"{pc.ControlledCreature.Name} has stabilized.");
        }

        public static void PlayerHasDied(this NwPlayer pc)
        {
            pc.ControlledCreature.PlayVoiceChat(VoiceChatType.Death);
            pc.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.VisualEffect(VfxType.ImpDeath));
            pc.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.Death());
        }

        public static void ScreamOnDying(this NwPlayer pc)
        {
            pc.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.Damage(1));
            Random random = new();
            switch (random.Next(1, 6))
            {
                case 1: pc.ControlledCreature.PlayVoiceChat(VoiceChatType.Cuss); break;
                case 2: pc.ControlledCreature.PlayVoiceChat(VoiceChatType.NearDeath); break;
                case 3: pc.ControlledCreature.PlayVoiceChat(VoiceChatType.Pain1); break;
                case 4: pc.ControlledCreature.PlayVoiceChat(VoiceChatType.Pain2); break;
                case 5: pc.ControlledCreature.PlayVoiceChat(VoiceChatType.Pain3); break;
            }
        }
    }
}