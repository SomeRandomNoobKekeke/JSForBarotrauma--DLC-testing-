
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
      Engine.AddHostObject("JS", Mod.JS);
      Engine.AddHostObject("Logger", Mod.Logger);
      Engine.AddHostType("Mod", typeof(Mod));
      Engine.AddHostObject("Engine", Engine);

      HostTypeCollection exposedAssemblies = new HostTypeCollection("mscorlib", "System", "System.Core", "Barotrauma");
      exposedAssemblies.AddAssembly(typeof(Mod).Assembly);

      Engine.AddHostObject("lib", HostItemFlags.PrivateAccess, exposedAssemblies);
    }

  }

}