using System;
using System.IO;
using HarmonyLib;
using UnityEngine;

namespace GameLogger
{
    [HarmonyPatch]

    public class LobbyLogs
    {
        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
        [HarmonyPostfix]

        public static void Postfix()
        {
#if !ANDROID
            if (GameLogger.Builder.Length > 0)
            {
                if (!Directory.Exists("GameLogs")) Directory.CreateDirectory("GameLogs");
                var game = GameLogger.Builder.ToString();
                File.AppendAllText($"GameLogs\\{DateTime.Now:u}_{Utils.GetMap()}.txt".Replace(":", "-"), game);
                GameLogger.Builder.Clear();
            }
#else
            if (GameLogger.Builder.Length > 0)
            {
                var path = Path.GetFullPath("GameLogs", Application.persistentDataPath);
                System.Console.WriteLine($"android path: {path}");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var game = GameLogger.Builder.ToString();
                var log = Path.Combine(path, $"{DateTime.Now:u}_{Utils.GetMap()}.txt".Replace(":", "-"));
                File.AppendAllText(log, game);
                GameLogger.Builder.Clear();
            }
#endif
            
            TimerLogs.Watch.Reset();
            TaskLogs.State = TaskLogs.TaskStates.None;
            KillLogs.ImpKills.Clear();
        }
    }
}