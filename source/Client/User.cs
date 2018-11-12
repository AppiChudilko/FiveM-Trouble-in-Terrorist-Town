using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class User : BaseScript
    {
        public static Vector3 WaitLobby = new Vector3(223.0697f, -586.3705f, 168.8611f);
        public static bool IsBlockAnimation = false;
        public static int Hour = 13;
        public static PlayerData Data = new PlayerData();
        public static LobbyInfo LobbyInfo = new LobbyInfo();
        public static GameInfo GameInfo = new GameInfo();
        public static MapInfo MapInfo = new MapInfo();
        
        public static dynamic WeaponList = null;
        public static dynamic MapList = null;
        
        public static double[,] SpawnList =
        {
            { -338.7884, -579.6178, 48.09489 },
            { -293.0222, -632.1178, 47.43132 },
            { -269.2281, -962.775, 143.5142 },
            { 98.88757, -870.8663, 136.9165 },
            { 580.1769, 89.59447, 117.3308 },
            { 423.6479, 15.56825, 151.9242 },
            { 424.9219, 18.586, 151.931 },
            { 551.9955, -28.19887, 93.86244 },
            { 305.863, -284.8494, 68.29829 },
            { 299.488, -313.9493, 68.29829 },
            { 1240.899, -1090.095, 44.35722 },
            { -418.4464, -2804.495, 14.80695 },
            { 802.3354, -2996.213, 27.36875 },
            { 548.3521, -2219.756, 67.94666 },
            { -701.2187, 58.91474, 68.68575 },
            { -696.7746, 208.6952, 139.7731 },
            { -769.8155, 255.006, 134.7385 },
            { -1918.884, -3028.625, 22.61429 },
            { -1039.817, -2385.444, 27.40255 },
            { -1590.373, -3212.547, 28.6604 },
            { -1311.997, -2624.589, 36.11582 },
            { -991.5846, -2774.019, 48.31227 },
            { -556.7017, -119.8519, 50.98835 },
            { -619.0831, -106.5815, 51.01202 },
            { -1152.408, -443.9738, 42.89137 },
            { -1156.081, -498.8079, 49.32043 },
            { -1290.007, -445.2428, 106.4711 },
            { -770.0829, -786.3356, 83.82861 },
            { -824.3132, -719.1805, 120.2517 },
            { -598.8342, -917.809, 35.84408 },
            { -678.5171, -717.0078, 54.09795 },
            { -669.458, -804.2544, 31.8844 },
            { -1463.988, -526.1229, 83.58365 },
            { -1525.904, -596.7999, 66.52119 },
            { -1375.134, -465.2585, 83.51427 },
            { -1711.984, 478.334, 127.1892 },
            { 202.6934, 1204, 230.2588 },
            { 217.0646, 1140.443, 230.2588 },
            { 668.7827, 590.3213, 136.9934 },
            { 722.2471, 562.2682, 134.2943 },
            { 838.1705, 510.1091, 138.6649 },
            { 773.1747, 575.3554, 138.4155 },
            { 735.4507, 231.9995, 145.1368 },
            { 450.932, 5566.451, 795.442 },
            { -449.0599, 6019.923, 35.56564 },
            { -142.5559, 6286.784, 39.26382 },
            { -368.0471, 6105.006, 38.42902 },
            { 2796.773, 5992.872, 354.989 },
            { 3460.883, 3653.532, 51.16711 },
            { 3614.592, 3636.562, 51.16711 },
            { -2180.794, 3252.703, 54.3309 },
            { -2124.381, 3219.853, 54.3309 },
            { -2050.939, 3178.414, 54.3309 },
            { 1858.295, 3694.042, 37.91168 },
            { 1695.486, 3614.863, 37.79684 },
            { 1692.829, 2532.073, 60.33785 },
            { 1692.829, 2647.942, 60.33785 },
            { 1824.353, 2574.386, 60.56225 },
            { 1407.908, 2117.489, 104.1011 },
            { -214.6158, -744.6461, 219.4428 },
            { -166.7245, -590.6718, 199.0783 },
            { 124.2959, -654.8749, 261.8616 },
            { 149.2771, -769.0092, 261.8616 },
            { 253.297, -3145.925, 39.40688 },
            { 207.652, -3145.925, 39.41451 },
            { 207.652, -3307.397, 39.51926 },
            { 247.3365, -3307.397, 39.52404 },
            { 484.2856, -2178.582, 40.25116 },
            { -150.321, -150.2459, 96.1528 },
            { -202.9684, -327.1913, 65.04893 },
            { -1913.77, -3031.85, 22.58777 },
            { -1042.578, -2390.227, 27.40255 },
            { -1583.461, -3216.81, 28.63388 },
            { -1308.23, -2626.368, 36.0893 },
            { -984.6726, -2778.282, 48.28575 },
            { -1167.27, -575.0267, 40.19548 },
            { -928.5076, -383.1334, 135.2698 },
            { -902.8115, -443.0529, 170.8185 },
            { -2311.601, 335.4441, 187.6049 },
            { -2214.416, 342.206, 198.1012 },
            { -2234.355, 187.0235, 193.6015 },
            { 2792.246, 5996.045, 355.1923 },
            { 3459.178, 3659.834, 51.19159 },
            { 3615.938, 3642.95, 51.19159 },
        };
        
        
        private static bool _spawnLock = false;
        private static int _lastSpec = -1;
        private static string _currentScenario = "";
        private static int _statusType = 0;
        
        public static void SpawnEvent(string skin, float x, float y, float z, float heading, bool checkIsLogin = false)
        {
            Spawn(skin, x, y, z, heading, checkIsLogin);
        }
        
        public static async void Spawn(string skin, float x, float y, float z, float heading, bool checkIsLogin = false)
        {
            if (_spawnLock)
                return;

            _spawnLock = true;

            await UI.ShowLoadDisplay();
            
            uint spawnModel = (uint) GetHashKey(skin);
            
            RequestModel(spawnModel);
            while (!HasModelLoaded(spawnModel))
            {
                RequestModel(spawnModel);
                await Delay(1);
            }

            SetPlayerModel(PlayerId(), spawnModel);
            SetModelAsNoLongerNeeded(spawnModel);
            RequestCollisionAtCoord(x, y, z);

            var ped = GetPlayerPed(-1);
            SetEntityCoordsNoOffset(ped, x, y, z, false, false, false);
            NetworkResurrectLocalPlayer(x, y, z, heading, true, true);
            ClearPedTasksImmediately(ped);
            RemoveAllPedWeapons(ped, false);
            ClearPlayerWantedLevel(PlayerId());
            
            while (!HasCollisionLoadedAroundEntity(ped))
                await Delay(1);
            
            if(GetIsLoadingScreenActive())
                ShutdownLoadingScreen();

            if (checkIsLogin)
            {
                await GetAllData();
                while (!IsLogin()) {
                    await GetAllData();
                    await Delay(500);
                }
                
                SetStatusType(StatusTypes.MainMenu);
                _spawnLock = false;
                return;
            }
            
            Freeze(false);
            Invisible(false);

            await UI.HideLoadDisplay();
            _spawnLock = false;
        }
        
        public static async void Respawn(Vector3 pos, float rot, bool isFreeze = true, bool isShowDisplay = true)
        {
            RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
            
            if (isShowDisplay)
                await UI.ShowLoadDisplay();
            
            if (isFreeze)
                Freeze(true);

            SetEntityCoords(GetPlayerPed(-1), pos.X, pos.Y, pos.Z, true, false, false, true);
            NetworkResurrectLocalPlayer(pos.X, pos.Y, pos.Z, rot, true, false);
            
            Sync.Data.Set(GetServerId(), "deathReason", -1);
            Sync.Data.Reset(GetServerId(), "isDead");
            
            PlayScenario("forcestop");
            ClearPedBloodDamage(GetPlayerPed(-1));
            StopAllScreenEffects();
            RemoveAllPedWeapons(GetPlayerPed(-1), false);
            IsBlockAnimation = false;

            await Delay(500);

            if (isFreeze)
                Freeze(false);
            
            await Delay(500);
            
            if (isShowDisplay)
                await UI.HideLoadDisplay();
        }
        
        public static async void GetAllDataEvent()
        {
            await GetAllData();
        }

        public static async Task<bool> GetAllData()
        {
            try
            {
                dynamic data = await Sync.Data.GetAll(GetServerId(), 1000);    
                if (data == null) return false;
            
                var localData = (IDictionary<String, Object>) data;
                foreach (var property in typeof(PlayerData).GetProperties())
                    property.SetValue(Data, localData[property.Name], null);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Ex{e}");
                throw;
            }
            return false;
        }
        
        public static int GetServerId()
        {
            return GetPlayerServerId(PlayerId());
        }
        
        public static void Freeze(bool freeze)
        {
            int playerId = PlayerId();
            var ped = GetPlayerPed(playerId);
            
            SetPlayerControl(playerId, !freeze, 0);
            if (!freeze)
                FreezeEntityPosition(ped, false);
            else
            {
                FreezeEntityPosition(ped, true);
                
                if (IsPedFatallyInjured(ped))
                    ClearPedTasksImmediately(ped);
            }
        }
        
        public static void Invisible(bool invisible)
        {
            int playerId = PlayerId();
            var ped = GetPlayerPed(playerId);
            
            if (!invisible)
            {
                if (!IsEntityVisible(ped))
                    SetEntityVisible(ped, true, false);
                
                if (!IsPedInAnyVehicle(ped, true))
                    SetEntityCollision(ped, true, true);
        
                SetPlayerInvincible(playerId, false);
            } 
            else 
            {
                if (IsEntityVisible(ped))
                    SetEntityVisible(ped, false, false);
        
                SetEntityCollision(ped, false, true);
                SetPlayerInvincible(playerId, true);
            }
        }
        
        public static void PlayScenario(string scenarioName)
        {
            if (IsBlockAnimation) return;
            
            if (_currentScenario == "" || _currentScenario != scenarioName)
            {
                _currentScenario = scenarioName;
                ClearPedTasks(PlayerPedId());

                if (IsPedInAnyVehicle(PlayerPedId(), true)) return;
                if (IsPedRunning(PlayerPedId())) return;
                if (IsEntityDead(PlayerPedId())) return;
                if (IsPlayerInCutscene(PlayerPedId())) return;
                if (IsPedFalling(PlayerPedId())) return;
                if (IsPedRagdoll(PlayerPedId())) return;
                if (!IsPedOnFoot(PlayerPedId())) return;
                if (NetworkIsInSpectatorMode()) return;
                if (GetEntitySpeed(PlayerPedId()) > 5.0f) return;
                TaskStartScenarioInPlace(PlayerPedId(), scenarioName, 0, true);
            }
            else
            {
                _currentScenario = "";
                ClearPedTasks(PlayerPedId());
                ClearPedSecondaryTask(PlayerPedId());
            }

            if (scenarioName != "forcestop") return;
            _currentScenario = "";
            ClearPedTasks(PlayerPedId());
        }

        public static void StopScenario()
        {
            ClearPedTasks(PlayerPedId());
        }
        
        public static bool IsInMainMenu()
        {
            return GetStatusType() == 0;
        }
        
        public static bool IsInShop()
        {
            return GetStatusType() == 1;
        }
        
        public static bool IsInLobby()
        {
            return GetStatusType() == 2;
        }
        
        public static bool IsInGame()
        {
            return GetStatusType() == 3;
        }
        
        public static bool IsInSpec()
        {
            return NetworkIsInSpectatorMode();
        }
        
        public static bool IsLogin()
        {
            return Data.id > 0;
        }
        
        public static async void Teleport(Vector3 pos)
        {
            RequestCollisionAtCoord(pos.X, pos.Y, pos.Z);
            await UI.ShowLoadDisplay();
            
            NetworkFadeOutEntity(GetPlayerPed(-1), true, true);
            Freeze(true);
            
            SetEntityCoords(GetPlayerPed(-1), pos.X, pos.Y, pos.Z, true, false, false, true);
            //NetworkResurrectLocalPlayer(pos.X, pos.Y, pos.Z, 0, true, false);

            await Delay(500);

            Freeze(false);
            
            await Delay(500);
            NetworkFadeInEntity(GetPlayerPed(-1), false);
            
            await UI.HideLoadDisplay();
        }
        
        public static void SetStatusType(int status)
        {
            StopSpec();
            MenuList.HideMenu();
            
            _statusType = status;
            SetRichPresence(GetStatusTypeLabel());
            Sync.Data.Set(GetServerId(), "StatusType", _statusType);
            
            switch (_statusType)
            {
                case 0:
                    SwitchToMainMenu();
                    break;
                case 1:
                    SwitchToLobby();
                    break;
                case 2:
                    SwitchToLobby();
                    break;
                case 3:
                    SwitchToGame();
                    break;
                case 4:
                    SwitchToSpec();
                    break;
                case 5:
                    SwitchToBriefing();
                    break;
            }
            
            ResetValues();
        }
        
        public static int GetStatusType()
        {
            return _statusType;
        }
        
        public static string GetStatusTypeLabel()
        {
            switch (_statusType)
            {
                case 0:
                    return "Main menu";
                case 1:
                    return "Shop";
                case 2:
                    return "Lobby";
                case 3:
                    return "In game";
                case 4:
                    return "Spectator";
                case 5:
                    return "Briefing";
            }
            return "Unknown";
        }
        
        public static async void SwitchToSpec()
        {
            foreach (Player p in new PlayerList())
            {
                if (p.IsDead) continue;
                if (!await Sync.Data.Has(p.ServerId, "InGame")) continue;
                StartSpec(p);
                return;
            }
        }
        
        public static void StartSpec(Player p)
        {
            if (NetworkIsInSpectatorMode())
                NetworkSetInSpectatorMode(false, GetPlayerPed(_lastSpec));
                        
            var plPos = GetEntityCoords(GetPlayerPed(p.Handle), true);
            RequestCollisionAtCoord(plPos.X, plPos.Y, plPos.Z);
            _lastSpec = p.Handle;
            NetworkSetInSpectatorMode(true, GetPlayerPed(p.Handle));
            NetworkSetVoiceChannel(1);
        }
        
        public static void StopSpec()
        {
            if (NetworkIsInSpectatorMode())
                NetworkSetInSpectatorMode(false, GetPlayerPed(_lastSpec));
            //NetworkSetVoiceChannel(999);
        }
        
        public static int GetSpecLast()
        {
            return _lastSpec;
        }
        
        public static void SwitchToGame()
        {
            SetEntityHealth(GetPlayerPed(-1), 200);
            Notification.Send("~g~GO! GO! GO!");
            MenuList.HideMenu();
        }
        
        public static async void SwitchToBriefing()
        {
            await UI.ShowLoadDisplay();

            Freeze(true);
            Invisible(true);
            
            float spawnX = (float) await Sync.Data.Get(GetServerId(), "spawnPosX");
            float spawnY = (float) await Sync.Data.Get(GetServerId(), "spawnPosY");
            float spawnZ = (float) await Sync.Data.Get(GetServerId(), "spawnPosZ");
            float spawnRot = (float) await Sync.Data.Get(GetServerId(), "spawnRot");

            NetworkSetVoiceChannel(0);
            Respawn(new Vector3(spawnX, spawnY, spawnZ), spawnRot, true, false);
            
            await Delay(5000);
            
            Freeze(false);
            Invisible(false);

            MenuList.ShowWeaponShopMenu();
            
            await UI.HideLoadDisplay();
        }
        
        public static async void SwitchToLobby()
        {
            await UI.ShowLoadDisplay();

            Sync.Data.Set(-1, "StartTimer", 121);
            Respawn(WaitLobby, new Random().Next(360), false, false);
            NetworkSetVoiceChannel(999);
            MenuList.ShowLobbyMenu();
            
            Freeze(true);
            Invisible(true);
            await UI.HideLoadDisplay();
        }
        
        public static async void SwitchToMainMenu()
        {
            await UI.ShowLoadDisplay();

            var rand = new Random();
            int i = rand.Next(SpawnList.Length / 3);
            
            Respawn(new Vector3((float) SpawnList[i, 0], (float) SpawnList[i, 1], (float) SpawnList[i, 2] + 50), rand.Next(360), false, false);
            
            NetworkSetVoiceChannel(999);
            MenuList.ShowMainMenu();
            
            Freeze(true);
            Invisible(true);
            await UI.HideLoadDisplay();
        }
        
        public static void ResetValues()
        {   
            if (GetStatusType() != StatusTypes.InGame)
            {
                GameInfo.Active = false;
                GameInfo.CamType = 0;
                GameInfo.Timer = 0;
                GameInfo.Traitor1 = 0;
                GameInfo.Traitor2 = 0;
                GameInfo.Traitor3 = 0;
                GameInfo.Traitor4 = 0;
                GameInfo.Detective1 = 0;
                GameInfo.Detective2 = 0;
                GameInfo.PlayerType = 0;
                
                RemoveAllPedWeapons(GetPlayerPed(-1), false);
            }
            if (GetStatusType() == StatusTypes.MainMenu || GetStatusType() == StatusTypes.Lobby || GetStatusType() == StatusTypes.Shop)
            {
                Sync.Data.Set(GetServerId(), "playerWarning", 0);
                Sync.Data.Reset(GetServerId(), "isDead");
                Sync.Data.Reset(GetServerId(), "InGame");
                Sync.Data.Reset(GetServerId(), "CameraType");
            }
            
            CitizenFX.Core.UI.Screen.LoadingPrompt.Hide();
            Sync.Data.Reset(GetServerId(), "Lobby");
            
            LobbyInfo.Active = false;
            LobbyInfo.Timer = 0;
            LobbyInfo.Cam1 = 0;
            LobbyInfo.Cam2 = 0;
        }
        
        public static async void AddCashMoney(int money)
        {
            int moneyNow = await GetCashMoney();
            SetCashMoney(moneyNow + money);
        }

        public static async void RemoveCashMoney(int money)
        {
            int moneyNow = await GetCashMoney();
            SetCashMoney(moneyNow - money);
        }

        public static void SetCashMoney(int money)
        {
            Data.money = money;
            Sync.Data.Set(GetServerId(), "money", money);
        }

        public static async Task<int> GetCashMoney()
        {
            return Data.money = (int) await Sync.Data.Get(GetServerId(), "money");
        }
    }
}

public class StatusTypes
{
    public static int Unknown => -1;
    public static int MainMenu => 0;
    public static int Shop => 1;
    public static int Lobby => 2;
    public static int InGame => 3;
    public static int Spectator => 4;
    public static int Briefing => 5;
}

public class PlayerData
{
    public int id { get; set; }
    public string lic { get; set; }
    public int score { get; set; }
    public string skin { get; set; }
    public int money { get; set; }
}

public class LobbyInfo
{
    public bool Active { get; set; }
    public int Timer { get; set; }
    public int Cam1 { get; set; }
    public int Cam2 { get; set; }
    public int CountPlayers { get; set; }
}

public class GameInfo
{
    public bool Active { get; set; }
    public int Timer { get; set; }
    public int CamType { get; set; }
    public int PlayerType { get; set; }
    public int Traitor1 { get; set; }
    public int Traitor2 { get; set; }
    public int Traitor3 { get; set; }
    public int Traitor4 { get; set; }
    public int Detective1 { get; set; }
    public int Detective2 { get; set; }
}

public class MapInfo
{
    public string Name { get; set; }
    public int Briefing { get; set; }
    public int MaxPlayers { get; set; }
    public double CenterX { get; set; }
    public double CenterY { get; set; }
    public double CenterZ { get; set; }
    public double MaxRadius { get; set; }
    public int PlayTime { get; set; }
    public string Weather { get; set; }
    public int Hour { get; set; }
    public bool CanSwim { get; set; }
    public bool EnableScope { get; set; }
}

public class PlayerTypes
{
    public static int Unknown => -1;
    public static int Innocent => 0;
    public static int Traitor => 1;
    public static int Detective => 2;
}