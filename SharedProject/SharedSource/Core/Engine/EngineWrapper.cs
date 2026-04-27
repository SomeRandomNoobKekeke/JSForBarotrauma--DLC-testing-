
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
  public partial class EngineWrapper
  {
    public V8ScriptEngine Engine { get; private set; }

#if CLIENT
    public int DebugPort { get; } = 9222;
#elif SERVER
    public int DebugPort { get; } = 9223;
#endif

    public bool DebuggerAttached { get; set; }

    public bool IsRunning => Engine != null;

    public ClearableEvent OnStop { get; } = new();

    public string SearchPath
    {
      get => Engine.DocumentSettings.SearchPath;
      set => Engine.DocumentSettings.SearchPath = value;
    }

    public ScriptLoader ScriptLoader { get; private set; }
    public JSCommandManager JSCommandManager { get; private set; } = new();
    public NetManager NetManager { get; private set; } = new();
    public XMLHookManager XMLHookManager { get; private set; } = new();

#if CLIENT
    public ServerManager ServerManager { get; private set; } = new();
#endif

    public void Start()
    {
      if (DebuggerAttached)
      {
        Mod.Logger.Error($"Tried to launch new engine before detaching debugger");
        return;
      }

      Mod.LoadTimeTracker.Start("Engine creation");
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
      Mod.LoadTimeTracker.Stop("Engine creation");

      Mod.LoadTimeTracker.Start("Exposing stuff");
      ExposeStuff();
      Mod.LoadTimeTracker.Stop("Exposing stuff");

      Mod.LoadTimeTracker.Start("NetManager.Init()");
      NetManager.Init();
      Mod.LoadTimeTracker.Stop("NetManager.Init()");

      Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Started", "White")));

      Mod.LoadTimeTracker.Start("Script loading");
      ScriptLoader.LoadScripts();
      Mod.LoadTimeTracker.Stop("Script loading");
    }

    public void Reload()
    {
      Utils.RunWithDelay(() =>
      {
        Mod.Engine.Stop();

        Utils.RunWithDelay(() => Mod.Engine.Start());
      });
    }

    public void Stop()
    {
      try
      {
        if (Engine == null) return;

        OnStop.Raise();
        OnStop.Clear();


        JSHook.Clear();
        JSCommandManager.Clear();
        XMLHookManager.Clear();
#if CLIENT
        ServerManager.Clear();
#endif

        NetManager.Dispose();

        Engine.Interrupt();
        Engine.Dispose();
        Engine = null;

        DocumentLoader.Default.DiscardCachedDocuments();

        Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Stopped", "White")));
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"Exception in JS.Stop: {e}");
      }
      finally
      {
        if (Engine != null)
        {
          Engine.Interrupt();
          Engine.Dispose();
          Engine = null;
        }
      }
    }

    public EngineWrapper()
    {
      ScriptLoader = new(this);
    }
  }

}