using System.Collections.Generic;
using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]

    public class VentLogs
    {
        public static Dictionary<int, string> SkeldVentLocations = new()
        {
            {0, "Admin"},
            {1, "Navigation Hallway"},
            {2, "Cafeteria"},
            {3, "Electrical"},
            {4, "Upper Engine"},
            {5, "Security"},
            {6, "Medbay"},
            {7, "Weapons"},
            {8, "Lower Reactor"},
            {9, "Lower Engine"},
            {10, "Shields"},
            {11, "Upper Reactor"},
            {12, "Upper Navigation"},
            {13, "Lower Navigation"},
        };

        public static Dictionary<int, string> MiraVentLocations = new()
        {
            {0, "Admin"},
            {1, "Balcony"},
            {2, "Right Y hallway"},
            {3, "Reactor"},
            {4, "Laboratory"},
            {5, "Office"},
            {6, "Admin"},
            {7, "Greenhouse"},
            {8, "Medbay"},
            {9, "Decon"},
            {10, "Locker room"},
            {11, "Launchpad"},
        };

        public static Dictionary<int, string> PolusVentLocations = new()
        {
            {0, "Electrical"},
            {1, "Under Electrical Cage"},
            {2, "O2"},
            {3, "Comms"},
            {4, "Office"},
            {5, "Admin"},
            {6, "Laboratory"},
            {7, "Under Laboratory"},
            {8, "Storage"},
            {9, "Right Reactor"},
            {10, "Left Reactor"},
            {11, "Outside Admin"},
        };

        public static Dictionary<int, string> AirshipVentLocations = new()
        {
            {0, "Vault"},
            {1, "Cockpit"},
            {2, "Left Viewing Deck"},
            {3, "Engine"},
            {4, "Kitchen"},
            {5, "Main Hall Bottom"},
            {6, "Main Hall Trash"},
            {7, "Right Gap"},
            {8, "Left Gap"},
            {9, "Showers"},
            {10, "Records"},
            {11, "Cargo Bay"},
        };

        public static Dictionary<int, string> FungleVentLocations = new()
        {
            {0, "Comms"},
            {1, "Kitchen"},
            {2, "Lookout"},
            {3, "Above Dorm Room"},
            {4, "Laboratory"},
            {5, "Reactor"},
            {6, "Right Of Laboratory"},
            {7, "Monitor Mushroom"},
            {8, "Splash Zone"},
            {9, "Left Of Dropship"},
        };

        public static Dictionary<byte, Dictionary<int, string>> AllMapVents = new()
        {
            {0, SkeldVentLocations},
            {1, MiraVentLocations},
            {2, PolusVentLocations},
            {4, AirshipVentLocations},
            {5, FungleVentLocations},
        };
        
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoEnterVent))]
        [HarmonyPostfix]

        public static void Enter(ref PlayerPhysics __instance, ref int id)
        {
            var player = Utils.FullName(__instance.myPlayer.Data);
            var vent = AllMapVents.TryGetValue(GameOptionsManager.Instance.currentNormalGameOptions.MapId, out var mapvents)
                ? mapvents.TryGetValue(id, out var ventName) ? ventName : "Unknown Vent"
                : "Unknown Vent";

            Utils.Write($"{player} entered vent at {vent}");
        }

        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.CoExitVent))]
        [HarmonyPostfix]

        public static void Exit(ref PlayerPhysics __instance, ref int id)
        {
            var player = Utils.FullName(__instance.myPlayer.Data);
            var vent = AllMapVents.TryGetValue(GameOptionsManager.Instance.currentNormalGameOptions.MapId, out var mapvents)
                ? mapvents.TryGetValue(id, out var ventName) ? ventName : "Unknown Vent"
                : "Unknown Vent";

            Utils.Write($"{player} exited vent at {vent}");
        }
    }
}