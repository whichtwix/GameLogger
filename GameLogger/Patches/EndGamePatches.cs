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
                case GameOverReason.HumansByVote:
                    text += "Crewmates by voting out Impostors";
                    break;
                case GameOverReason.HumansByTask:
                    text += "Crewmates by task win";
                    break;
                case GameOverReason.ImpostorByVote:
                    text += "Impostors by voting out a crewmate";
                    break;
                case GameOverReason.ImpostorByKill or GameOverReason.HideAndSeek_ByKills:
                    text += "Impostors by killing";
                    break;
                case GameOverReason.ImpostorBySabotage:
                    text += "Impostors by sabotage";
                    break;
                case GameOverReason.HumansDisconnect:
                    text += "Impostors by a crewmate disconnect";
                    break;
                case GameOverReason.ImpostorDisconnect:
                    text += "Crewmates by a impostor disconnect";
                    break;
                case GameOverReason.HideAndSeek_ByTimer:
                    text += "Crewmates by reaching 0 hide time left";
                    break;
            }
            Utils.Write(text);
        }
    }
}