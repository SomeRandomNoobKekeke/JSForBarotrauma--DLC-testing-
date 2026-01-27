
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
  public static class HostObjects
  {

    public static void Add(V8ScriptEngine Engine)
    {
      Engine.AddHostObject("host", new HostFunctions());
      Engine.AddHostObject("xHost", new ExtendedHostFunctions());

      Engine.AddHostObject("JS", Mod.JS);
      Engine.AddHostObject("Logger", Mod.Logger);
      Engine.AddHostType("Console", typeof(UnifiedConsole));
      Engine.AddHostObject("Hook", GameMain.LuaCs.Hook);
      Engine.AddHostType("DebugConsole", HostItemFlags.PrivateAccess, typeof(DebugConsole));


      HostTypeCollection exposedAssemblies = new HostTypeCollection("mscorlib", "System", "System.Core", "Barotrauma");
      exposedAssemblies.AddAssembly(typeof(Mod).Assembly);
      exposedAssemblies.AddAssembly(typeof(Harmony).Assembly);
      exposedAssemblies.AddAssembly(typeof(Vector2).Assembly);

      Engine.AddHostObject("lib", HostItemFlags.PrivateAccess, exposedAssemblies);

      // Engine.Execute("function Throw(e){ throw e }");
    }

  }

}