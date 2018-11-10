using CitizenFX.Core;

namespace Client
{
    public class Shared : BaseScript 
    {
        public static void TriggerEventToPlayer(int serverId, string eventName)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 0);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 1, args1);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 2, args1, args2);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2, object args3)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 3, args1, args2, args3);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2, object args3, object args4)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 4, args1, args2, args3, args4);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2, object args3, object args4, object args5)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 5, args1, args2, args3, args4, args5);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2, object args3, object args4, object args5, object args6)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 6, args1, args2, args3, args4, args5, args6);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2, object args3, object args4, object args5, object args6, object args7)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 7, args1, args2, args3, args4, args5, args6, args7);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2, object args3, object args4, object args5, object args6, object args7, object args8)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 8, args1, args2, args3, args4, args5, args6, args7, args8);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2, object args3, object args4, object args5, object args6, object args7, object args8, object args9)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 9, args1, args2, args3, args4, args5, args6, args7, args8, args9);
        }

        public static void TriggerEventToPlayer(int serverId, string eventName, object args1, object args2, object args3, object args4, object args5, object args6, object args7, object args8, object args9, object args10)
        {
            TriggerServerEvent("TTT:TriggerEventToPlayer", serverId, eventName, 10, args1, args2, args3, args4, args5, args6, args7, args8, args9, args10);
        }

        public static void TriggerEventToAllPlayers(string eventName)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 0);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 1, args1);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 2, args1, args2);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2, object args3)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 3, args1, args2, args3);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2, object args3, object args4)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 4, args1, args2, args3, args4);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2, object args3, object args4, object args5)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 5, args1, args2, args3, args4, args5);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2, object args3, object args4, object args5, object args6)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 6, args1, args2, args3, args4, args5, args6);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2, object args3, object args4, object args5, object args6, object args7)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 7, args1, args2, args3, args4, args5, args6, args7);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2, object args3, object args4, object args5, object args6, object args7, object args8)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 8, args1, args2, args3, args4, args5, args6, args7, args8);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2, object args3, object args4, object args5, object args6, object args7, object args8, object args9)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 9, args1, args2, args3, args4, args5, args6, args7, args8, args9);
        }

        public static void TriggerEventToAllPlayers(string eventName, object args1, object args2, object args3, object args4, object args5, object args6, object args7, object args8, object args9, object args10)
        {
            TriggerServerEvent("TTT:TriggerEventToAllPlayers", eventName, 10, args1, args2, args3, args4, args5, args6, args7, args8, args9, args10);
        }
    }
}