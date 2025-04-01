using System;
using System.IO;
using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]

    public class LobbyLogs
    {
        [HarmonyPatch(typeof(LobbyBehaviour), nameof(LobbyBehaviour.Start))]
        [HarmonyPostfix]

        public static void Postfix()
        {
            if (GameLogger.Builder.Length > 0) 
            {
                if (!Directory.Exists("GameLogs")) Directory.CreateDirectory("GameLogs");
                var game = GameLogger.Builder.ToString();
                File.AppendAllText($"GameLogs\\{DateTime.Now:u}_{Utils.GetMap()}.txt".Replace(":", "-"), game);
                GameLogger.Builder.Clear();
            }
            
            TimerLogs.Watch.Reset();
            TaskLogs.State = TaskLogs.TaskStates.None;
            KillLogs.ImpKills.Clear();
        }
    }
}