
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

      Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Started", "White")));

      ScriptLoader.LoadScripts();
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
      if (Engine == null) return;

      JS.OnStop.Raise();
      JS.OnStop.Clear();

      Engine.Interrupt();
      Engine.Dispose();
      Engine = null;

      DocumentLoader.Default.DiscardCachedDocuments();

      JSHook.Clear();

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