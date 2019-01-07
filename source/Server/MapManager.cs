using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Server
{
    public class MapManager : BaseScript
    {
        public static List<string> MapList = new List<string>();
        public static MapData MapData = new MapData();
        
        public MapManager()
        {
            Tick += FindMapTimer;
            
            if (Directory.GetDirectories(@"resources/").ToList().All(item => item != $"resources/{GetCurrentResourceName()}"))
            {
                Debug.WriteLine("=======================================================================================");
                Debug.WriteLine("=====================================[ERROR]===========================================");
                Debug.WriteLine("=======================================================================================");
                Debug.WriteLine($"{GetCurrentResourceName()} not found. Please put your resource in /resources/");
                Debug.WriteLine("=======================================================================================");
                Debug.WriteLine("=======================================================================================");
                StopResource(GetCurrentResourceName());
                return;
            }

            FindMaps();
            SetMap(MapList.First());
        }

        public static void FindMaps()
        {
            MapList.Clear();
            foreach (string dir in Directory.GetDirectories($@"resources/{GetCurrentResourceName()}/maps/"))
                MapList.Add(dir.Replace($"resources/{GetCurrentResourceName()}/maps/", ""));
            TriggerClientEvent("TTT:LoadMapList", MapList);

            foreach (var item in MapList)
                Debug.WriteLine(item);
        }

        public static void SetMap(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (MapList.Count > 3)
                {
                    var rand = new Random();
                    SetMap(MapList[rand.Next(MapList.Count - 1)]);
                    return;
                }
                SetMap(MapList.First());
                return;
            }
            SetMapName(name);
            Sync.Data.Set(-1, "MapName", name);
            GameManager.CountPlayMap = 0;
            TriggerClientEvent("TTT:NewMapInfo", name);
        }

        public static string GetMap()
        {
            return (string) Sync.Data.Get(-1, "MapName");
        }

        public static async Task<bool> LoadMap(string mapName, List<Player> lobbyList)
        {
            if (MapList.All(item => item != mapName))
            {
                Debug.WriteLine($"Map \"{mapName}\" not found");
                SetMap(MapList.First());
                return false;
            }
            
            SetMapName(mapName);
            
            var fileList = new DirectoryInfo($@"resources/{GetCurrentResourceName()}/maps/{mapName}/").GetFiles("*.json", SearchOption.AllDirectories);

            bool isFindConfig = false;
            bool isFindWeapon = false;
            bool isFindSpawn = false;
            bool isFindIpl = false;

            foreach (var item in fileList)
            {
                if (item.Name == "config.json")
                    isFindConfig = true;
                if (item.Name == "weapon.json")
                    isFindWeapon = true;
                if (item.Name == "spawn.json")
                    isFindSpawn = true;
                if (item.Name == "ipl.json")
                    isFindIpl = true;
            }

            if (!isFindConfig)
            {
                Debug.WriteLine("Config is not found");
                return false;
            }
            
            MapData.Name = mapName;
            MapData.Briefing = 30;
            MapData.MaxPlayers = 32;
            MapData.CenterX = 0;
            MapData.CenterY = 0;
            MapData.CenterZ = 0;
            MapData.MaxRadius = 100;
            MapData.CanSwim = true;
            MapData.EnableScope = false;
            MapData.PlayTime = 300;
            
            foreach (var item in Main.LoadJson($@"resources/{GetCurrentResourceName()}/maps/{mapName}/config.json"))
            {
                if (item.Key == "breefing")
                    MapData.Briefing = Convert.ToInt32(item.Value);
                if (item.Key == "maxPlayers")
                    MapData.MaxPlayers = Convert.ToInt32(item.Value);
                if (item.Key == "centerX")
                    MapData.CenterX = Convert.ToDouble(item.Value);
                if (item.Key == "centerY")
                    MapData.CenterY = Convert.ToDouble(item.Value);
                if (item.Key == "centerZ")
                    MapData.CenterZ = Convert.ToDouble(item.Value);
                if (item.Key == "maxRadius")
                    MapData.MaxRadius = Convert.ToDouble(item.Value);
                if (item.Key == "playTime")
                    MapData.PlayTime = Convert.ToInt32(item.Value);
                if (item.Key == "canSwiming")
                    MapData.CanSwim = (bool) item.Value;
                if (item.Key == "enableScope")
                    MapData.EnableScope = (bool) item.Value;
            }

            if (isFindWeapon)
            {
                foreach (var p in lobbyList)
                    p.TriggerEvent("TTT:LoadWeapon", Main.LoadJson($@"resources/{GetCurrentResourceName()}/maps/{mapName}/weapon.json"));
            }

            if (isFindIpl)
            {
                foreach (var p in lobbyList)
                    p.TriggerEvent("TTT:LoadIpl", Main.LoadJson($@"resources/{GetCurrentResourceName()}/maps/{mapName}/ipl.json"));
            }

            if (isFindSpawn)
            {
                var spawnList = Main.LoadJson($@"resources/{GetCurrentResourceName()}/maps/{mapName}/spawn.json");

                int countPlayerSpawn = 0;
                while (countPlayerSpawn < lobbyList.Count)
                {
                    foreach (var item in spawnList)
                    {
                        if (countPlayerSpawn >= lobbyList.Count) continue;
                        
                        var playerId = User.GetPlayerServerId(lobbyList[countPlayerSpawn]);
                        var posString = item.Value.ToString().Split(',');               
                        if (posString.Length != 4) continue;
                        
                        //TODO Windows version
                        posString[0] = posString[0].Trim();
                        posString[0] = posString[0].Substring(0, posString[0].IndexOf('.'));

                        posString[1] = posString[1].Trim();
                        posString[1] = posString[1].Substring(0, posString[1].IndexOf('.'));

                        posString[2] = posString[2].Trim();
                        posString[2] = posString[2].Substring(0, posString[2].IndexOf('.'));

                        posString[3] = posString[3].Trim();
                        posString[3] = posString[3].Substring(0, posString[3].IndexOf('.'));
                        
                        Sync.Data.Set(playerId, "spawnPosX", Convert.ToInt32(posString[0]));
                        Sync.Data.Set(playerId, "spawnPosY", Convert.ToInt32(posString[1]));
                        Sync.Data.Set(playerId, "spawnPosZ", Convert.ToInt32(posString[2]));
                        Sync.Data.Set(playerId, "spawnRot", Convert.ToInt32(posString[3]));
                        
                        //TODO Linux version
                        /*Sync.Data.Set(playerId, "spawnPosX", Convert.ToDouble(posString[0]));
                        Sync.Data.Set(playerId, "spawnPosY", Convert.ToDouble(posString[1]));
                        Sync.Data.Set(playerId, "spawnPosZ", Convert.ToDouble(posString[2]));
                        Sync.Data.Set(playerId, "spawnRot", Convert.ToDouble(posString[3]));*/
                        countPlayerSpawn++;
                    }
                }
            }
            else
            {
                foreach (var p in lobbyList)
                {
                    var playerId = User.GetPlayerServerId(p);
                    Sync.Data.Set(playerId, "spawnPosX", MapData.CenterX);
                    Sync.Data.Set(playerId, "spawnPosY", MapData.CenterY);
                    Sync.Data.Set(playerId, "spawnPosZ", MapData.CenterZ);
                    Sync.Data.Set(playerId, "spawnRot", 0);                   
                }
            }

            return true;
        }

        private static async Task FindMapTimer()
        {
            await Delay(60000);
            FindMaps();
        }
    }
}

public class MapData
{
    public string Name { get; set; }
    public int Briefing { get; set; }
    public int MaxPlayers { get; set; }
    public double CenterX { get; set; }
    public double CenterY { get; set; }
    public double CenterZ { get; set; }
    public double MaxRadius { get; set; }
    public bool CanSwim { get; set; }
    public bool EnableScope { get; set; }
    public int PlayTime { get; set; }
}