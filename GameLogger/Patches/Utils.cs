using System;
using HarmonyLib;
using UnityEngine;

namespace GameLogger
{
    public class Utils
    {
        public static void Write(params string[] parts)
        {
            string info = "";
            parts.Do(x => info += $"{x}\n");
            GameLogger.Builder.AppendJoin("\n", $"[{DateTime.Now}]", info, "");
        }

        public static string GetMap()
        {
            if (GameOptionsManager.Instance.currentGameMode == AmongUs.GameOptions.GameModes.HideNSeek)
            {
                return ((MapNames)GameOptionsManager.Instance.currentHideNSeekGameOptions.MapId).ToString();
            }
            return ((MapNames)GameOptionsManager.Instance.currentNormalGameOptions.MapId).ToString();
        }

        public static string FullName(NetworkedPlayerInfo player)
        {
            return $"{player.PlayerName} {player.ColorName}";
        }

        public static NetworkedPlayerInfo GetPlayer(int playerid)
        {
            foreach (var player in GameData.Instance.AllPlayers)
            {
                if (player.PlayerId == playerid) return player;
            }

            return null;
        }

        public static string GetLocation(PlayerControl player)
        {
            HudManager.Instance.InitMap();
            var Instance = MapBehaviour.Instance.countOverlay;

            for (int i = 0; i < Instance.CountAreas.Length; i++)
            {
                CounterArea counterArea = Instance.CountAreas[i];
                if (ShipStatus.Instance.FastRooms.TryGetValue(counterArea.RoomType, out PlainShipRoom plainShipRoom) && plainShipRoom.roomArea)
                {
                    int num = plainShipRoom.roomArea.OverlapCollider(Instance.filter, Instance.buffer);
                    for (int j = 0; j < num; j++)
                    {
                        Collider2D collider2D = Instance.buffer[j];
                        if (!collider2D.isTrigger)
                        {
                            PlayerControl component2 = collider2D.GetComponent<PlayerControl>();
                            if (component2?.PlayerId == player.PlayerId)
                            {
                                return $"at {TranslationController.Instance.GetString(counterArea.RoomType)}";
                            }
                        }
                    }
                }
            }

            return "outside / in a hallway";
        }
    }
}