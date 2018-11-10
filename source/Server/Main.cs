using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class Main : BaseScript
    {
        public static dynamic FromJson(string json)
        {   
            return JObject.Parse(json);
        }

        public static string ToJson(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
        
        public static IDictionary<String, Object> LoadJson(string file)
        {
            using (StreamReader r = new StreamReader(file))
            {
                return JsonConvert.DeserializeObject<IDictionary<String, Object>>(r.ReadToEnd());
            }
        }
        
        public static string Sha256(string randomString)
        {
            string hash = String.Empty;
            byte[] crypto = new SHA256Managed().ComputeHash(Encoding.ASCII.GetBytes(randomString));
            return crypto.Aggregate(hash, (current, theByte) => current + theByte.ToString("x2"));
        }
        
        public static async void PlayerFinishLoad([FromSource] Player player)
        {
            if (User.DoesAccountExist(player))
            {
                User.LoadAccount(player);
                return;
            }
            
            User.CreatePlayerAccount(player);

            while (!User.DoesAccountExist(player))
                await Delay(1000);
            
            User.LoadAccount(player);
        }
        
        public static string DeleteSqlQuote(string text)
        {
            return text.Replace("'", "");
        }
        
        public static dynamic[,] GetAllSkinList()
        {
            dynamic[,] skins =
            {
                { "a_m_y_beach_02", 1000 },
                { "a_m_y_beach_03", 1000 },
                { "a_m_y_beachvesp_01", 1000 },
                { "a_m_y_beachvesp_02", 1000 },
                { "a_m_m_bevhills_01", 1000 },
                { "a_m_y_bevhills_01", 1000 },
                { "a_m_m_bevhills_02", 1000 },
                { "a_m_y_bevhills_03", 1000 },
                { "a_m_y_breakdance_01", 1000 },
                { "a_m_y_beach_01", 1000 },
                { "s_m_y_dealer_01", 1000 },
                { "a_m_y_eastsa_02", 1000 },
                { "a_m_y_genstreet_01", 1000 },
                { "a_m_y_genstreet_02", 1000 },
                { "u_m_y_guido_01", 1000 },
                { "u_m_y_gunvend_01", 1000 },
                { "a_m_y_hipster_01", 1000 },
                { "a_m_y_indian_01", 1000 },
                { "g_m_y_korean_01", 1000 },
                { "g_m_y_korean_02", 1000 },
                { "a_m_y_ktown_01", 1000 },
                { "a_m_y_latino_01", 1000 },
                { "a_m_m_malibu_01", 1000 },
                { "a_m_m_mexlabor_01", 1000 },
                { "a_m_y_motox_02", 1000 },
                { "a_m_y_musclbeac_02", 1000 },
                { "a_m_m_paparazzi_01", 1000 },
                { "a_m_y_polynesian_01", 1000 },
                { "a_m_y_runner_01", 1000 },
                { "a_m_y_skater_01", 1000 },
                { "a_m_m_skater_01", 1000 },
                { "a_m_m_socenlat_01", 1000 },
                { "s_f_y_bartender_01", 1000 },
                { "a_f_m_bevhills_01", 1000 },
                { "a_f_y_bevhills_01", 1000 },
                { "a_f_y_bevhills_02", 1000 },
                { "a_f_y_bevhills_04", 1000 },
                { "a_f_y_yoga_01", 1000 },
                { "a_f_y_business_01", 1000 },
                { "a_f_m_business_02", 1000 },
                { "a_f_y_business_02", 1000 },
                { "a_f_y_business_04", 1000 },
                { "a_f_y_genhot_01", 1000 },
                { "a_f_y_golfer_01", 1000 },
                { "a_f_y_hippie_01", 1000 },
                { "a_f_y_hipster_01", 1000 },
                { "a_f_y_hipster_02", 1000 },
                { "a_f_y_hipster_03", 1000 },
                { "a_f_y_hipster_04", 1000 },
                { "s_f_y_hooker_01", 1000 },
                { "u_f_y_hotposh_01", 1000 },
                { "a_f_y_indian_01", 1000 },
                { "g_f_y_lost_01", 1000 },
                { "s_f_y_movprem_01", 1000 },
                { "a_f_y_runner_01", 1000 },
                { "a_f_y_rurmeth_01", 1000 },
                { "a_f_y_skater_01", 1000 },
                { "u_f_y_spyactress", 1000 },
                { "s_f_y_sweatshop_01", 1000 },
                { "a_f_y_tourist_01", 1000 },
                { "g_f_y_vagos_01", 1000 },
                { "a_f_y_vinewood_01", 1000 },
                { "a_f_y_vinewood_02", 1000 },
                { "a_f_y_vinewood_04", 1000 },
            };

            return skins;
        }
        
        public static string GetRandomSkin()
        {
            dynamic[,] skins = GetAllSkinList();
            return (string) skins[new Random().Next(skins.Length / 2), 0];
        }
        
        public static void SendLog(string filename, string log)
        {
            try
            {
                File.AppendAllText($"{filename}.log", $"[{DateTime.Now:dd/MM/yyyy}] [{DateTime.Now:HH:mm:ss tt}] {log}\n");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"ServerLog TRY-CATCH {log} | {e}");
            }
        }
    }
}