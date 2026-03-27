
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
    public static  IDebugConsole DebugConsole = PluginServiceProvider.GetService<IDebugConsole>();

    /// <summary>
    /// Do it old fashioned way
    /// </summary>
    public static string ModName = "JS for Barotrauma (DLC testing)";
    public static ContentPackage JSForBarotraumaPackage
      => ContentPackageManager.EnabledPackages.All.First(p => p.Name == ModName);

    public static Mod Instance { get; private set; }
    public static Logger Logger { get; private set; } = new();
    public Harmony Harmony { get; private set; } = new Harmony("JSForBarotrauma");

    public static ConsoleInterface ConsoleInterface => Instance?._consoleInterface;
    public static EngineWrapper Engine => Instance?._engine;

    private ConsoleInterface _consoleInterface;
    private EngineWrapper _engine;

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
      _consoleInterface.AddPatches(Harmony);
    }


    public void OnContentLoaded() { }

    public void Dispose()
    {
      _consoleInterface.RemoveCommands();
      Harmony.UnpatchSelf();
      _engine.Stop();
      DebuggerTracker.Untrack();
      Instance = null;
      DebugConsole = null;
      Logger = null;
    }


  }
}