using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch]

    public class DeathWatch
    {
        static readonly Dictionary<byte, bool> WasDead = new();
        static readonly Dictionary<byte, RoleTypes> LastRole = new();
        static int frame;
        static bool errored;

        [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
        [HarmonyPostfix]

        public static void Poll()
        {
            if (++frame % 10 != 0) return;

            if (ShipStatus.Instance == null)
            {
                WasDead.Clear();
                LastRole.Clear();
                return;
            }

            try
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player == null || player.Data == null || player.Data.Role == null) continue;

                    var id = player.PlayerId;
                    var dead = player.Data.IsDead;
                    var role = player.Data.Role.Role;
                    var known = WasDead.TryGetValue(id, out var wasDead);

                    if (known && dead && !wasDead && !player.Data.Disconnected
                        && MeetingHud.Instance == null && ExileController.Instance == null)
                    {
                        Utils.Write($"{Utils.FullName(player.Data)} died {Utils.GetLocation(player)}");
                    }

                    if (known && dead && role != RoleTypes.CrewmateGhost && role != RoleTypes.ImpostorGhost
                        && (!LastRole.TryGetValue(id, out var last) || last != role))
                    {
                        Utils.Write($"{Utils.FullName(player.Data)} became {role}");
                    }

                    WasDead[id] = dead;
                    LastRole[id] = role;
                }
            }
            catch (Exception e)
            {
                if (!errored)
                {
                    errored = true;
                    GameLogger.Logger.LogError(e);
                }
            }
        }
    }
}