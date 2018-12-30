using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace Server
{
    public class GameManager : BaseScript
    {
        public static int CountPlayMap = 0;
        
        public GameManager()
        {
            Tick += LobbyTimer;
            Tick += BriefingTimer;
            Tick += GameTimer;
        }

        public static void StopGame(List<Player> inGameList, int winnerType)
        {
            CountPlayMap++;
            if (CountPlayMap > 2)
                VoteMapTimer();
                        
            Sync.Data.Reset(-1, "GameTimer");
            foreach (var p in inGameList)
            {
                var playerId = User.GetPlayerServerId(p);
                p.TriggerEvent("TTT:StopGame", winnerType, CountPlayMap > 2);
                Sync.Data.Reset(playerId, "InGame");   
                Sync.Data.Reset(playerId, "isDead");
                Sync.Data.Reset(playerId, "playerType");
            }
        }

        private static async void VoteMapTimer()
        {
            Debug.WriteLine("START VOTE");
            
            await Delay(30000);

            Debug.WriteLine("CHECK VOTE");
            
            var votes = new Dictionary<string, int>();
            foreach (var p in new PlayerList())
            {
                if (!Sync.Data.Has(User.GetPlayerServerId(p), "voteMap")) continue;
                string map = (string) Sync.Data.Get(User.GetPlayerServerId(p), "voteMap");
                
                Debug.WriteLine("VOTE " + map);
                
                if (votes.ContainsKey(map))
                    votes[map]++;
                else
                    votes.Add(map, 1);
            }
            
            MapManager.SetMap(votes.FirstOrDefault(x => x.Value == votes.Values.Max()).Key);
            Debug.WriteLine("END VOTE");
        }

        private static async Task BriefingTimer()
        {
            await Delay(1000);

            var inGameList = new PlayerList().Where(p => Sync.Data.Has(User.GetPlayerServerId(p), "InGame")).ToList();
            if (inGameList.Any())
            {
                if (Sync.Data.Has(-1, "BriefingTimer") && (int) Sync.Data.Get(-1, "BriefingTimer") > 0)
                {
                    Sync.Data.Set(-1, "BriefingTimer", (int) Sync.Data.Get(-1, "BriefingTimer") - 1);

                    foreach (var p in inGameList)
                        p.TriggerEvent("TTT:UpdateBriefingInfo", (int) Sync.Data.Get(-1, "BriefingTimer"));
                    
                    if ((int) Sync.Data.Get(-1, "BriefingTimer") < 1)
                    {
                        int traitor1 = -1;
                        int traitor2 = -1;
                        int traitor3 = -1;
                        int traitor4 = -1;
                        int detective1 = -1;
                        int detective2 = -1;
                            
                        var rand = new Random();
    
                        traitor1 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                            
                        if (inGameList.Count > 7 && inGameList.Count <= 10)
                        {
                            traitor2 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                            //detective1 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                        }
                        else if (inGameList.Count > 10 && inGameList.Count <= 16) 
                        {
                            traitor2 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                            traitor3 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                            //detective1 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                        }
                        else if (inGameList.Count > 16)
                        {
                            traitor2 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                            traitor3 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                            traitor4 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                            //detective1 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                            //detective2 = User.GetPlayerServerId(inGameList[rand.Next(inGameList.Count)]);
                        }
                        
                        Sync.Data.Set(-1, "GameTimer", MapManager.MapData.PlayTime);
                        Sync.Data.Reset(-1, "BriefingTimer");
                        
                        foreach (var p in inGameList)
                            p.TriggerEvent("TTT:StartGame", traitor1, traitor2, traitor3, traitor4, detective1, detective2, MapManager.MapData.PlayTime, (int) Sync.Data.Get(-1, "CameraType"));
                        
                    }
                }
            }
            else if (Sync.Data.Has(-1, "BriefingTimer"))
                Sync.Data.Reset(-1, "BriefingTimer");
        }

        private static async Task GameTimer()
        {
            await Delay(1000);
            
            var inGameList = new PlayerList().Where(p => Sync.Data.Has(User.GetPlayerServerId(p), "InGame")).ToList();
            if (inGameList.Any())
            {
                if (Sync.Data.Has(-1, "GameTimer") && (int) Sync.Data.Get(-1, "GameTimer") > 0)
                {
                    if (MapManager.MapData.PlayTime == (int) Sync.Data.Get(-1, "GameTimer"))
                        await Delay(5000);

                    Sync.Data.Set(-1, "GameTimer", (int) Sync.Data.Get(-1, "GameTimer") - 1);

                    int innocentAliveCount = 0;
                    int traitorAliveCount = 0;

                    foreach (var p in inGameList)
                    {
                        p.TriggerEvent("TTT:UpdateGameInfo", (int) Sync.Data.Get(-1, "GameTimer"));

                        var playerId = User.GetPlayerServerId(p);
                        
                        if (!Sync.Data.Has(playerId, "playerType")) continue;

                        if ((int) Sync.Data.Get(playerId, "playerType") == PlayerTypes.Innocent ||
                            (int) Sync.Data.Get(playerId, "playerType") == PlayerTypes.Detective)
                        {
                            if (!Sync.Data.Has(playerId, "isDead"))
                                innocentAliveCount++;
                        }
                        else if ((int) Sync.Data.Get(playerId, "playerType") == PlayerTypes.Traitor)
                        {
                            if (!Sync.Data.Has(playerId, "isDead"))
                                traitorAliveCount++;
                        }
                    }

                    if (traitorAliveCount == 0)
                        StopGame(inGameList, PlayerTypes.Innocent);
                    else if (innocentAliveCount == 0)
                        StopGame(inGameList, PlayerTypes.Traitor);
                    else if ((int) Sync.Data.Get(-1, "GameTimer") < 1)
                        StopGame(inGameList, PlayerTypes.Innocent);
                }
            }
            else if (Sync.Data.Has(-1, "GameTimer"))
                Sync.Data.Reset(-1, "GameTimer");
        }

        private static async Task LobbyTimer()
        {
            await Delay(1000);

            var lobbyList = new PlayerList().Where(p => Sync.Data.Has(User.GetPlayerServerId(p), "Lobby")).ToList();

            if (lobbyList.Any())
            {
                if (Sync.Data.Has(-1, "StartTimer") && (int) Sync.Data.Get(-1, "StartTimer") > 0)
                {
                    Sync.Data.Set(-1, "StartTimer", (int) Sync.Data.Get(-1, "StartTimer") - 1);

                    int voteCam1 = 0;
                    int voteCam2 = 0;

                    foreach (var player in new PlayerList())
                    {
                        if (!Sync.Data.Has(User.GetPlayerServerId(player), "CameraType")) continue;
                        if ((int) Sync.Data.Get(User.GetPlayerServerId(player), "CameraType") == 0)
                            voteCam1++;
                        else
                            voteCam2++;
                    }
                    

                    foreach (var p in lobbyList)
                        p.TriggerEvent("TTT:UpdateLobbyInfo", (int) Sync.Data.Get(-1, "StartTimer"), voteCam1, voteCam2, lobbyList.Count);

                    if ((int) Sync.Data.Get(-1, "StartTimer") > 15)
                        if (lobbyList.Count == voteCam1 + voteCam2)
                            Sync.Data.Set(-1, "StartTimer", 10);

                    if ((int) Sync.Data.Get(-1, "StartTimer") < 1)
                    {
                        Sync.Data.Reset(-1, "StartTimer");
                        Sync.Data.Set(-1, "CameraType", voteCam1 > voteCam2 ? 0 : 1);

                        if (await MapManager.LoadMap(MapManager.GetMap(), lobbyList))
                        {
                            var weatherList = new List<string>
                            {
                                "EXTRASUNNY",
                                "CLEAR",
                                "CLOUDS",
                                "SMOG",
                                "FOGGY",
                                "OVERCAST",
                                "CLEARING",
                                "XMAS"
                            };

                            var rand = new Random();

                            string weatherMap = weatherList[rand.Next(weatherList.Count)];
                            int hourMap = rand.Next(23);

                            Sync.Data.Set(-1, "BriefingTimer", MapManager.MapData.Briefing);
                            foreach (var p in lobbyList)
                            {
                                Sync.Data.Set(User.GetPlayerServerId(p), "InGame", true);
                                p.TriggerEvent("TTT:StartBriefing", MapManager.MapData.Name,
                                    MapManager.MapData.Briefing, MapManager.MapData.CenterX,
                                    MapManager.MapData.CenterY, MapManager.MapData.CenterZ,
                                    MapManager.MapData.MaxRadius, MapManager.MapData.CanSwim, MapManager.MapData.EnableScope, weatherMap, hourMap);
                            }
                        }
                        else
                        {
                            MapManager.SetMap(MapManager.MapList.First());
                            StopGame(lobbyList, PlayerTypes.Unknown);
                        }

                        foreach (var player in new PlayerList())
                        {
                            Sync.Data.Reset(User.GetPlayerServerId(player), "CameraType");
                            Sync.Data.Reset(User.GetPlayerServerId(player), "Lobby");
                        }
                    }
                }
            }
            else if (Sync.Data.Has(-1, "StartTimer"))
                Sync.Data.Reset(-1, "StartTimer");
        }
    }
}