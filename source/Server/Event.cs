using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace Server
{
    public class Event : BaseScript
    {
        public Event()
        {
            EventHandlers.Add("TTT:SendLog", new Action<string, string>(Main.SendLog));
            
            EventHandlers.Add("TTT:KickPlayer", new Action<Player, string>(User.Kick));
            EventHandlers.Add("TTT:KickByPlayer", new Action<Player, int, string>(KickByPlayer));
            EventHandlers.Add("TTT:PlayerFinishLoad", new Action<Player>(Main.PlayerFinishLoad));
            
            EventHandlers.Add("playerConnecting", new Action<Player, string, CallbackDelegate, IDictionary<string, object>>(OnPlayerConnecting));
            EventHandlers.Add("playerDropped", new Action<Player, string, CallbackDelegate>(OnPlayerDropped));
        }
        
        protected static void KickByPlayer([FromSource]Player player, int playerId, string reason)
        {
            foreach (var p in new PlayerList())
            {
                 if (User.GetPlayerServerId(p) == playerId)
                     User.Kick(p, reason);
            }
        }
        
        protected static void OnPlayerConnecting([FromSource]Player player, string playerName, CallbackDelegate kickCallback, IDictionary<string, object> deferrals)
        {
            Debug.WriteLine($"Connect: {player.Name} [{player.EndPoint}]");
            
            if (!User.DoesAccountExist(player))
                User.CreatePlayerAccount(player);
        }
    
        protected static void OnPlayerDropped([FromSource]Player player, string playerName, CallbackDelegate kickReason)
        {
            Debug.WriteLine($"Disconnect: {player.Name} [{player.EndPoint}]. Reason: {kickReason}");
            User.SaveAccount(player);
        }
    }
}