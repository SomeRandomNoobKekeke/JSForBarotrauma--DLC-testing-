
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
      Engine.AddHostObject("JS", Mod.JS);
      Engine.AddHostObject("Logger", Mod.Logger);
      Engine.AddHostType("Mod", typeof(Mod));
      Engine.AddHostObject("Engine", Engine);

      Engine.AddHostObject("lib", HostItemFlags.PrivateAccess, new HostTypeCollection("mscorlib", "System", "System.Core", "Barotrauma"));


    }

    public void Stop()
    {
      if (Engine == null) return;

      //Engine.Interrupt();
      Engine.Dispose();
      Engine = null;

    }

    public void PrintProps()
    {
      Mod.Logger.Log(BaroJunk.Logger.Wrap.IEnumerable(Engine.Global.PropertyNames));
    }
  }

}