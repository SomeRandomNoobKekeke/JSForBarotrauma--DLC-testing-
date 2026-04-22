
using BaroJunk;
using Barotrauma;
using Barotrauma.Items.Components;
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
  public partial class Mod
  {
    public static Mod Instance { get; private set; }
    public static Logger Logger { get; private set; } = new()
    {
      PrintFilePath = false
    };
    public static ConsoleInterface ConsoleInterface { get; private set; }
    public static EngineWrapper Engine { get; private set; }
    public static Harmony Harmony { get; private set; } = new Harmony("JSForBarotrauma");
    public static DebuggerTracker DebuggerTracker { get; private set; } = new();


    public void Init()
    {
      Instance = this;

      InitBuildSpecific();

      Engine = new();
      ConsoleInterface = new(Engine);
      ConsoleInterface.AddPatches(Harmony);

      ConsoleInterface.AddCommands();
      DebuggerTracker.Track();

      Engine.Start();
      // Utils.PrintAllPatchedMethods();
    }


    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public partial void InitBuildSpecific();
    public partial void DisposeBuildSpecific();

    public void Dispose()
    {
      ConsoleInterface.RemoveCommands();
      ConsoleInterface = null;

      Engine.Stop();
      Engine = null;

      Harmony.UnpatchSelf();

      DebuggerTracker.Untrack();
      DebuggerTracker = null;

      DisposeBuildSpecific();

      Instance = null;
    }


  }
}