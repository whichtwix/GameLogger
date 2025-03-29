
using HarmonyLib;

namespace GameLogger
{
    [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]

    public class ModStamp
    {
        [HarmonyPostfix]

        public static void Postfix(ModManager __instance)
        {
            __instance.ShowModStamp();
        }
    }
}