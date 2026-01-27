
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

namespace JSForBarotrauma
{
  public class EngineWrapper
  {
    public V8ScriptEngine Engine { get; private set; }
    public int DebugPort { get; } = 9222;

    /// <summary>
    /// If you restart without delay attached debugger won't detachs
    /// Can't find a proper way to detach it
    /// </summary>
    public int RestartDelay { get; } = 1000;

    public string SearchPath
    {
      get => Engine.DocumentSettings.SearchPath;
      set => Engine.DocumentSettings.SearchPath = value;
    }

    public void Start()
    {

      Engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging, DebugPort)
      {
        AccessContext = typeof(GameMain),
        ExposeHostObjectStaticMembers = true,
        DisableTypeRestriction = true,
        DocumentSettings = new DocumentSettings()
        {
          AccessFlags = DocumentAccessFlags.EnableAllLoading,
        },
      };

      Load();
    }

    public void Load()
    {
      AddHostObjects();
    }



    public void AddHostObjects()
    {
      Engine.AddHostObject("Logger", Mod.Logger);
      Engine.AddHostObject("Engine", Engine);

      Engine.AddHostObject("lib", HostItemFlags.PrivateAccess, new HostTypeCollection("System", "Barotrauma", "mscorlib", "System.Core"));
    }

    public void Stop()
    {
      if (Engine == null) return;

      // Engine.Dispose(); Don't
      Engine = null;
    }
    public void Restart()
    {
      Stop();
      Start();
    }

    public void PrintProps()
    {
      Mod.Logger.Log(BaroJunk.Logger.Wrap.IEnumerable(Engine.Global.PropertyNames));
    }

    // public void Reload() { Clear(); Load(); }

    // This is not enough, can't find any gracefull method to clear all loaded stuff
    // public void Clear()
    // {
    //   foreach (string name in Engine.Global.PropertyNames)
    //   {
    //     Engine.Global.DeleteProperty(name);
    //   }

    //   var moduleCache = typeof(CommonJSManager).GetField("moduleCache", AccessTools.all).GetValue(typeof(V8ScriptEngine).GetField("commonJSManager", AccessTools.all).GetValue(Engine));

    //   moduleCache.GetType().GetMethod("Clear").Invoke(moduleCache, new object[] { });
    // }
  }

}