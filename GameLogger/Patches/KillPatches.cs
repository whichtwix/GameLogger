using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]

    public class KillLogs
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
        [HarmonyPrefix]

        public static void Prefix(PlayerControl __instance, ref PlayerControl target, ref MurderResultFlags resultFlags)
        {
            if (resultFlags.HasFlag(MurderResultFlags.FailedProtected) || (resultFlags.HasFlag(MurderResultFlags.DecisionByHost) && target.protectedByGuardianId > -1))
            {
                Utils.Write($"{Utils.FullName(__instance.Data)} failed to kill {Utils.FullName(target.Data)} {Utils.GetLocation(target)}");
            }
            else if (resultFlags.HasFlag(MurderResultFlags.Succeeded) || resultFlags.HasFlag(MurderResultFlags.DecisionByHost))
            {
                Utils.Write($"{Utils.FullName(__instance.Data)} killed {Utils.FullName(target.Data)} {Utils.GetLocation(target)}");
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.ProtectPlayer))]
        [HarmonyPostfix]

        public static void Protect(PlayerControl __instance, ref PlayerControl target)
        {
            Utils.Write($"{Utils.FullName(__instance.Data)} set protection on {Utils.FullName(target.Data)}");
        }

        [HarmonyPatch(typeof(NoisemakerRole), nameof(NoisemakerRole.NotifyOfDeath))]
        [HarmonyPostfix]

        public static void NoiseMakerDeath(NoisemakerRole __instance)
        {
            if (!PlayerControl.LocalPlayer.AreCommsAffected())
            {
                Utils.Write($"{Utils.FullName(__instance.Player.Data)} has alerted the lobby of their death");
            }
            else if (PlayerControl.LocalPlayer.AreCommsAffected())
            {
                Utils.Write($"Comms Sabotage stopped {Utils.FullName(__instance.Player.Data)} from alerting the lobby of their death");
            }
        }
    }
}