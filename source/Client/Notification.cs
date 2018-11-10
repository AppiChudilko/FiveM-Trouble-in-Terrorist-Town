using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class Notification : BaseScript
    {
        public static readonly int TypeChatbox = 1;
        public static readonly int TypeEmail = 2;
        public static readonly int TypeAddFriendRequest = 3;
        public static readonly int TypeNothing = 3;
        public static readonly int TypeRightJumpingArrow = 7;
        public static readonly int TypeRpIcon = 8;
        public static readonly int TypeMoneyIcon = 9;
        
        public Notification()
        {
            EventHandlers.Add("TTT:SendPlayerNotification", new Action<string, bool, bool>(Send));
            EventHandlers.Add("TTT:SendPlayerNotificationPicture", new Action<string, string, string, string, int>(SendPicture));
            EventHandlers.Add("TTT:SendPlayerSubTitle", new Action<string, int, bool>(SendSubtitle));
        }

        public static void Send(string message, bool blink = true, bool saveToBrief = true)
        {
            SetNotificationTextEntry("THREESTRINGS");
            foreach (string msg in Main.StringToArray(message))
                if (msg != null)
                    AddTextComponentSubstringPlayerName(msg);
            DrawNotification(blink, saveToBrief);
        }
        
        public static void SendPicture(string text, string title, string subtitle, string icon, int type)
        {
            SetNotificationTextEntry("STRING");
            AddTextComponentString(text);
            SetNotificationMessage(icon, icon, true, type, title, subtitle);
            DrawNotification(false, true);
        }
        
        public static void SendSubtitle(string message, int duration = 5000, bool drawImmediately = true)
        {
            BeginTextCommandPrint("THREESTRINGS");
            foreach (var msg in Main.StringToArray(message))
                AddTextComponentSubstringPlayerName(msg);
            EndTextCommandPrint(duration, drawImmediately);
        }
    }
}