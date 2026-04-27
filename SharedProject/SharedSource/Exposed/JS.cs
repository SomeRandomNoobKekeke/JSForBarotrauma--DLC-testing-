
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Threading;
using BaroJunk;


namespace JSForBarotrauma
{
  public static class JS
  {
    public static V8ScriptEngine Engine => Mod.Engine?.Engine;
    public static ScriptObject Global => Engine?.Global;
    public static event Action OnStop
    {
      add => Mod.Engine.OnStop.Add(value);
      remove => Mod.Engine.OnStop.Remove(value);
    }

    // Just to check added extension props
    public static object EmptyObject => new object();

    public static bool REPL
    {
      get => Mod.ConsoleInterface.REPL;
      set => Mod.ConsoleInterface.REPL = value;
    }

    public static void ReloadLua()
    {
      DebugConsole.ExecuteCommand("cl_reloadlua");
    }
    public static void Reload()
    {
      Utils.RunWithDelay(() => Mod.Engine?.Reload());
    }
    public static void Stop()
    {
      Utils.RunWithDelay(() => Mod.Engine?.Stop());
    }
    public static void Start()
    {
      Mod.Engine?.Start();
    }
  }
}