using System;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class Timer : BaseScript
    {
        public Timer()
        {
            Tick += SecTimer;
            Tick += SetTick;
            Tick += Set100Tick;
        }
        
        private static async Task Set100Tick()
        {
            await Delay(100);
            NetworkOverrideClockTime(User.MapInfo.Hour, 0, 0);
        }
        
        private static async Task SecTimer()
        {
            await Delay(1000);

            UI.CurrentOnline = new PlayerList().Count();
            
            SetWeatherTypePersist(User.MapInfo.Weather);
            SetWeatherTypeNowPersist(User.MapInfo.Weather);
            SetWeatherTypeNow(User.MapInfo.Weather);
            SetOverrideWeather(User.MapInfo.Weather);

            if (User.GameInfo.Active)
            {
                if (User.GameInfo.CamType == 0)
                {
                    if (GetFollowPedCamViewMode() != 4)
                        SetFollowPedCamViewMode(4);
                }
                else
                {
                    if (GetFollowPedCamViewMode() != 1)
                        SetFollowPedCamViewMode(1);
                }
            }

            if (User.GetStatusType() == StatusTypes.InGame || User.GetStatusType() == StatusTypes.Briefing)
            {
                var playerPos = GetEntityCoords(GetPlayerPed(-1), true);
                if (Main.GetDistance(playerPos, new Vector3((float) User.MapInfo.CenterX, (float) User.MapInfo.CenterY, (float) User.MapInfo.CenterZ)) > (float) User.MapInfo.MaxRadius || !User.MapInfo.CanSwim && (IsPedSwimming(GetPlayerPed(-1)) || IsPedSwimmingUnderWater(GetPlayerPed(-1))))
                {
                    float spawnX = (float) await Sync.Data.Get(User.GetServerId(), "spawnPosX");
                    float spawnY = (float) await Sync.Data.Get(User.GetServerId(), "spawnPosY");
                    float spawnZ = (float) await Sync.Data.Get(User.GetServerId(), "spawnPosZ");
                    
                    User.Teleport(new Vector3(spawnX, spawnY, spawnZ));
                }

                if (IsEntityDead(GetPlayerPed(-1)))
                {
                    NetworkSetVoiceChannel(1);
                    Sync.Data.Set(User.GetServerId(), "isDead", true);
                                        
                    if (!NetworkIsInSpectatorMode())
                        User.SetStatusType(StatusTypes.Spectator);
                }
            }
            if (User.GetStatusType() == StatusTypes.Lobby)
            {
                if (await Sync.Data.Has(-1, "GameTimer"))
                {
                    var t = TimeSpan.FromSeconds((int) await Sync.Data.Get(-1, "GameTimer"));
                    Screen.LoadingPrompt.Show($"Wait new game: {t.Minutes}:{t.Seconds:d2}");
                    await Delay(4000);
                }
                else if (await Sync.Data.Has(-1, "BriefingTimer"))
                {
                    Screen.LoadingPrompt.Show("Wait new game...");
                    await Delay(4000);
                }
                else
                {
                    Sync.Data.Set(User.GetServerId(), "Lobby", true);
                    if (User.LobbyInfo.CountPlayers < 2)
                    {
                        Screen.LoadingPrompt.Show($"Wait more players... {User.LobbyInfo.CountPlayers}/2");
                        Sync.Data.Set(-1, "StartTimer", 121);
                    }
                    else if (Screen.LoadingPrompt.IsActive)
                        Screen.LoadingPrompt.Hide();
                }
            }
            /*if (User.GetStatusType() == StatusTypes.Spectator)
            {
                if (IsEntityDead(GetPlayerPed(User.GetSpecLast())))
                {
                    User.SetStatusType(StatusTypes.Spectator);
                    await Delay(5000);
                }
            }*/
        }
        
        private static async Task SetTick()
        {
            Game.DisableControlThisFrame(0, Control.NextCamera);
        }
    }
}