using System.Text;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace GameLogger;

[BepInAutoPlugin("com.whichtwix.gamelogger", "GameLogger", "1.3.0")]
[BepInProcess("Among Us.exe")]

public partial class GameLogger : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public static ManualLogSource Logger { get; set; }

    public static StringBuilder Builder { get; set; } = new();

    public override void Load()
    {
        Logger = Log;
        Harmony.PatchAll();
    }
}
