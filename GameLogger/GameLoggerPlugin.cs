using System.Text;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace GameLogger;

[BepInAutoPlugin("com.whichtwix.gamelogger", "GameLogger", "1.4.0")]
[BepInProcess("Among Us.exe")]

public partial class GameLogger : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public static ManualLogSource Logger { get; set; }

    public static StringBuilder Builder { get; set; } = new();

    public static ConfigEntry<bool> LogVotes { get; set; }

    public override void Load()
    {
        LogVotes = Config.Bind("Settings", "Log Votes", true, "Whether to log votes, this will be spammy");
        Logger = Log;
        Harmony.PatchAll();
    }
}
