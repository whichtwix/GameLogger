using System.Diagnostics;
using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]
    
    public class TimerLogs
    {
        public static Stopwatch Watch { get; set; } = new();

        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Awake))]
        [HarmonyPostfix]

        public static void Start()
        {
            GameLogger.Logger.LogMessage("ShipStatus.Awake reached");

            Watch.Start();

            Utils.Write($"Started game on {Utils.GetMap()} - game mode: {GameOptionsManager.Instance.currentGameMode}");
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnDisconnected))]
        [HarmonyPostfix]

        public static void OnDC(AmongUsClient __instance)
        {
            if (__instance.AmClient) Watch.Reset();
        }
    }
}