using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]

    public class EndGameLogs
    {
        [HarmonyPatch(typeof(EndGameResult), nameof(EndGameResult.Create))]
        [HarmonyPostfix]

        public static void Postfix(ref EndGameResult __result)
        {
            string text = "Winners: ";
            switch (__result.GameOverReason)
            {
                case GameOverReason.CrewmatesByVote:
                    text += "Crewmates by voting out Impostors";
                    break;
                case GameOverReason.CrewmatesByTask:
                    text += "Crewmates by task win";
                    break;
                case GameOverReason.ImpostorsByVote:
                    text += "Impostors by voting out a crewmate";
                    break;
                case GameOverReason.ImpostorsByKill or GameOverReason.HideAndSeek_ImpostorsByKills:
                    text += "Impostors by killing";
                    break;
                case GameOverReason.ImpostorsBySabotage:
                    text += "Impostors by sabotage";
                    break;
                case GameOverReason.CrewmateDisconnect:
                    text += "Impostors by a crewmate disconnect";
                    break;
                case GameOverReason.ImpostorDisconnect:
                    text += "Crewmates by a impostor disconnect";
                    break;
                case GameOverReason.HideAndSeek_CrewmatesByTimer:
                    text += "Crewmates by reaching 0 hide time left";
                    break;
            }
            Utils.Write(text);

            foreach (var killer in KillLogs.ImpKills) Utils.Write($"{killer.Key} killed {killer.Value} players");
        }
    }
}