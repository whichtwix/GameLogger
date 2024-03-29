using System.Text;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;

namespace GameLogger;

[BepInAutoPlugin("com.whichtwix.gamelogger", "GameLogger", "1.0.0")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
public partial class GameLogger : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    public static StringBuilder Builder { get; set; } = new();

    public override void Load()
    {
        Harmony.PatchAll();
    }
}
