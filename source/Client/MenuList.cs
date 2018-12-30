using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using NativeUI;
using static CitizenFX.Core.Native.API;

namespace Client
{
    public class MenuList : BaseScript
    {
        protected static UIMenu UiMenu = null;
        protected static MenuPool MenuPool = new MenuPool();
        
        public MenuList()
        {
            Tick += ProcessMenuPool;
            Tick += ProcessMainMenu;
        }
        
        public static Camera Camera;
        
        public static void ShowWeaponShopMenu()
        {
            HideMenu();

            var menu = new Menu();
            UiMenu = menu.Create("Briefing", "~b~Weapon shop");

            if (User.WeaponList == null)
            {
                menu.AddMenuItem(UiMenu, "Knife").Activated += (uimenu, idx) =>
                {
                    GiveWeaponToPed(GetPlayerPed(-1), (uint) WeaponHash.Knife, 1, false, true);
                };

                menu.AddMenuItem(UiMenu, "Pistol").Activated += (uimenu, idx) =>
                {
                    GiveWeaponToPed(GetPlayerPed(-1), (uint) WeaponHash.Pistol, 20, false, true);
                };

                menu.AddMenuItem(UiMenu, "SMG").Activated += (uimenu, idx) =>
                {
                    GiveWeaponToPed(GetPlayerPed(-1), (uint) WeaponHash.SMG, 60, false, true);
                };

                menu.AddMenuItem(UiMenu, "CarbineRifle").Activated += (uimenu, idx) =>
                {
                    GiveWeaponToPed(GetPlayerPed(-1), (uint) WeaponHash.CarbineRifle, 60, false, true);
                };

                menu.AddMenuItem(UiMenu, "AssaultRifle").Activated += (uimenu, idx) =>
                {
                    GiveWeaponToPed(GetPlayerPed(-1), (uint) WeaponHash.AssaultRifle, 60, false, true);
                };
            }
            else
            {
                foreach (var item in (IDictionary<String, Object>) User.WeaponList)
                {
                    menu.AddMenuItem(UiMenu, Main.WeaponNameToNormalName(item.Key)).Activated += (uimenu, idx) =>
                    {
                        GiveWeaponToPed(GetPlayerPed(-1), (uint) GetHashKey(item.Key), Convert.ToInt32(item.Value), false, true);
                    };
                }
            }
            
            menu.AddMenuItem(UiMenu, "~r~Close").Activated += (uimenu, item) =>
            {
                HideMenu();
            };
            
            MenuPool.Add(UiMenu);
        }
        
        public static void ShowVoteMapMenu()
        {
            HideMenu();
            
            if (User.MapList == null)
                return;

            var menu = new Menu();
            UiMenu = menu.Create("Lobby", "~b~Vote map", true, true);

            foreach (var item in (IList<Object>) User.MapList)
            {
                menu.AddMenuItem(UiMenu, item.ToString()).Activated += (uimenu, idx) =>
                {
                    Sync.Data.Set(User.GetServerId(), "voteMap", item.ToString());
                    Notification.Send($"You are vote ~g~{item}");
                };
            }
            
            MenuPool.Add(UiMenu);
        }
        
        public static async void ShowShopMenu()
        {
            HideMenu();

            var menu = new Menu();
            UiMenu = menu.Create("Shop", "~b~Skin Shop", true, true);
            
            RequestCollisionAtCoord(9.653649f, 528.3086f, 169.635f);
            await Delay(1000);
                    
            User.Teleport(new Vector3(9.653649f, 528.3086f, 169.635f));
            NetworkResurrectLocalPlayer(9.653649f, 528.3086f, 169.635f, 120.0613f, true, true);
            User.PedRotation(120.0613f);
                    
            User.Freeze(true);
                    
            Camera = new CitizenFX.Core.Camera(CreateCam("DEFAULT_SCRIPTED_CAMERA", true));
            Camera.IsActive = true;
            Camera.Position = new Vector3(8.243752f, 527.4373f, 171.6173f);
            Camera.PointAt(new Vector3(9.653649f, 528.3086f, 171.335f));
            RenderScriptCams(true, false, Camera.Handle, false, false);

            var skinList = Main.GetAllSkinList();
            
            var listSkin = new List<dynamic>();     
            for (int i = 0; i < skinList.Length / 2; i++)
                listSkin.Add("$" + skinList[i, 1].ToString());
            
                    
            var skinListMenu = menu.AddMenuItemList(UiMenu, "Skin List", listSkin, "Press enter if u wanna buy");
            skinListMenu.OnListSelected += async (uimenu, idx) =>
            {
                int sum = (int) skinList[idx, 1];
                if (await User.GetCashMoney() < sum)
                {
                    Notification.Send("~r~You dont have money");
                    return;
                }
                User.RemoveCashMoney(sum);
                Sync.Data.Set(User.GetServerId(), "skin", (string) skinList[idx, 0]);
                User.Data.skin = (string) skinList[idx, 0];
                User.SetSkin((string) skinList[idx, 0]);
                Notification.Send($"~g~You are buy new skin. Price: ${sum:#,#}");
            };
            
            skinListMenu.OnListChanged += (uimenu, idx) =>
            {
                User.SetSkin((string) skinList[idx, 0]);
            };
            
            menu.AddMenuItem(UiMenu, "~y~Exit to main menu").Activated += (uimenu, item) =>
            {
                HideMenu();
                User.Freeze(false);
                Camera.Delete();
                Camera = null;
                RenderScriptCams(false, true, 500, true, true);
                User.SetSkin(User.Data.skin);
                User.SetStatusType(StatusTypes.MainMenu);
            };
            
            MenuPool.Add(UiMenu);
        }
        
        public static void ShowLobbyMenu()
        {
            HideMenu();

            var menu = new Menu();
            UiMenu = menu.Create("Lobby", "~b~Vote", true, true);
            
            var listCam = new List<dynamic> {"First person", "Third person"};     
            menu.AddMenuItemList(UiMenu, "Camera", listCam, "Press enter if u wanna vote").OnListSelected += (uimenu, idx) =>
            {
                Sync.Data.Set(User.GetServerId(), "CameraType", idx);
            };
            
            menu.AddMenuItem(UiMenu, "~y~Exit to main menu").Activated += (uimenu, item) =>
            {
                HideMenu();
                User.SetStatusType(StatusTypes.MainMenu);
            };
            
            MenuPool.Add(UiMenu);
        }
        
        public static void ShowMainMenu()
        {
            HideMenu();

            var menu = new Menu();
            UiMenu = menu.Create("Menu", "~b~Main menu", true, true);
            
            menu.AddMenuItem(UiMenu, "~g~Play").Activated += async (uimenu, item) =>
            {
                HideMenu();
                User.SetStatusType(StatusTypes.Lobby);
            };
            
            menu.AddMenuItem(UiMenu, "Skin shop").Activated += (uimenu, item) =>
            {
                HideMenu();
                User.SetStatusType(StatusTypes.Shop);
            };
            
            menu.AddMenuItem(UiMenu, "~r~Exit", "You are kicked from the server").Activated += (uimenu, item) =>
            {
                HideMenu();
                TriggerServerEvent("TTT:KickPlayer", "You are kicked from the server");
            };
            
            MenuPool.Add(UiMenu);
        }
        
        public static async void ShowSpecMenu()
        {
            HideMenu();
            
            var menu = new Menu();
            UiMenu = menu.Create("Spec", "~b~Spectator menu");
            
            Sync.Data.ShowSyncMessage = false;
            CitizenFX.Core.UI.Screen.LoadingPrompt.Show("Loading data...");
            
            foreach (Player p in new PlayerList())
            {
                try
                {
                    if (p.IsDead) continue;
                    if (!await Sync.Data.Has(p.ServerId, "InGame")) continue;
                    menu.AddMenuItem(UiMenu, $"~b~ID: ~s~{p.Name}").Activated += (uimenu, item) =>
                    {
                        User.StartSpec(p);
                    };
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
            
            CitizenFX.Core.UI.Screen.LoadingPrompt.Hide();
            Sync.Data.ShowSyncMessage = true;
            
            menu.AddMenuItem(UiMenu, "~r~Exit to main menu").Activated += (uimenu, item) =>
            {
                User.SetStatusType(StatusTypes.MainMenu);
            };
            
            MenuPool.Add(UiMenu);
        }
        
        public static void HideMenu()
        {
            MenuPool = new MenuPool();
            UiMenu = null;
        }
        
        private static async Task ProcessMainMenu()
        {
            if (UiMenu != null)
            {
                if (UiMenu.Visible)
                {
                    Game.DisableControlThisFrame(0, (Control) 157);
                    Game.DisableControlThisFrame(0, (Control) 158);
                }
            }
            else
            {
                await Delay(10);
            }
        }

        private static async Task ProcessMenuPool()
        {
            MenuPool.ProcessMenus();

            if (MenuPool.ToList().Count == 0 && Camera != null)
            {
                Camera = null;
                ShowShopMenu();
            }
        }
    }
}