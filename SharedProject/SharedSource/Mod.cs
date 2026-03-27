
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Barotrauma;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Runtime.CompilerServices;
using System.IO;
using BaroJunk;


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

    public static PluginServices PluginServices { get; private set;} = new();

    /// <summary>
    /// Do it old fashioned way
    /// </summary>
    public static string ModName = "JS for Barotrauma (DLC testing)";
    public static ContentPackage JSForBarotraumaPackage
      => ContentPackageManager.EnabledPackages.All.First(p => p.Name == ModName);

    public static Mod Instance { get; private set; }
    public static Logger Logger { get; private set; } = new();
    //public static Harmony Harmony => Instance?._harmony;
    public static ConsoleInterface ConsoleInterface => Instance?._consoleInterface;
    public static EngineWrapper Engine => Instance?._engine;




    private ConsoleInterface _consoleInterface;
    private EngineWrapper _engine;
    //private Harmony _harmony = new Harmony("JSForBarotrauma");

    private DebuggerTracker DebuggerTracker { get; } = new();


    public void Init()
    {
      Instance = this;

      _engine = new();
      _consoleInterface = new(_engine);

      PatchAll();
      _consoleInterface.AddCommands();

      DebuggerTracker.Track();

      _engine.Start();
    }


    public void PatchAll()
    {
      //_consoleInterface.AddPatches(Harmony);
    }


    public void OnContentLoaded() { }

    public void Dispose()
    {
      _consoleInterface.RemoveCommands();
      //_harmony.UnpatchSelf();
      _engine.Stop();
      DebuggerTracker.Untrack();
      Instance = null;


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