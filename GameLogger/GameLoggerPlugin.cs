using System.Text;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace GameLogger;

[BepInAutoPlugin("com.whichtwix.gamelogger", "GameLogger", "1.1.0")]
[BepInProcess("Among Us.exe")]

public partial class GameLogger : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public static StringBuilder Builder { get; set; } = new();

    public override void Load()
    {
        Harmony.PatchAll();
    }
}
