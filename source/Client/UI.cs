using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using NativeUI;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class UI : BaseScript
    {
        public static SizeF Res = GetScreenResolutionMaintainRatio();
        private static float Height = Res.Height;
        private static readonly float Width = Res.Width;
        public static int CurrentOnline = 0;

        private static readonly Dictionary<int, bool> VisiblePlayer = new Dictionary<int, bool>();
            
        public UI()
        {
            Tick += TimeSync;
            Tick += SetSec;
            Tick += Set10Tick;
            Tick += SetTickHideHud;
            Tick += SetTickHideNpc;
            Tick += SetTickShowNickname;
        }
        
        private static async Task SetTickShowNickname()
        {
            if (Game.CurrentInputMode == InputMode.MouseAndKeyboard && !Menu.IsShowInput && User.GameInfo.Active)
            {
                foreach (var p in new PlayerList())
                {
                    if (p.ServerId == GetPlayerServerId(PlayerId())) continue;
                    if (!(Main.GetDistance(GetEntityCoords(GetPlayerPed(p.Handle), true),
                              GetEntityCoords(GetPlayerPed(-1), true)) < 20f)) continue;
                    var entity = new MyEntity(GetPlayerPed(p.Handle));

                    if (!VisiblePlayer.ContainsKey(p.ServerId)) continue;
                    var text = p.Name;
                    
                    if (User.GameInfo.Active && User.GameInfo.PlayerType == PlayerTypes.Traitor)
                    {
                        if (p.ServerId == User.GameInfo.Traitor1)
                            text = $"~r~{p.Name}";
                        else if (p.ServerId == User.GameInfo.Traitor2)
                            text = $"~r~{p.Name}";
                        else if (p.ServerId == User.GameInfo.Traitor3)
                            text = $"~r~{p.Name}";
                        else if (p.ServerId == User.GameInfo.Traitor4)
                            text = $"~r~{p.Name}";
                    }
                    if (User.GameInfo.Active && User.GameInfo.PlayerType == PlayerTypes.Detective)
                    {
                        if (p.ServerId == User.GameInfo.Detective1)
                            text = $"~b~{p.Name}";
                        else if (p.ServerId == User.GameInfo.Detective2)
                            text = $"~b~{p.Name}";
                    }
                    
                    if (NetworkIsPlayerTalking(p.Handle))
                        text += "\n~b~Talking";
                    DrawText3D(entity.Position + new Vector3(0, 0, 0.6f), text);
                }
            }
        }
        
        private static async Task SetSec()
        {
            await Delay(1000);
            if (Game.CurrentInputMode == InputMode.MouseAndKeyboard && !Menu.IsShowInput && User.GameInfo.Active)
            {
                VisiblePlayer.Clear();
                foreach (var p in new PlayerList())
                {
                    if (p.ServerId == GetPlayerServerId(PlayerId())) continue;
                    if (!(Main.GetDistance(GetEntityCoords(GetPlayerPed(p.Handle), true),
                              GetEntityCoords(GetPlayerPed(-1), true)) < 20f)) continue;

                    if (HasEntityClearLosToEntity(GetPlayerPed(-1), GetPlayerPed(p.Handle), 17))
                        VisiblePlayer.Add(p.ServerId, true);
                }
            }
        }
        
        private static async Task SetTickHideNpc()
        {
            SetParkedVehicleDensityMultiplierThisFrame(0);
            SetVehicleDensityMultiplierThisFrame(0);
            SetParkedVehicleDensityMultiplierThisFrame(0);
            SetPedDensityMultiplierThisFrame(0);
            SetRandomVehicleDensityMultiplierThisFrame(0);
            SetScenarioPedDensityMultiplierThisFrame(0, 0);
            SetSomethingMultiplierThisFrame(false);
            SetSomeVehicleDensityMultiplierThisFrame(0);
            SetVehicleDensityMultiplierThisFrame(0);
        }
        
        private static async Task SetTickHideHud()
        {
            HideHudComponentThisFrame(1); // Wanted Stars
            HideHudComponentThisFrame(3); // Cash
            HideHudComponentThisFrame(4); // MP Cash
            HideHudComponentThisFrame(6); // Vehicle Name
            HideHudComponentThisFrame(7); // Area Name
            HideHudComponentThisFrame(8);// Vehicle Class
            HideHudComponentThisFrame(9); // Street Name
            HideHudComponentThisFrame(13); // Cash Change
            
            if (!User.MapInfo.EnableScope)
                HideHudComponentThisFrame(14); // GunScope
            //HideHudComponentThisFrame(17); // Save Game
        }
        
        public static SizeF GetScreenResolutionMaintainRatio()
        {
            return new SizeF(Screen.Resolution.Height * ((float) Screen.Resolution.Width / (float) Screen.Resolution.Height), Screen.Resolution.Height);
        }

        protected static TimerBarPool TimerBarPool = new TimerBarPool();
        
        private static async Task Set10Tick()
        {
            await Delay(10);

            TimerBarPool =  new TimerBarPool();
            TimerBarPool.Add(new TextTimerBar("Status", User.GetStatusTypeLabel()));

            if (User.LobbyInfo.Active)
            {
                TimerBarPool.Add(new TextTimerBar("Players", $"{User.LobbyInfo.CountPlayers}"));
                TimerBarPool.Add(new TextTimerBar("Third person", $"{User.LobbyInfo.Cam2} votes"));
                TimerBarPool.Add(new TextTimerBar("First person", $"{User.LobbyInfo.Cam1} votes"));

                if (User.LobbyInfo.Timer < 120)
                {
                    var t = TimeSpan.FromSeconds(User.LobbyInfo.Timer);
                    if (User.LobbyInfo.Timer < 10)
                        TimerBarPool.Add(new TextTimerBar("Time to start", $"~r~{t.Minutes}:{t.Seconds:d2}"));
                    else if (User.LobbyInfo.Timer < 30)
                        TimerBarPool.Add(new TextTimerBar("Time to start", $"~y~{t.Minutes}:{t.Seconds:d2}"));
                    else
                        TimerBarPool.Add(new TextTimerBar("Time to start", $"{t.Minutes}:{t.Seconds:d2}"));
                }
            }
            
            if (User.GameInfo.Active)
            {
                if (User.GameInfo.PlayerType == PlayerTypes.Innocent)
                    TimerBarPool.Add(new TextTimerBar("Role", "~g~Innocent"));
                else if (User.GameInfo.PlayerType == PlayerTypes.Traitor)
                    TimerBarPool.Add(new TextTimerBar("Role", "~r~Traitor"));
                else if (User.GameInfo.PlayerType == PlayerTypes.Detective)
                    TimerBarPool.Add(new TextTimerBar("Role", "~b~Detective"));
                else
                    TimerBarPool.Add(new TextTimerBar("Role", "Unknown"));

                var t = TimeSpan.FromSeconds(User.GameInfo.Timer);
                if (User.GameInfo.Timer < 60)
                    TimerBarPool.Add(new TextTimerBar("Timer", $"~r~{t.Minutes}:{t.Seconds:d2}"));
                else if (User.GameInfo.Timer < 90)
                    TimerBarPool.Add(new TextTimerBar("Timer", $"~y~{t.Minutes}:{t.Seconds:d2}"));
                else
                    TimerBarPool.Add(new TextTimerBar("Timer", $"{t.Minutes}:{t.Seconds:d2}"));
            }
            else if (User.MapInfo.Briefing > 0)
            {
                TimerBarPool.Add(new TextTimerBar("Role", "Unknown"));
                TimerBarPool.Add(new TextTimerBar("Briefing", $"{User.MapInfo.Briefing:d2}"));
            }
            
            foreach (var p in new PlayerList())
            {
                if (!NetworkIsPlayerTalking(p.Handle)) continue;
                TimerBarPool.Add(new TextTimerBar("", $"{p.Name} ~b~Talking"));
            }
        }

        private static async Task TimeSync()
        {        
            if (IsHudPreferenceSwitchedOn() || Screen.Hud.IsVisible)
            {
                TimerBarPool.Draw();
                
                if (User.Data.money < 0)
                    DrawText("$" + User.Data.money.ToString("#,#"), 15, 50, 0.6f, 244, 67, 54, 255, 7, 2, false, true, 0, 0, 2);
                else
                    DrawText("$" + User.Data.money.ToString("#,#"), 15, 50, 0.6f, 115, 186, 131, 255, 7, 2, false, true, 0, 0, 2);
                
                DrawText($"Players: {CurrentOnline}", 130, 8, 0.3f, 255, 255, 255, 150, 0, 2, false, false, 0, 0, 2);
            }
        }
        
        public static void DrawText3D(Vector3 pos, string text) {
            
            pos.Z += .5f;
            SetDrawOrigin(pos.X, pos.Y, pos.Z, 0);
            var camPos = GetGameplayCamCoords();
            var dist = World.GetDistance(pos, camPos);
            float scale = 1 / dist * 2f;
            float fov = 1 / GetGameplayCamFov() * 100f;
            scale *= fov;
            if (scale < 0.4)
                scale = 0.4f;

            SetTextScale(0.1f * scale, 0.55f * scale);
            SetTextFont(0);
            SetTextProportional(true);
            SetTextColour(255, 255, 255, 255);
            SetTextDropshadow(0, 0, 0, 0, 255);
            SetTextOutline();
            SetTextEdge(2, 0, 0, 0, 150);
            SetTextDropShadow();
            SetTextEntry("STRING");
            SetTextCentre(true);
            AddTextComponentString(text);
            CitizenFX.Core.Native.API.DrawText(0, 0);
            ClearDrawOrigin();
        }

        public static void DrawMarker(int type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, int r, int g, int b, int a)
        {
            CitizenFX.Core.Native.API.DrawMarker(type, pos.X, pos.Y, pos.Z, dir.X, dir.Y, dir.Z, rot.X, rot.Y, rot.Z, scale.X, scale.Y, scale.Z, r, g, b, a, false, true, 1, false, null, null, false);
        }

        public static void Draw3DText(string text, Vector3 pos, float size, int r, int g, int b, int a)
        {
            /*if (World3dToScreen2d(pos.X, pos.Y, pos.Z, ref _screenX, ref _screenY))
                DrawText(text, Res.Width * _screenX, Res.Height * _screenY, size, r, g, b, a, 4, 0, true, true, 0);*/
        }
        
        public static void DrawSprite(string dict, string txtName, float xPos, float yPos, float width, float height, float heading, int r, int g, int b, int alpha, int vAlig = 0, int hAlig = 0)
        {
            if (!IsHudPreferenceSwitchedOn() || !CitizenFX.Core.UI.Screen.Hud.IsVisible) return;
            
            if (!HasStreamedTextureDictLoaded(dict))
                RequestStreamedTextureDict(dict, true);
            
            if (hAlig == 2)
                xPos = Res.Width - xPos;
            else if (hAlig == 1)
                xPos = Res.Width / 2 + xPos;
            
            if (vAlig == 2)
                yPos = Res.Height - yPos;
            else if (vAlig == 1)
                yPos = Res.Height / 2 + yPos;

            float w = width / Width;
            float h = height / Height;
            float x = xPos / Width + w * 0.5f;
            float y = yPos / Height + h * 0.5f;
            
            CitizenFX.Core.Native.API.DrawSprite(dict, txtName, x, y, w, h, heading, r, g, b, alpha);
        }

        public static void DrawRectangle(float xPos, float yPos, float wSize, float hSize, int r, int g, int b, int alpha, int vAlig = 0, int hAlig = 0)
        {
            if (!IsHudPreferenceSwitchedOn() || !CitizenFX.Core.UI.Screen.Hud.IsVisible) return;
            
            if (hAlig == 2)
                xPos = Res.Width - xPos;
            else if (hAlig == 1)
                xPos = Res.Width / 2 + xPos;
            
            if (vAlig == 2)
                yPos = Res.Height - yPos;
            else if (vAlig == 1)
                yPos = Res.Height / 2 + yPos;
            
            float w = wSize / Width;
            float h = hSize / Height;
            float x = xPos / Width + w * 0.5f;
            float y = yPos / Height + h * 0.5f;
        
            DrawRect(x, y, w, h, r, g, b, alpha);
        }
        
        public static void DrawText(string caption, float xPos, float yPos, float scale, int r, int g, int b, int alpha, int font, int justify, bool shadow, bool outline, int wordWrap, int vAlig = 0, int hAlig = 0)
        {
            if (!IsHudPreferenceSwitchedOn() || !CitizenFX.Core.UI.Screen.Hud.IsVisible) return;
        
            if (hAlig == 2)
                xPos = Res.Width - xPos;
            else if (hAlig == 1)
                xPos = Res.Width / 2 + xPos;
            
            if (vAlig == 2)
                yPos = Res.Height - yPos;
            else if (vAlig == 1)
                yPos = Res.Height / 2 + yPos;
            
            float x = xPos / Width;
            float y = yPos / Height;
            
            SetTextFont(font);
            SetTextScale(1f, scale);
            SetTextColour(r, g, b, alpha);
            
            if (shadow) SetTextDropShadow();
            if (outline) SetTextOutline();
            switch (justify)
            {
                case 1:
                    SetTextCentre(true);
                    break;
                case 2:
                    SetTextRightJustify(true);
                    SetTextWrap(0, x);
                    break;
            }
        
            if (wordWrap != 0)
                SetTextWrap(x, (xPos + wordWrap) / Width);
        
            BeginTextCommandDisplayText("STRING");
        
            const int maxStringLength = 99;
            for (int i = 0; i < caption.Length; i += maxStringLength)
                AddTextComponentSubstringPlayerName(caption.Substring(i, System.Math.Min(maxStringLength, caption.Length - i)));
        
            EndTextCommandDisplayText(x, y);
        }
        
        public static void DrawTextOnScreen(string text, float xPosition, float yPosition, float size, int r, int g, int b, int a, Alignment justification, int font = 0, bool isTextOutline = false)
        {
            if (!IsHudPreferenceSwitchedOn() || !Screen.Hud.IsVisible) return;
            
            SetTextFont(font);
            SetTextScale(1.0f, size);
            SetTextColour(r, g, b, a);
            if (justification == Alignment.Right) SetTextWrap(0f, xPosition);
            SetTextJustification((int)justification);
            if (isTextOutline) SetTextOutline();
            BeginTextCommandDisplayText("STRING");
            AddTextComponentSubstringPlayerName(text);
            EndTextCommandDisplayText(xPosition, yPosition);
        }
        
        public static void ShowToolTip(string text)
        {
            var rand = new Random();
            int idx = rand.Next(0, 50000);
            AddTextEntry($"toolTip{idx}", text);
            SetTextComponentFormat($"toolTip{idx}");
            EndTextCommandDisplayHelp(0, false, true, -1);
        }

        public static async void ShowSimpleShard(string text, string desc ="", int time = 5000)
        {
            var msg = new BigMessageHandler();
            await msg.Load();
            msg.ShowSimpleShard(text, desc, time);
        }

        public static async void ShowMissionPassedMessage(string text, int time = 5000)
        {
            var msg = new BigMessageHandler();
            await msg.Load();
            msg.ShowMissionPassedMessage(text, time);
        }
        
        public static async Task<bool> ShowLoadDisplay()
        {
            DoScreenFadeOut(500);
            while (IsScreenFadingOut())
                await Delay(1);
            return true;
        }

        public static async Task<bool> HideLoadDisplay()
        {
            DoScreenFadeIn(500);
            while (IsScreenFadingIn())
                await Delay(1);
            return true;
        }
    }
}