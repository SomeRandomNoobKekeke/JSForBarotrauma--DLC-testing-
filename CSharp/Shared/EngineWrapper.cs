
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

    public bool IsRunning => Engine != null;

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
        AllowReflection = true,
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
      HostObjects.Add(Engine);


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