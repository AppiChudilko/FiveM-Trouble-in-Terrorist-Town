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
        }
        
        public static dynamic[,] GetAllSkinList()
        {
            dynamic[,] skins =
            {
                { "a_m_y_beach_02", 50000 },
                { "a_m_y_beach_03", 50000 },
                { "a_m_y_beachvesp_01", 50000 },
                { "a_m_y_beachvesp_02", 50000 },
                { "a_m_m_bevhills_01", 50000 },
                { "a_m_y_bevhills_01", 50000 },
                { "a_m_m_bevhills_02", 50000 },
                { "a_m_y_bevhills_03", 50000 },
                { "a_m_y_breakdance_01", 50000 },
                { "a_m_y_beach_01", 50000 },
                { "s_m_y_dealer_01", 50000 },
                { "a_m_y_eastsa_02", 50000 },
                { "a_m_y_genstreet_01", 50000 },
                { "a_m_y_genstreet_02", 50000 },
                { "u_m_y_guido_01", 50000 },
                { "u_m_y_gunvend_01", 50000 },
                { "a_m_y_hipster_01", 50000 },
                { "a_m_y_indian_01", 50000 },
                { "g_m_y_korean_01", 50000 },
                { "g_m_y_korean_02", 50000 },
                { "a_m_y_ktown_01", 50000 },
                { "a_m_y_latino_01", 50000 },
                { "a_m_m_malibu_01", 50000 },
                { "a_m_m_mexlabor_01", 50000 },
                { "a_m_y_motox_02", 50000 },
                { "a_m_y_musclbeac_02", 50000 },
                { "a_m_m_paparazzi_01", 50000 },
                { "a_m_y_polynesian_01", 50000 },
                { "a_m_y_runner_01", 50000 },
                { "a_m_y_skater_01", 50000 },
                { "a_m_m_skater_01", 50000 },
                { "a_m_m_socenlat_01", 50000 },
                { "s_f_y_bartender_01", 50000 },
                { "a_f_m_bevhills_01", 50000 },
                { "a_f_y_bevhills_01", 50000 },
                { "a_f_y_bevhills_02", 50000 },
                { "a_f_y_bevhills_04", 50000 },
                { "a_f_y_yoga_01", 50000 },
                { "a_f_y_business_01", 50000 },
                { "a_f_m_business_02", 50000 },
                { "a_f_y_business_02", 50000 },
                { "a_f_y_business_04", 50000 },
                { "a_f_y_genhot_01", 50000 },
                { "a_f_y_golfer_01", 50000 },
                { "a_f_y_hippie_01", 50000 },
                { "a_f_y_hipster_01", 50000 },
                { "a_f_y_hipster_02", 50000 },
                { "a_f_y_hipster_03", 50000 },
                { "a_f_y_hipster_04", 50000 },
                { "s_f_y_hooker_01", 50000 },
                { "u_f_y_hotposh_01", 50000 },
                { "a_f_y_indian_01", 50000 },
                { "g_f_y_lost_01", 50000 },
                { "s_f_y_movprem_01", 50000 },
                { "a_f_y_runner_01", 50000 },
                { "a_f_y_rurmeth_01", 50000 },
                { "a_f_y_skater_01", 50000 },
                { "u_f_y_spyactress", 50000 },
                { "s_f_y_sweatshop_01", 50000 },
                { "a_f_y_tourist_01", 50000 },
                { "g_f_y_vagos_01", 50000 },
                { "a_f_y_vinewood_01", 50000 },
                { "a_f_y_vinewood_02", 50000 },
                { "a_f_y_vinewood_04", 50000 },
            };

            return skins;
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