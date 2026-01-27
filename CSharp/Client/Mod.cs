
using System;
using System.Reflection;
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
    public static Logger Logger { get; } = new();
    public Harmony Harmony { get; } = new Harmony("JSForBarotrauma");

    public static ConsoleInterface ConsoleInterface => Instance?._consoleInterface;
    public static EngineWrapper Engine => Instance?._engine;
    public static ScriptLoader ScriptLoader => Instance?._scriptLoader;


    private ScriptLoader _scriptLoader;
    private ConsoleInterface _consoleInterface;
    private EngineWrapper _engine;

    public void Initialize()
    {
      Instance = this;

      _engine = new();
      _consoleInterface = new(_engine);
      _scriptLoader = new ScriptLoader(_engine);

      Engine.Start();

      PatchAll();

      ConsoleInterface.AddCommands();
      ScriptLoader.LoadScriptsFromMod(ModInfo.ModDir<Mod>());
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