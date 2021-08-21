using NLog;

using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Services.ChatSystem
{
    [ServiceBinding(typeof(Chat))]
    public class Chat
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Chat() => NwModule.Instance.OnPlayerChat += chat =>
        {
            if (chat.TriggerChatTools())
            {
                chat.Message = chat.Message[1..];
                chat.Message = chat.Message.ToLower();
                ChatToolsRouter(chat, chat.Message.Split(' '));
            }
        };

        private static void ChatToolsRouter(ModuleEvents.OnPlayerChat chat, string[] chatArray)
        {
            switch (chatArray[0])
            {
                case "xp":
                    _ = int.TryParse(chatArray[1], out int x);
                    chat.Sender.ControlledCreature.Xp += x;
                    break;
                case "live":
                    chat.Sender.ControlledCreature.HP = chat.Sender.ControlledCreature.HP = chat.Sender.ControlledCreature.MaxHP;
                    break;
                case "dead":
                    chat.Sender.ControlledCreature.ApplyEffect(EffectDuration.Instant, Effect.Damage(1));
                    break;
                case "roster":
                    chat.Sender.Roster();
                    break;
                case "armbone":
                    chat.Sender.SetArmBone();
                    break;
                case "armskin":
                    chat.Sender.SetArmNormal();
                    break;
                case "!":
                    chat.Sender.Emote(chatArray);
                    break;
                case "head":
                    chat.Sender.SetHead(chatArray);
                    break;
                case "portrait":
                    chat.Sender.SetPortrait(chatArray);
                    break;
                case "voice":
                    chat.SetVoice(chatArray);
                    break;
                case "skin":
                    chat.Sender.SetSkin(chatArray);
                    break;
                case "hair":
                    chat.Sender.SetHair(chatArray);
                    break;
                case "tattoocolor1":
                    chat.Sender.SetTattooColor1(chatArray);
                    break;
                case "tattoocolor2":
                    chat.Sender.SetTattooColor2(chatArray);
                    break;
                case "tail":
                    chat.Sender.SetTail(chatArray);
                    break;
                case "wings":
                    chat.Sender.SetWings(chatArray);
                    break;
                case "alignment":
                    chat.Sender.SetAlignment(chatArray);
                    break;
                case "resetlevel":
                    chat.Sender.ResetLevel(chatArray);
                    break;
                case "roll":
                    chat.RollDice(chatArray);
                    break;
                case "status":
                    chat.Sender.SetStatus(chatArray);
                    break;
                case "eyes":
                    chat.Sender.SetEyes(chatArray);
                    break;
                case "visual":
                    chat.Sender.SetVisual(chatArray);
                    break;
                case "lfg":
                    NwModule.Instance.SpeakString($"{chat.Sender.ControlledCreature.Name.ColorString(ColorConstants.White)} is looking for a party!", TalkVolume.Shout);
                    break;
                case "save":
                    chat.Sender.ExportCharacter();
                    break;
                case "delete":
                    chat.Sender.Delete($"{chat.Sender.BicFileName}.bic has been deleted.");
                    break;
                default:
                    break;
            }
        }
    }
}
