using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class User : BaseScript
    {
        public static string GetPlayerLic(Player player)
        {
            return player.Identifiers["license"];
        }

        public static void Kick([FromSource] Player player, string reason)
        {
            player.Drop(reason);
        }

        public static void LoadAccount(Player player)
        {   
            foreach (DataRow row in Appi.MySql.ExecuteQueryWithResult("SELECT * FROM users WHERE lic = '" + GetPlayerLic(player) + "'").Rows)
            {
                var playerObj = new PlayerData()
                {
                    id = (int) row["id"],
                    lic = (string) row["lic"],
                    score = (int) row["score"],
                    skin = (string) row["skin"],
                    money = (int) row["money"],
                };

                var playerId = GetPlayerServerId(player);
                
                foreach (var property in typeof(PlayerData).GetProperties())
                {
                    Sync.Data.Reset(playerId, property.Name);
                    Sync.Data.Set(playerId, property.Name, property.GetValue(playerObj, null));
                }
                
                player.TriggerEvent("TTT:SpawnPlayer", playerObj.skin, 0, 0, 0, 0, true);
                return;
            }
        }

        public static void SaveAccount(Player player)
        {
            var id = GetPlayerServerId(player);
            if (!Sync.Data.Has(id, "id"))
                return;
            string sql = "UPDATE users SET";
            sql = sql + " score = '" + (int) Sync.Data.Get(id, "score") + "'";
            sql = sql + ", skin = '" + (string) Sync.Data.Get(id, "skin") + "'";
            sql = sql + ", money = '" + (int) Sync.Data.Get(id, "money") + "'";
            sql = sql + " where id = '" + (int) Sync.Data.Get(id, "id") + "'";
            Appi.MySql.ExecuteQuery(sql);
        }

        public static int GetPlayerServerId(Player player)
        {
            return Convert.ToInt32(player.Handle) <= 65535 ? Convert.ToInt32(player.Handle) : Convert.ToInt32(player.Handle) - 65535;
        }
        
        public static void CreatePlayerAccount(Player player)
        {
            Appi.MySql.ExecuteQuery("INSERT INTO users (lic, skin) VALUES ('" + GetPlayerLic(player) + "', '" + Main.GetRandomSkin() + "')");
        }

        public static bool DoesAccountExist(Player player)
        {
            return Appi.MySql.ExecuteQueryWithResult("SELECT * FROM users WHERE lic = '" + GetPlayerLic(player) + "'").Rows.Cast<DataRow>().Any();
        }
        
        public static void AddCashMoney(Player player, int money)
        {
            SetCashMoney(player, GetCashMoney(player) + money);
        }

        public static void RemoveCashMoney(Player player, int money)
        {
            SetCashMoney(player, GetCashMoney(player) - money);
        }

        public static void SetCashMoney(Player player, int money)
        {
            Sync.Data.Set(GetPlayerServerId(player), "money", money);
        }

        public static int GetCashMoney(Player player)
        {
            return Sync.Data.Get(GetPlayerServerId(player), "money");
        }
        
        public static void UpdateAllData(Player player)
        {
            player.TriggerEvent("TTT:GetPlayerAllData");
        }
    }
}

public class PlayerData
{
    public int id { get; set; }
    public string lic { get; set; }
    public int score { get; set; }
    public string skin { get; set; }
    public int money { get; set; }
}

public class PlayerTypes
{
    public static int Unknown => -1;
    public static int Innocent => 0;
    public static int Traitor => 1;
    public static int Detective => 2;
}