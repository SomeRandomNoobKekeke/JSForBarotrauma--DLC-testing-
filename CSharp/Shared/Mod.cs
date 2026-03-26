
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Runtime.CompilerServices;
using System.IO;
using BaroJunk;
using FluentResults;

namespace JSForBarotrauma
{
  public partial class Mod : IAssemblyPlugin
  {
    /// <summary>
    /// Do it old fashioned way
    /// </summary>
    public static string ModName = "JS for Barotrauma [code]";
    public static ContentPackage JSForBarotraumaPackage
      => ContentPackageManager.EnabledPackages.All.First(p => p.Name == ModName);

    public static Mod Instance { get; private set; }
    public static Logger Logger { get; } = new();
    public Harmony Harmony { get; } = new Harmony("JSForBarotrauma");

    public static ConsoleInterface ConsoleInterface => Instance?._consoleInterface;
    public static EngineWrapper Engine => Instance?._engine;

    private ConsoleInterface _consoleInterface;
    private EngineWrapper _engine;

    private DebuggerTracker DebuggerTracker { get; } = new();


    public void Initialize()
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


    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      _consoleInterface.RemoveCommands();
      Harmony.UnpatchSelf();
      _engine.Stop();
      DebuggerTracker.Untrack();
      Instance = null;
    }


  }
}