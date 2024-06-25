using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace GameLogger
{
    [HarmonyPatch]
    
    public class MeetingLogs
    {
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.CoIntro))]
        [HarmonyPostfix]

        public static void Start(ref NetworkedPlayerInfo reporter, ref NetworkedPlayerInfo reportedBody, ref Il2CppReferenceArray<NetworkedPlayerInfo> deadBodies)
        {
            string action = reportedBody == null ? "This is a emergency meeting" : $"{Utils.FullName(reportedBody)}'s body was found";
            string bodytext = "Players died this round: ";
            
            if (deadBodies.Length == 0)
            {
                bodytext = "No one died this round";
            }
            else
            {
                foreach (var body in deadBodies)
                {
                    bodytext += $"{Utils.FullName(body)}, ";
                }
                bodytext = bodytext.Remove(bodytext.LastIndexOf(","));
            }
            
            Utils.Write($"Meeting started by {Utils.FullName(reporter)}", action, bodytext);
        }

        [HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
        [HarmonyPostfix]

        public static void End(ExileController __instance)
        {
            Utils.Write(__instance.completeString);
        }
    }
}