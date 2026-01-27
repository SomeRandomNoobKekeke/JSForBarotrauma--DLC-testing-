
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
        // AccessContext = typeof(GameMain),
        ExposeHostObjectStaticMembers = true,
        DisableTypeRestriction = true,
        DocumentSettings = new DocumentSettings()
        {
          AccessFlags = DocumentAccessFlags.EnableAllLoading,
        },
      };

      AddHostObjects();
    }

    public void AddHostObjects()
    {
      Engine.AddHostObject("Logger", Mod.Logger);
      Engine.AddHostObject("lib", HostItemFlags.PrivateAccess, new HostTypeCollection("System", "Barotrauma", "mscorlib", "System.Core"));
      Engine.AddHostType(HostItemFlags.PrivateAccess, typeof(GameMain));
      Engine.AddHostType(HostItemFlags.PrivateAccess, typeof(LuaCsSetup));

      // Engine.AddHostObject("Engine", Engine);

      Engine.AddHostObject("JS", this);
    }

    public void Stop() { Engine?.Dispose(); }
    public void Restart() { Stop(); Start(); }
  }

}