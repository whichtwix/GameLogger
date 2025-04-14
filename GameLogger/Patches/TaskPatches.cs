using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]
    
    public class TaskLogs
    {
        public static TaskStates State { get; set; } = TaskStates.None;
        
        public enum TaskStates
        {
            None,

            Quarter,

            Half,

            ThreeQuarters,
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
        [HarmonyPostfix]

        public static void Postfix(GameData __instance)
        {
            if (__instance.TotalTasks == 0) return;
            
            var completed = (double) __instance.CompletedTasks / __instance.TotalTasks;
            string info;

            if (completed >= 0.75 && State < TaskStates.ThreeQuarters)
            {
                info = $"Task completion has reached 75%";
                State = TaskStates.ThreeQuarters;
                Utils.Write(info);
            }
            else if (completed >= 0.5 && State < TaskStates.Half)
            {
                info = $"Task completion has reached 50%";
                State = TaskStates.Half;
                Utils.Write(info);
            }
            else if (completed >= 0.25 && State < TaskStates.Quarter)
            {
                info = $"Task completion has reached 25%";
                State = TaskStates.Quarter;
                Utils.Write(info);
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]
        [HarmonyPostfix]

        public static void Postfix(PlayerControl __instance)
        {
            if (__instance.AllTasksCompleted())
            {
                var player = Utils.FullName(__instance.Data);
                var info = $"{player} has completed all tasks";
                Utils.Write(info);
            }
        }
    }
}