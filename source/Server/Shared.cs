using System;
using CitizenFX.Core;

namespace Server
{
    public class Shared : BaseScript 
    {
        public Shared()
        {
            EventHandlers.Add("TTT:TriggerEventToAllPlayers", new Action<string, int, object, object, object, object, object, object, object, object, object, object>(TriggerEventToAllPlayers));
            EventHandlers.Add("TTT:TriggerEventToPlayer", new Action<int, string, int, object, object, object, object, object, object, object, object, object, object>(TriggerEventToPlayer));
        }

        public static void TriggerEventToAllPlayers(string eventName, int countArgs = 0, object args1 = null, object args2 = null, object args3 = null, object args4 = null, object args5 = null, object args6 = null, object args7 = null, object args8 = null, object args9 = null, object args10 = null)
        {
            switch (countArgs)
            {
                case 0:
                    TriggerClientEvent(eventName);
                    break;
                case 1:
                    TriggerClientEvent(eventName, args1);
                    break;
                case 2:
                    TriggerClientEvent(eventName, args1, args2);
                    break;
                case 3:
                    TriggerClientEvent(eventName, args1, args2, args3);
                    break;
                case 4:
                    TriggerClientEvent(eventName, args1, args2, args3, args4);
                    break;
                case 5:
                    TriggerClientEvent(eventName, args1, args2, args3, args4, args5);
                    break;
                case 6:
                    TriggerClientEvent(eventName, args1, args2, args3, args4, args5, args6);
                    break;
                case 7:
                    TriggerClientEvent(eventName, args1, args2, args3, args4, args5, args6, args7);
                    break;
                case 8:
                    TriggerClientEvent(eventName, args1, args2, args3, args4, args5, args6, args7, args8);
                    break;
                case 9:
                    TriggerClientEvent(eventName, args1, args2, args3, args4, args5, args6, args7, args8, args9);
                    break;
                case 10:
                    TriggerClientEvent(eventName, args1, args2, args3, args4, args5, args6, args7, args8, args9, args10);
                    break;
            }
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, int countArgs = 0, object args1 = null, object args2 = null, object args3 = null, object args4 = null, object args5 = null, object args6 = null, object args7 = null, object args8 = null, object args9 = null, object args10 = null)
        {
            foreach (var pl in new PlayerList())
            {
                if (User.GetPlayerServerId(pl) != serverId) continue;

                switch (countArgs)
                {
                    case 0:
                        TriggerClientEvent(pl, eventName);
                        break;
                    case 1:
                        TriggerClientEvent(pl, eventName, args1);
                        break;
                    case 2:
                        TriggerClientEvent(pl, eventName, args1, args2);
                        break;
                    case 3:
                        TriggerClientEvent(pl, eventName, args1, args2, args3);
                        break;
                    case 4:
                        TriggerClientEvent(pl, eventName, args1, args2, args3, args4);
                        break;
                    case 5:
                        TriggerClientEvent(pl, eventName, args1, args2, args3, args4, args5);
                        break;
                    case 6:
                        TriggerClientEvent(pl, eventName, args1, args2, args3, args4, args5, args6);
                        break;
                    case 7:
                        TriggerClientEvent(pl, eventName, args1, args2, args3, args4, args5, args6, args7);
                        break;
                    case 8:
                        TriggerClientEvent(pl, eventName, args1, args2, args3, args4, args5, args6, args7, args8);
                        break;
                    case 9:
                        TriggerClientEvent(pl, eventName, args1, args2, args3, args4, args5, args6, args7, args8, args9);
                        break;
                    case 10:
                        TriggerClientEvent(pl, eventName, args1, args2, args3, args4, args5, args6, args7, args8, args9, args10);
                        break;
                }

                return;
            }
        }
    }
}