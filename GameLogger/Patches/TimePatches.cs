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
            Watch.Start();

            Utils.Write($"Started game on {Utils.GetMap()} - game mode: {GameOptionsManager.Instance.currentGameMode}");
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        [HarmonyPostfix]

        public static void End()
        {
            Watch.Stop();
            Utils.Write($"Game finished in {Watch.Elapsed.Minutes} minutes");
            Watch.Reset();
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnDisconnected))]
        [HarmonyPostfix]

        public static void OnDC(AmongUsClient __instance)
        {
            if (__instance.AmClient) Watch.Reset();
        }
    }
}