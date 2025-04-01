using HarmonyLib;
using Hazel;

namespace GameLogger
{
    [HarmonyPatch]
    
    public class SabotageLogs
    {
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.UpdateSystem), typeof(SystemTypes), typeof(PlayerControl), typeof(MessageReader))]
        [HarmonyPostfix]

        public static void Start(ref SystemTypes systemType, ref PlayerControl player)
        {
            string sabtext = "Sabotage started: ";
            switch (systemType)
            {
                case SystemTypes.Reactor or SystemTypes.Laboratory:
                    sabtext += "Reactor";
                    break;
                case SystemTypes.Electrical:
                    sabtext += "Lights";
                    break;
                case SystemTypes.LifeSupp:
                    sabtext += "Oxygen";
                    break;
                case SystemTypes.Comms:
                    sabtext += "Comms";
                    break;
                case SystemTypes.HeliSabotage:
                    sabtext += "Heli";
                    break;
                case SystemTypes.MushroomMixupSabotage:
                    sabtext += "Mushroom Mixup";
                    break;
            }
            sabtext += $" by {Utils.FullName(player.Data)}";
            Utils.Write(sabtext);
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RemoveTask))]
        [HarmonyPostfix]

        public static void Postfix(ref PlayerTask task)
        {
            switch (task.TaskType)
            {
                case TaskTypes.ResetReactor:
                case TaskTypes.ResetSeismic:
                case TaskTypes.RestoreOxy:
                case TaskTypes.FixComms:
                case TaskTypes.FixLights:
                case TaskTypes.StopCharles:
                case TaskTypes.MushroomMixupSabotage:
                    Utils.Write("Sabotage ended / fixed");
                    break;

            }
        }
    }
}