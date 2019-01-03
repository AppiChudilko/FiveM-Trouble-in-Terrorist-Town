using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class Event : BaseScript
    {
        private static bool _isDead = false;
        private static bool _hasBeenDead = false;
        private static int _diedAt = 0;
        private static int _ped = -1;
        public static uint KillerWeapon = 0;
        
        public Event()
        {
            EventHandlers.Add("TTT:SpawnPlayer", new Action<string, float, float, float, float, bool>(User.SpawnEvent));
            EventHandlers.Add("TTT:GetPlayerAllData", new Action(User.GetAllDataEvent));
            EventHandlers.Add("TTT:UpdateLobbyInfo", new Action<int, int, int, int>(UpdateLobbyInfo));
            EventHandlers.Add("TTT:UpdateGameInfo", new Action<int>(UpdateGameInfo));
            EventHandlers.Add("TTT:UpdateBriefingInfo", new Action<int>(UpdateBriefingInfo));
            EventHandlers.Add("TTT:StartBriefing", new Action<string, int, double, double, double, double, bool, bool, string, int>(StartBriefing));
            EventHandlers.Add("TTT:StartGame", new Action<int, int, int, int, int, int, int, int>(StartGame));
            EventHandlers.Add("TTT:StopGame", new Action<int, bool>(StopGame));
            EventHandlers.Add("TTT:LoadMapList", new Action<dynamic>(LoadMapList));
            EventHandlers.Add("TTT:LoadWeapon", new Action<dynamic>(LoadWeapon));
            EventHandlers.Add("TTT:LoadIpl", new Action<dynamic>(LoadIpl));
            EventHandlers.Add("TTT:NewMapInfo", new Action<string>(NewMapInfo));
            
            Tick += SetTick;
        }

        public static void UpdateLobbyInfo(int timer, int cam1, int cam2, int countPlayers)
        {
            User.LobbyInfo.Active = true;
            User.LobbyInfo.Timer = timer;
            User.LobbyInfo.Cam1 = cam1;
            User.LobbyInfo.Cam2 = cam2;
            User.LobbyInfo.CountPlayers = countPlayers;
        }

        public static void UpdateBriefingInfo(int timer)
        {
            User.MapInfo.Briefing = timer;
        }

        public static void UpdateGameInfo(int timer)
        {
            User.GameInfo.Timer = timer;
        }

        public static void LoadWeapon(dynamic weapon)
        {
            User.WeaponList = weapon;
        }

        public static void LoadMapList(dynamic mapList)
        {
            User.MapList = mapList;
        }

        public static void LoadIpl(dynamic ipl)
        {
            foreach (var item in (IDictionary<String, Object>) ipl)
            {
                if (!(bool) item.Value)
                    RemoveIpl(item.Key);
                else if (!IsIplActive(item.Key))
                    RequestIpl(item.Key);
            }
        }

        public static void NewMapInfo(string name)
        {
            UI.ShowMissionPassedMessage($"~g~Next map: {name}", 10000);
            Notification.Send($"~g~Next map: {name}");
        }

        public static void StartBriefing(string name, int briefing, double x, double y, double z, double radius, bool canSwim, bool enableScope, string weather, int hour)
        {
            User.MapInfo.Name = name;
            User.MapInfo.Briefing = briefing;
            User.MapInfo.CenterX = x;
            User.MapInfo.CenterY = y;
            User.MapInfo.CenterZ = z;
            User.MapInfo.MaxRadius = radius;
            User.MapInfo.Weather = weather;
            User.MapInfo.Hour = hour;
            User.MapInfo.CanSwim = canSwim;
            User.MapInfo.EnableScope = enableScope;
            
            User.SetStatusType(StatusTypes.Briefing);
        }

        public static async void StartGame(int traitor1, int traitor2, int traitor3, int traitor4, int detective1, int detective2, int gameTimer, int camType)
        {
            User.SetStatusType(StatusTypes.InGame);

            await Delay(500);
            
            User.GameInfo.Active = true;
            User.GameInfo.CamType = camType;
            User.GameInfo.Timer = gameTimer;
            User.GameInfo.Traitor1 = traitor1;
            User.GameInfo.Traitor2 = traitor2;
            User.GameInfo.Traitor3 = traitor3;
            User.GameInfo.Traitor4 = traitor4;
            User.GameInfo.Detective1 = detective1;
            User.GameInfo.Detective2 = detective2;

            var playerId = User.GetServerId();
            if (playerId == User.GameInfo.Traitor1)
                User.GameInfo.PlayerType = PlayerTypes.Traitor;
            else if (playerId == User.GameInfo.Traitor2)
                User.GameInfo.PlayerType = PlayerTypes.Traitor;
            else if (playerId == User.GameInfo.Traitor3)
                User.GameInfo.PlayerType = PlayerTypes.Traitor;
            else if (playerId == User.GameInfo.Traitor4)
                User.GameInfo.PlayerType = PlayerTypes.Traitor;
            else if (playerId == User.GameInfo.Detective1)
                User.GameInfo.PlayerType = PlayerTypes.Detective;
            else if (playerId == User.GameInfo.Detective2)
                User.GameInfo.PlayerType = PlayerTypes.Detective;
            else
                User.GameInfo.PlayerType = PlayerTypes.Innocent;
            
            if (User.GameInfo.PlayerType == PlayerTypes.Traitor)
                MenuList.ShowWeaponShopMenu("Special");
            
            Sync.Data.Set(User.GetServerId(), "playerType", User.GameInfo.PlayerType);
        }

        public static async void StopGame(int winner, bool isVoteMap)
        {
            if (PlayerTypes.Innocent == winner)
            {
                if (User.GameInfo.PlayerType == PlayerTypes.Innocent || User.GameInfo.PlayerType == PlayerTypes.Detective)
                    User.AddCashMoney(5000);
                UI.ShowMissionPassedMessage("~g~Innocent win", 20000);
                Notification.Send("~g~Innocent win");
            }
            else if (PlayerTypes.Traitor == winner)
            {
                if (User.GameInfo.PlayerType == PlayerTypes.Traitor)
                    User.AddCashMoney(5000);
                UI.ShowMissionPassedMessage("~r~Traitor win", 20000);
                Notification.Send("~r~Traitor win");
            }

            await Delay(10000);

            if (isVoteMap)
            {
                MenuList.ShowVoteMapMenu();
                await Delay(30000);
                MenuList.HideMenu();
                await Delay(5000);
            }
            
            User.SetStatusType(StatusTypes.Lobby);
        }
        
        private static async Task SetTick()
        {
            var player = PlayerId();

            if (NetworkIsPlayerActive(player))
            {
                var ped = PlayerPedId();
                
                if (IsPedFatallyInjured(ped) && !_isDead)
                {
                    _isDead = true;

                    if (!_isDead)
                        _diedAt = GetGameTimer();

                    var killer = NetworkGetEntityKillerOfPlayer(player, ref KillerWeapon);
                    var killerEntityType = GetEntityType(killer);
                    var killerType = -1;
                    var killerVehicleName = "";
                    var killerId = GetPlayerByEntityId(killer);

                    if (killerEntityType == 1)
                    {
                        killerType = GetPedType(killer);

                        if (IsPedInAnyVehicle(killer, false))
                            killerVehicleName = GetDisplayNameFromVehicleModel((uint) GetEntityModel(GetVehiclePedIsUsing(killer)));
                    }

                    if (killer != ped && killerId != -1 && NetworkIsPlayerActive(killerId))
                        killerId = GetPlayerServerId(killerId);

                    if (killer == ped || killer == -1 || killerId == -1 || killerType == -1)
                    {
                        /*var pos = GetEntityCoords(ped, true);
                        TriggerEvent("ARP:OnPlayerDeath", killerType, pos.X, pos.Y, pos.Z);*/
                        _hasBeenDead = true;
                    }
                    else
                    {
                        var pos = GetEntityCoords(ped, true);
                        var killerPos = GetEntityCoords(GetPlayerPed(killerId), true);
                        TriggerEvent("ARP:OnPlayerKiller", killerType, killerId, KillerWeapon, killerVehicleName, pos.X, pos.Y, pos.Z, killerPos.X, killerPos.Y, killerPos.Z);

                        if (User.GetStatusType() == StatusTypes.Briefing)
                            if (User.MapInfo.Briefing > 15)
                            {
                                TriggerServerEvent("TTT:KickByPlayer", killerId, "RDM");
                                User.SetStatusType(StatusTypes.Briefing);
                            }

                        if (User.GetStatusType() == StatusTypes.InGame)
                        {
                            if ((int) await Sync.Data.Get(User.GetServerId(), "playerType") ==
                                (int) await Sync.Data.Get(killerId, "playerType"))
                            {
                                if (await Sync.Data.Has(killerId, "playerWarning"))
                                {
                                    int count = (int) await Sync.Data.Get(killerId, "playerType") + 1;
                                    Sync.Data.Set(killerId, "playerWarning", count);
                                    if (count > 3)
                                        TriggerServerEvent("TTT:KickByPlayer", killerId, "RDM");
                                }
                                else
                                    Sync.Data.Set(killerId, "playerWarning", 1);
                            }
                        }
                            
                        
                        Client.Sync.Data.Set(User.GetServerId(), "deathReason", KillerWeapon);
                        _hasBeenDead = true;
                    }
                }
                else if (IsPedFatallyInjured(ped))
                {
                    _isDead = false;
                    _diedAt = -1;
                }

                if (!_hasBeenDead && _diedAt > 0)
                {
                    /*var pos = GetEntityCoords(ped, true);
                    TriggerEvent("ARP:OnPlayerWasted", pos.X, pos.Y, pos.Z);*/
                    _hasBeenDead = true;
                }
                else if (_hasBeenDead && _diedAt <= 0)
                    _hasBeenDead = false;
            }
        }

        public static int GetPlayerByEntityId(int id)
        {
            for (int i = 0; i <= 32; i++)
            {
                if (NetworkIsPlayerActive(i) && GetPlayerPed(i) == id)
                    return i;
            }
            return -1;
        }
    }
}