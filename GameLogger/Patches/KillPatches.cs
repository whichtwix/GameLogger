using System.Collections.Generic;
using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]

    public class KillLogs
    {
        public static Dictionary<string, int> ImpKills = [];

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
        [HarmonyPrefix]

        public static void Prefix(PlayerControl __instance, ref PlayerControl target, ref MurderResultFlags resultFlags)
        {
            var killer = Utils.FullName(__instance.Data);
            var victim = Utils.FullName(target.Data);
            var location = Utils.GetLocation(target);

            if (resultFlags.HasFlag(MurderResultFlags.FailedProtected) || (resultFlags.HasFlag(MurderResultFlags.DecisionByHost) && target.protectedByGuardianId > -1))
            {
                var ga = Utils.GetPlayer(target.protectedByGuardianId);
                Utils.Write($"{killer} failed to kill {victim} {location} due to protection by {Utils.FullName(ga)}");
            }
            else if (resultFlags.HasFlag(MurderResultFlags.Succeeded) || resultFlags.HasFlag(MurderResultFlags.DecisionByHost))
            {
                Utils.Write($"{killer} killed {victim} {location}");

                if (ImpKills.TryGetValue(killer, out int kills))
                {
                    ImpKills[killer] = kills + 1;
                }
                else
                {
                    ImpKills.Add(killer, 1);
                }
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
            var noisemaker = Utils.FullName(__instance.Player.Data);
            
            if (!PlayerControl.LocalPlayer.AreCommsAffected())
            {
                Utils.Write($"{noisemaker} has alerted the lobby of their death");
            }
            else if (PlayerControl.LocalPlayer.AreCommsAffected())
            {
                Utils.Write($"Comms Sabotage stopped {noisemaker} from alerting the lobby of their death");
            }
        }
    }
}