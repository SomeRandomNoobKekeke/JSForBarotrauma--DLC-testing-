
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

namespace JSForBarotrauma
{
  public partial class Mod : IAssemblyPlugin
  {
    public static Mod Instance { get; private set; }
    public static Logger Logger { get; } = new();
    public Harmony Harmony { get; } = new Harmony("jsforbarotrauma");

    public static JS JS => Mod.Instance?.js;
    private JS js;


    public void Initialize()
    {
      Instance = this;
      js = new();

      PatchAll();

      AddCommands();
      js.Engine.ExecuteDocument(new DocumentInfo { Category = ModuleCategory.Standard }, "Autorun/lol.js");
    }


    public void PatchAll()
    {
      LuaGamePatch.PatchClientLuaGame(Harmony);
      DebugConsolePatch.PatchClientDebugConsole(Harmony);
    }


    public void OnLoadCompleted() { }
    public void PreInitPatching() { }

    public void Dispose()
    {
      RemoveCommands();
      Harmony.UnpatchSelf();
      JS.Stop();
      Instance = null;
    }


  }
}