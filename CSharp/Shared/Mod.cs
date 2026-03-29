
using BaroJunk;
using Barotrauma;
using Barotrauma.Items.Components;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace JSForBarotrauma
{
  public partial class Mod : IBarotraumaPlugin
  {
    // if i get this correctly they are injected only into static var on IBarotraumaPlugin, for whatever reason
    public static IDebugConsole DebugConsole = PluginServiceProvider.GetService<IDebugConsole>();
    public static ISettingsService SettingsService = PluginServiceProvider.GetService<ISettingsService>();
    public static IItemComponentRegistrar ItemComponentRegistrar = PluginServiceProvider.GetService<IItemComponentRegistrar>();
    public static ISimpleHookService HookService = PluginServiceProvider.GetService<ISimpleHookService>();
    public static IHarmonyProvider HarmonyProvider = PluginServiceProvider.GetService<IHarmonyProvider>();
    public static IContentFileRegistrar ContentFileRegistrar = PluginServiceProvider.GetService<IContentFileRegistrar>();
    public static IGameNetwork GameNetwork = PluginServiceProvider.GetService<IGameNetwork>();
    public static IStatusEffectService StatusEffectService = PluginServiceProvider.GetService<IStatusEffectService>();

    public static PluginServices PluginServices { get; private set; } = new();

    /// <summary>
    /// Do it old fashioned way
    /// </summary>
    // public static string ModName = "JS for Barotrauma (DLC testing)";
    // Barotrauma.Plugins compatibility
    public static string ModName = "JS for Barotrauma [code]";
    public static ContentPackage JSForBarotraumaPackage
      => ContentPackageManager.EnabledPackages.All.First(p => p.Name == ModName);


    public static Logger Logger { get; private set; } = new();
    public static ConsoleInterface ConsoleInterface { get; private set; }
    public static EngineWrapper Engine { get; private set; }
    public static Harmony Harmony { get; private set; }
    public static DebuggerTracker DebuggerTracker { get; private set; } = new();

    public void Initialize() => Init();
    public void Init()
    {
      Engine = new();
      ConsoleInterface = new(Engine);
      Harmony = HarmonyProvider.GetHarmony();

      ConsoleInterface.AddCommands();
      DebuggerTracker.Track();

      Engine.Start();
    }




    public void PatchAll()
    {
      //_consoleInterface.AddPatches(Harmony);
    }


    public void OnContentLoaded() { }
    public void OnLoadCompleted() { }
    public void PreInitPatching() { }


    public void Dispose()
    {
      ConsoleInterface.RemoveCommands();
      ConsoleInterface = null;

      Engine.Stop();
      Engine = null;

      Harmony.UnpatchSelf();
      Harmony = null;


      DebuggerTracker.Untrack();
      DebuggerTracker = null;

      DebugConsole = null;
      SettingsService = null;
      ItemComponentRegistrar = null;
      HookService = null;
      HarmonyProvider = null;
      ContentFileRegistrar = null;
      GameNetwork = null;
      StatusEffectService = null;

      PluginServices = null;
    }


  }
}