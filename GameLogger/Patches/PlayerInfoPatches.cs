using AmongUs.GameOptions;
using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]

    public class PlayerDataLogs
    {
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetTasks))]
        [HarmonyPostfix]

        public static void Postfix(PlayerControl __instance)
        {
            if (PlayerControl.LocalPlayer.PlayerId != __instance.PlayerId) return;
            
            string roletext = "Player's Roles:\n";
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                roletext += $"{Utils.FullName(player.Data)} : {player.Data.Role.Role}\n";
            }
            Utils.Write(roletext);
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CoSetRole))]
        [HarmonyPostfix]

        public static void CheckGuardianAngel(PlayerControl __instance, ref RoleTypes role)
        {
            if (role is RoleTypes.GuardianAngel)
            {
                string text = $"{Utils.FullName(__instance.Data)}, became guardian angel";
                Utils.Write(text);
            }
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.HandleDisconnect), typeof(PlayerControl), typeof(DisconnectReasons))]
        [HarmonyPostfix]

        public static void Postfix(ref PlayerControl player)
        {
            if (player.Data.Disconnected && !player.Data.IsDead) 
            {
                Utils.Write($"{Utils.FullName(player.Data)} disconnected");
            }
        }
    }
}