
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
    public static Mod Instance { get; private set; }
    //BRUH mb use actual package?
    public static string PackageName;
    public static Logger Logger { get; } = new();
    public Harmony Harmony { get; } = new Harmony("JSForBarotrauma");

    public static ConsoleInterface ConsoleInterface => Instance?._consoleInterface;
    public static EngineWrapper Engine => Instance?._engine;
    public static ScriptLoader ScriptLoader => Instance?._scriptLoader;
    public static JS JS => Instance?._js;

    private JS _js;
    private ScriptLoader _scriptLoader;
    private ConsoleInterface _consoleInterface;
    private EngineWrapper _engine;



    public void Initialize()
    {
      Instance = this;
      PackageName = ModInfo.ModName<Mod>();

      _js = new JS();
      _engine = new();
      _consoleInterface = new(_engine);
      _scriptLoader = new ScriptLoader(_engine);

      V8Runtime.DebuggerConnected += (sender, args) => { };
      V8Runtime.DebuggerDisconnected += (sender, args) => JS.Reload();

      Engine.Start();


      PatchAll();

      ConsoleInterface.AddCommands();
      ScriptLoader.LoadScripts();
    }


    public void PatchAll()
    {
      ConsoleInterface.AddPatches(Harmony);
    }


    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      ConsoleInterface.RemoveCommands();
      Harmony.UnpatchSelf();
      Engine.Stop();
      Instance = null;
    }


  }
}