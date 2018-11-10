using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace Client
{
    class MyEntity : Entity
    {
        public MyEntity(int handle) : base(handle)
        {
            
        }
    }
    
    public class Main : BaseScript
    {
        public Main()
        {
            User.Invisible(true);
            User.Freeze(true);
            NetworkSetVoiceChannel(999);
            NetworkSetFriendlyFireOption(true);
            SetCanAttackFriendly(GetPlayerPed(-1), true, true);
            TriggerServerEvent("TTT:PlayerFinishLoad");
            
            User.MapInfo.Weather = "EXTRASUNNY";
            User.MapInfo.Hour = 13;
            
            UI.ShowSimpleShard("TEST", "TEST222", 20000);
        }
        
        public static void SendLog(string fileName, string text)
        {
            TriggerServerEvent("TTT:SendLog", fileName, text);
        }
        
        public static string WeaponNameToNormalName(string name)
        {
            foreach (uint hash in Enum.GetValues(typeof(WeaponHash)))
            {
                if (hash == (uint) GetHashKey(name))
                    return Enum.GetName(typeof(WeaponHash), hash);
            }
            return name;
        }
        
        public static CitizenFX.Core.Player GetPlayerOnRadius(Vector3 pos, float radius)
        {
            return new PlayerList().Where(player => player.ServerId != User.GetServerId()).FirstOrDefault(player => GetDistance(pos, GetEntityCoords(GetPlayerPed(player.Handle), true)) < radius);
        }
        
        public static List<CitizenFX.Core.Player> GetPlayerListOnRadius(Vector3 pos, float radius)
        {
            return new PlayerList().Where(player => GetDistance(pos, GetEntityCoords(GetPlayerPed(player.Handle), true)) < radius).ToList();
        }
        
        public static async Task<int> GetGameStatus()
        {
            return (int) await Sync.Data.Get(-1, "GameStatus");
        }
        
        public static float GetDistance(Vector3 pos1, Vector3 pos2)
        {
            return (float) Math.Sqrt(pos1.DistanceToSquared(pos2));
        }

        public static float GetDistance2D(Vector3 pos1, Vector3 pos2)
        {
            return (float) Math.Sqrt(pos1.DistanceToSquared2D(pos2));
        }
        
        public static string[] StringToArray(string inputString)
        {
            string[] outputString = new string[3];

            var lastSpaceIndex = 0;
            var newStartIndex = 0;
            outputString[0] = inputString;

            if (inputString.Length <= 99) return outputString;
            
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString.Substring(i, 1) == " ")
                {
                    lastSpaceIndex = i;
                }

                if (inputString.Length > 99 && i >= 98)
                {
                    if (i == 98)
                    {
                        outputString[0] = inputString.Substring(0, lastSpaceIndex);
                        newStartIndex = lastSpaceIndex + 1;
                    }
                    if (i > 98 && i < 198)
                    {
                        if (i == 197)
                        {
                            outputString[1] = inputString.Substring(newStartIndex, (lastSpaceIndex - (outputString[0].Length - 1)) - (inputString.Length - 1 > 197 ? 1 : -1));
                            newStartIndex = lastSpaceIndex + 1;
                        }
                        else if (i == inputString.Length - 1 && inputString.Length < 198)
                        {
                            outputString[1] = inputString.Substring(newStartIndex, ((inputString.Length - 1) - outputString[0].Length));
                            newStartIndex = lastSpaceIndex + 1;
                        }
                    }
                        
                    if (i <= 197) continue;
                        
                    if (i == inputString.Length - 1 || i == 296)
                    {
                        outputString[2] = inputString.Substring(newStartIndex, ((inputString.Length - 1) - outputString[0].Length) - outputString[1].Length);
                    }
                }
            }

            return outputString;
        }
    }
}