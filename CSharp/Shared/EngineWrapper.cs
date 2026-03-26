
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Threading;
using BaroJunk;
namespace JSForBarotrauma
{
  public partial class EngineWrapper
  {
    public V8ScriptEngine Engine { get; private set; }
    public int DebugPort { get; } = 9222;
    public bool DebuggerAttached { get; set; }

    public bool IsRunning => Engine != null;

    public string SearchPath
    {
      get => Engine.DocumentSettings.SearchPath;
      set => Engine.DocumentSettings.SearchPath = value;
    }

    public ScriptLoader ScriptLoader { get; private set; }

    public void Start()
    {
      if (DebuggerAttached)
      {
        Mod.Logger.Error($"Tried to launch new engine before detaching debugger");
        return;
      }

      Engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging, DebugPort)
      {
        AccessContext = typeof(GameMain),
        AllowReflection = true,
        ExposeHostObjectStaticMembers = true,
        DisableTypeRestriction = true,
        DocumentSettings = new DocumentSettings()
        {
          AccessFlags = DocumentAccessFlags.EnableAllLoading,
        },
      };

      ExposeStuff();

      ScriptLoader.LoadScripts();

      Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Started", "White")));
    }

    public void Reload()
    {
      GameMain.LuaCs.Timer.Wait((args) =>
      {
        Mod.Engine.Stop();

        GameMain.LuaCs.Timer.Wait((args) =>
        {
          Mod.Engine.Start();
        }, 100);
      }, 100);
      // Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Reloaded", "White")));
    }

    public void Stop()
    {
      if (Engine == null) return;

      JS.StopEvent.Raise();
      JS.StopEvent.Clear();

      Mod.Logger.Log($"Engine.Interrupt");
      Engine.Interrupt();
      Mod.Logger.Log($"Engine.Dispose");
      Engine.Dispose();
      Mod.Logger.Log($"Engine = null");
      Engine = null;
      DocumentLoader.Default.DiscardCachedDocuments();

      Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Stopped", "White")));
    }

    public void PrintProps()
    {
      Mod.Logger.Log(Logger.Wrap.IEnumerable(Engine.Global.PropertyNames));
    }

    public EngineWrapper()
    {
      JS = new(this);
      ScriptLoader = new(this);
    }
  }

}