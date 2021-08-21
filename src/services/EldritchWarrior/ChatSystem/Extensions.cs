using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Anvil.API;
using Anvil.API.Events;
using NLog.Fluent;


namespace Services.ChatSystem
{
    public static class Extensions
    {
        private static readonly char playerWildcard = '!';
        private static readonly string notReady = "Feature not implemented.";

        public static int SetPortrait(this NwPlayer player, string[] chatArray) => int.TryParse(chatArray[1], out int n) ? (player.ControlledCreature.PortraitId = n) : 0;
        public static string SetVoice(this ModuleEvents.OnPlayerChat chat, string[] chatArray) => int.TryParse(chatArray[1], out _) ? (notReady) : chat.Message;
        public static bool TriggerChatTools(this ModuleEvents.OnPlayerChat chat) => chat.Message.StartsWith(playerWildcard);

        public static StringBuilder Roster(this NwPlayer player)
        {
            int playerCount = 0;
            int dmCount = 0;
            StringBuilder stringBuilder = new("Players Online.\n".ColorString(ColorConstants.Pink));
            
            foreach (NwPlayer pc in NwModule.Instance.Players)
            {
                if (pc.IsDM)
                {
                    dmCount++;
                }
                else
                {
                    playerCount++;
                    stringBuilder.Append($"{pc.ControlledCreature.Name.ColorString(ColorConstants.Pink)} | {pc}\n".ColorString(ColorConstants.White));
                }
            }

            stringBuilder.Append($"Player Online | {playerCount.ToString().ColorString(ColorConstants.White)}\n".ColorString(ColorConstants.Pink));
            stringBuilder.Append($"DM Online | {dmCount.ToString().ColorString(ColorConstants.White)}\n".ColorString(ColorConstants.Pink));
            stringBuilder.Append($"Total Online | {(playerCount + dmCount).ToString().ColorString(ColorConstants.White)}\n".ColorString(ColorConstants.Pink));

            player.SendServerMessage(stringBuilder.ToString());
            return stringBuilder;
        }

        public static void SetArmBone(this NwPlayer player)
        {
            player.ControlledCreature.SetCreatureBodyPart(CreaturePart.LeftBicep, CreatureModelNumber.Undead);
            player.ControlledCreature.SetCreatureBodyPart(CreaturePart.LeftForearm, CreatureModelNumber.Undead);
            player.ControlledCreature.SetCreatureBodyPart(CreaturePart.LeftHand, CreatureModelNumber.Undead);
        }

        public static void SetArmNormal(this NwPlayer player)
        {
            player.ControlledCreature.SetCreatureBodyPart(CreaturePart.LeftBicep, CreatureModelNumber.Skin);
            player.ControlledCreature.SetCreatureBodyPart(CreaturePart.LeftForearm, CreatureModelNumber.Skin);
            player.ControlledCreature.SetCreatureBodyPart(CreaturePart.LeftHand, CreatureModelNumber.Skin);
        }

        public static CreatureTailType SetTail(this NwPlayer player, string[] chatArray)
        {
            return (chatArray[1]) switch
            {
                "bone" => player.ControlledCreature.TailType = CreatureTailType.Bone,
                "devil" => player.ControlledCreature.TailType = CreatureTailType.Devil,
                "lizard" => player.ControlledCreature.TailType = CreatureTailType.Lizard,
                _ => player.ControlledCreature.TailType = CreatureTailType.None,
            };
        }

        public static CreatureWingType SetWings(this NwPlayer player, string[] chatArray)
        {
            return (chatArray[1]) switch
            {
                "angel" => player.ControlledCreature.WingType = CreatureWingType.Angel,
                "bat" => player.ControlledCreature.WingType = CreatureWingType.Bat,
                "bird" => player.ControlledCreature.WingType = CreatureWingType.Bird,
                "butterfly" => player.ControlledCreature.WingType = CreatureWingType.Butterfly,
                "demon" => player.ControlledCreature.WingType = CreatureWingType.Demon,
                "dragon" => player.ControlledCreature.WingType = CreatureWingType.Dragon,
                _ => player.ControlledCreature.WingType = CreatureWingType.None,
            };
        }

        public static NwPlayer SetAlignment(this NwPlayer player, string[] chatArray)
        {
            switch (chatArray[1])
            {
                case "chaotic":
                    player.ControlledCreature.LawChaosValue = 0;
                    break;
                case "evil":
                    player.ControlledCreature.GoodEvilValue = 0;
                    break;
                case "good":
                    player.ControlledCreature.GoodEvilValue = 100;
                    break;
                case "lawful":
                    player.ControlledCreature.LawChaosValue = 100;
                    break;
                case "neutral":
                    if (chatArray[2].Equals("1"))
                    {
                        player.ControlledCreature.LawChaosValue = 50; break;
                    }
                    else if (chatArray[2].Equals("2"))
                    {
                        player.ControlledCreature.GoodEvilValue = 50; break;
                    }
                    else
                    {

                    }
                    break;
                default:
                    break;
            }
            return player;
        }

        public static NwItem SetVisual(this NwPlayer player, string[] chatArray)
        {
            if (player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).IsValid && player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).ItemProperties.Any(x => x.PropertyType == ItemPropertyType.VisualEffect))
            {
                foreach (ItemProperty property in player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).ItemProperties.Where<ItemProperty>(x => x.PropertyType == ItemPropertyType.VisualEffect))
                {
                    player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).RemoveItemProperty(property);
                }
            }

            switch (chatArray[1])
            {
                case "acid": player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).AddItemProperty(ItemProperty.VisualEffect(ItemVisual.Acid), EffectDuration.Permanent); break;
                case "cold": player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).AddItemProperty(ItemProperty.VisualEffect(ItemVisual.Cold), EffectDuration.Permanent); break;
                case "electric": player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).AddItemProperty(ItemProperty.VisualEffect(ItemVisual.Electrical), EffectDuration.Permanent); break;
                case "evil": player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).AddItemProperty(ItemProperty.VisualEffect(ItemVisual.Evil), EffectDuration.Permanent); break;
                case "fire": player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).AddItemProperty(ItemProperty.VisualEffect(ItemVisual.Fire), EffectDuration.Permanent); break;
                case "holy": player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).AddItemProperty(ItemProperty.VisualEffect(ItemVisual.Holy), EffectDuration.Permanent); break;
                case "sonic": player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand).AddItemProperty(ItemProperty.VisualEffect(ItemVisual.Sonic), EffectDuration.Permanent); break;
                default: break;
            }

            return player.ControlledCreature.GetItemInSlot(InventorySlot.RightHand);
        }

        public static void SetEyes(this NwPlayer player, string[] chatArray)
        {
            VfxType eyes = VfxType.EyesCynTroglodyte;
            string color = chatArray[1];

            switch (player.ControlledCreature.RacialType)
            {
                case RacialType.Dwarf:
                    switch (color)
                    {
                        case "cyan": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesCynDwarfFemale : VfxType.EyesCynDwarfMale; break;
                        case "green": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesGreenDwarfFemale : VfxType.EyesGreenDwarfMale; break;
                        case "orange": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesOrgDwarfFemale : VfxType.EyesOrgDwarfMale; break;
                        case "purple": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesPurDwarfFemale : VfxType.EyesPurDwarfMale; break;
                        case "red": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesRedFlameDwarfFemale : VfxType.EyesRedFlameDwarfMale; break;
                        case "white": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesWhtDwarfFemale : VfxType.EyesWhtDwarfMale; break;
                        case "yellow": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesYelDwarfFemale : VfxType.EyesYelDwarfMale; break;
                    }
                    break;
                case RacialType.Elf:
                    switch (color)
                    {
                        case "cyan": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesCynElfFemale : VfxType.EyesCynElfMale; break;
                        case "green": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesGreenElfFemale : VfxType.EyesGreenElfMale; break;
                        case "orange": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesOrgElfFemale : VfxType.EyesOrgElfMale; break;
                        case "purple": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesPurElfFemale : VfxType.EyesPurElfMale; break;
                        case "red": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesRedFlameElfFemale : VfxType.EyesRedFlameElfMale; break;
                        case "white": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesWhtElfFemale : VfxType.EyesWhtHalflingMale; break;
                        case "yellow": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesYelElfFemale : VfxType.EyesYelElfMale; break;
                        default: break;
                    }
                    break;
                case RacialType.Gnome:
                    switch (color)
                    {
                        case "cyan": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesCynGnomeFemale : VfxType.EyesCynGnomeMale; break;
                        case "green": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesGreenGnomeFemale : VfxType.EyesGreenGnomeMale; break;
                        case "orange": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesOrgGnomeFemale : VfxType.EyesOrgGnomeMale; break;
                        case "purple": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesPurGnomeFemale : VfxType.EyesPurGnomeMale; break;
                        case "red": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesRedFlameGnomeFemale : VfxType.EyesRedFlameGnomeMale; break;
                        case "white": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesWhtGnomeFemale : VfxType.EyesWhtGnomeMale; break;
                        case "yellow": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesYelGnomeFemale : VfxType.EyesYelGnomeMale; break;
                        default: break;
                    }
                    break;
                case RacialType.Halfling:
                    switch (color)
                    {
                        case "cyan": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesCynHalflingFemale : VfxType.EyesCynHalflingMale; ; break;
                        case "green": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesGreenHalflingFemale : VfxType.EyesGreenHalflingMale; break;
                        case "orange": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesOrgHalflingFemale : VfxType.EyesOrgHalflingMale; break;
                        case "purple": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesPurHalflingFemale : VfxType.EyesPurHalflingMale; break;
                        case "red": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesRedFlameHalflingFemale : VfxType.EyesRedFlameHalflingMale; break;
                        case "white": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesWhtHalflingFemale : VfxType.EyesWhtHalflingMale; break;
                        case "yellow": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesYelHalflingFemale : VfxType.EyesYelHalflingMale; break;
                        default: break;
                    }
                    break;
                case RacialType.HalfOrc:
                    switch (color)
                    {
                        case "cyan": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesCynHalforcFemale : VfxType.EyesCynHalforcMale; break;
                        case "green": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesGreenHalforcFemale : VfxType.EyesGreenHalforcMale; break;
                        case "orange": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesOrgHalforcFemale : VfxType.EyesOrgHalforcMale; break;
                        case "purple": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesPurHalforcFemale : VfxType.EyesPurHalforcMale; break;
                        case "red": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesRedFlameHalforcFemale : VfxType.EyesRedFlameHalforcMale; break;
                        case "white": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesWhtHalforcFemale : VfxType.EyesWhtHalforcMale; break;
                        case "yellow": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesYelHalforcFemale : VfxType.EyesYelHalforcMale; break;
                        default: break;
                    }
                    break;
                case RacialType.Human:
                    switch (color)
                    {
                        case "cyan": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesCynHumanFemale : VfxType.EyesCynHumanMale; break;
                        case "green": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesGreenHumanFemale : VfxType.EyesGreenHumanMale; break;
                        case "orange": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesOrgHumanFemale : VfxType.EyesOrgHumanMale; break;
                        case "purple": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesPurHumanFemale : VfxType.EyesPurHumanMale; break;
                        case "red": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesRedFlameHumanFemale : VfxType.EyesRedFlameHumanMale; break;
                        case "white": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesWhtHumanFemale : VfxType.EyesWhtHumanMale; break;
                        case "yellow": eyes = player.ControlledCreature.Gender == Gender.Female ? VfxType.EyesYelHumanFemale : VfxType.EyesYelHumanMale; break;
                        default: break;
                    }
                    break;
                default:
                    switch (color)
                    {
                        case "cyan": eyes = VfxType.EyesCynTroglodyte; break;
                        case "green": eyes = VfxType.EyesCynTroglodyte; break;
                        case "orange": eyes = VfxType.EyesCynTroglodyte; break;
                        case "purple": eyes = VfxType.EyesCynTroglodyte; break;
                        case "red": eyes = VfxType.EyesCynTroglodyte; break;
                        case "white": eyes = VfxType.EyesCynTroglodyte; break;
                        case "yellow": eyes = VfxType.EyesCynTroglodyte; break;
                        default: break;
                    }
                    break;
            }

            player.ControlledCreature.ApplyEffect(EffectDuration.Permanent, Effect.VisualEffect(eyes));
        }

        public static void Emote(this NwPlayer player, string[] chatArray)
        {
            if (float.TryParse(chatArray[2].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out float animSpeed))
            {
                switch (chatArray[1])
                {
                    case "bow": player.ControlledCreature.PlayAnimation(Animation.FireForgetBow, animSpeed); break;
                    case "duck": player.ControlledCreature.PlayAnimation(Animation.FireForgetDodgeDuck, animSpeed); break;
                    case "dodge": player.ControlledCreature.PlayAnimation(Animation.FireForgetDodgeDuck, animSpeed); break;
                    case "drink": player.ControlledCreature.PlayAnimation(Animation.FireForgetDrink, animSpeed); break;
                    case "greet": player.ControlledCreature.PlayAnimation(Animation.FireForgetGreeting, animSpeed); break;
                    case "left": player.ControlledCreature.PlayAnimation(Animation.FireForgetHeadTurnLeft, animSpeed); break;
                    case "right": player.ControlledCreature.PlayAnimation(Animation.FireForgetHeadTurnRight, animSpeed); break;
                    case "bored": player.ControlledCreature.PlayAnimation(Animation.FireForgetPauseBored, animSpeed); break;
                    case "scratch": player.ControlledCreature.PlayAnimation(Animation.FireForgetPauseScratchHead, animSpeed); break;
                    case "read": player.ControlledCreature.PlayAnimation(Animation.FireForgetRead, animSpeed); break;
                    case "salute": player.ControlledCreature.PlayAnimation(Animation.FireForgetSalute, animSpeed); break;
                    case "spasm": player.ControlledCreature.PlayAnimation(Animation.FireForgetSpasm, animSpeed); break;
                    case "steal": player.ControlledCreature.PlayAnimation(Animation.FireForgetSteal, animSpeed); break;
                    case "taunt": player.ControlledCreature.PlayAnimation(Animation.FireForgetTaunt, animSpeed); break;
                    case "v1": player.ControlledCreature.PlayAnimation(Animation.FireForgetVictory1, animSpeed); break;
                    case "v2": player.ControlledCreature.PlayAnimation(Animation.FireForgetVictory2, animSpeed); break;
                    case "v3": player.ControlledCreature.PlayAnimation(Animation.FireForgetVictory3, animSpeed); break;
                    case "c1": player.ControlledCreature.PlayAnimation(Animation.LoopingConjure1, animSpeed); break;
                    case "c2": player.ControlledCreature.PlayAnimation(Animation.LoopingConjure2, animSpeed); break;
                    case "back": player.ControlledCreature.PlayAnimation(Animation.LoopingDeadBack, animSpeed); break;
                    case "front": player.ControlledCreature.PlayAnimation(Animation.LoopingDeadFront, animSpeed); break;
                    case "low": player.ControlledCreature.PlayAnimation(Animation.LoopingGetLow, animSpeed); break;
                    case "mid": player.ControlledCreature.PlayAnimation(Animation.LoopingGetMid, animSpeed); break;
                    case "listen": player.ControlledCreature.PlayAnimation(Animation.LoopingListen, animSpeed); break;
                    case "look": player.ControlledCreature.PlayAnimation(Animation.LoopingLookFar, animSpeed); break;
                    case "meditate": player.ControlledCreature.PlayAnimation(Animation.LoopingMeditate, animSpeed); break;
                    case "p1": player.ControlledCreature.PlayAnimation(Animation.FireForgetPauseBored, animSpeed); break;
                    case "p2": player.ControlledCreature.PlayAnimation(Animation.FireForgetPauseScratchHead, animSpeed); break;
                    case "drunk": player.ControlledCreature.PlayAnimation(Animation.LoopingPauseDrunk, animSpeed); break;
                    case "tired": player.ControlledCreature.PlayAnimation(Animation.LoopingPauseTired, animSpeed); break;
                    case "squat": player.ControlledCreature.PlayAnimation(Animation.LoopingSitChair, animSpeed); break;
                    case "sit": player.ControlledCreature.PlayAnimation(Animation.LoopingSitCross, animSpeed); break;
                    case "spasming": player.ControlledCreature.PlayAnimation(Animation.LoopingSpasm, animSpeed); break;
                    case "forceful": player.ControlledCreature.PlayAnimation(Animation.LoopingTalkForceful, animSpeed); break;
                    case "lol": player.ControlledCreature.PlayAnimation(Animation.LoopingTalkLaughing, animSpeed); break;
                    case "normal": player.ControlledCreature.PlayAnimation(Animation.LoopingTalkNormal, animSpeed); break;
                    case "beg": player.ControlledCreature.PlayAnimation(Animation.LoopingTalkPleading, animSpeed); break;
                    case "worship": player.ControlledCreature.PlayAnimation(Animation.LoopingWorship, animSpeed); break;
                    default: break;
                }
            }
        }

        public static int ResetLevel(this NwPlayer player, string[] chatArray)
        {
            int xp = player.ControlledCreature.Xp;

            if (chatArray[1].Equals("one"))
            {
                int hd = player.ControlledCreature.Level;
                player.ControlledCreature.Xp = (hd * (hd - 1) / 2 * 1000) - 1;
            }
            else if (chatArray[1].Equals("all"))
            {
                player.ControlledCreature.Xp = 0;
            }
            else
            {

            }

            player.SendServerMessage($"{player.ControlledCreature.Name.ColorString(ColorConstants.White)} has reset {chatArray[1]} {(chatArray[1].Equals("one") ? "level" : "levels")}.".ColorString(ColorConstants.Green));
            player.ExportCharacter();
            return player.ControlledCreature.Xp = xp;
        }

        public static void SetStatus(this NwPlayer player, string[] chatArray)
        {
            IEnumerable<NwPlayer> server = NwModule.Instance.Players;

            if (chatArray[1].Equals("like"))
            {
                foreach (NwPlayer pc in server)
                {
                    pc.SetPCReputation(true, player);
                }

            }
            else if (chatArray[1].Equals("dislike"))
            {
                foreach (NwPlayer pc in server)
                {
                    player.SetPCReputation(false, pc);
                }
            }
            else
            {

            }
        }

        public static string RollDice(this ModuleEvents.OnPlayerChat chat, string[] chatArray)
        {
            if (int.TryParse(chatArray[1], out int n))
            {
                Random random = new();
                int dice = random.Next(1, n);
                return chat.Message = $" rolled a d{n} and got {dice}.";
            }
            else
            {
                Log.Info(nameof(RollDice));
                return $"ERROR | RollDice{chat}{chatArray[1]}";
            }
        }

        public static void SetSkin(this NwPlayer player, string[] chatArray)
        {
            if (int.TryParse(chatArray[1], out int n))
            {
                player.ControlledCreature.SetColor(ColorChannel.Skin, n);
            }
        }

        public static void SetHair(this NwPlayer player, string[] chatArray)
        {
            if (int.TryParse(chatArray[1], out int n))
            {
                player.ControlledCreature.SetColor(ColorChannel.Hair, n);
            }
        }

        public static void SetTattooColor1(this NwPlayer player, string[] chatArray)
        {
            if (int.TryParse(chatArray[1], out int n))
            {
                player.ControlledCreature.SetColor(ColorChannel.Tattoo1, n);
            }
        }

        public static void SetTattooColor2(this NwPlayer player, string[] chatArray)
        {
            if (int.TryParse(chatArray[1], out int n))
            {
                player.ControlledCreature.SetColor(ColorChannel.Tattoo2, n);
            }
        }

        public static void SetHead(this NwPlayer player, string[] chatArray)
        {
            if (int.TryParse(chatArray[1], out int n))
            {
                player.ControlledCreature.SetCreatureBodyPart(CreaturePart.Head, n);
            }
        }
    }
}