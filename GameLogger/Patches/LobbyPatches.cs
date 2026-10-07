using System;
using System.IO;
using HarmonyLib;
using UnityEngine;

namespace GameLogger
{
    [HarmonyPatch]

    public class LobbyLogs
    {
        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        [HarmonyPostfix]

        public static void Postfix() => Save();

        public static void Save()
        {
            if (OperatingSystem.IsAndroid())
            {
                GameLogger.Logger.LogMessage("Android detected, saving logs to persistent data path.");
                if (GameLogger.Builder.Length > 0)
                {
                    var path = Path.GetFullPath("GameLogs", Application.persistentDataPath);
                    GameLogger.Logger.LogMessage($"android path: {path}");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var game = GameLogger.Builder.ToString();
                    var log = Path.Combine(path, $"{DateTime.Now:u}_{Utils.GetMap()}.txt".Replace(":", "-"));
                    File.AppendAllText(log, game);
                    GameLogger.Builder.Clear();
                }
            }
            else
            {
                GameLogger.Logger.LogMessage("Non-Android detected, saving logs to local GameLogs folder.");
                if (GameLogger.Builder.Length > 0)
                {
                    if (!Directory.Exists("GameLogs")) Directory.CreateDirectory("GameLogs");
                    var game = GameLogger.Builder.ToString();
                    File.AppendAllText($"GameLogs\\{DateTime.Now:u}_{Utils.GetMap()}.txt".Replace(":", "-"), game);
                    GameLogger.Builder.Clear();
                }
            }

            TimerLogs.Watch.Reset();
            TaskLogs.State = TaskLogs.TaskStates.None;
            KillLogs.ImpKills.Clear();
            MeetingLogs.GhostsLogged.Clear();
        }
    }
}