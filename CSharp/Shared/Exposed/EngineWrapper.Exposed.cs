
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
    public JS_Link JS { get; private set; }

    private void ExposeStuff()
    {
      AddExtraObjects();
    }

    private void AddExtraObjects()
    {
      Engine.AddHostObject("host", new HostFunctions());
      Engine.AddHostObject("xHost", new ExtendedHostFunctions());

      Engine.AddHostObject("JS", JS);

      Engine.AddHostType("JSHook", typeof(JSHookExposed));
      Engine.AddHostType("Console", typeof(UnifiedConsole));
      // Engine.AddHostObject("Hook", GameMain.LuaCs.Hook);
      //Engine.AddHostType("DebugConsole", HostItemFlags.PrivateAccess, typeof(DebugConsole));

      HostTypeCollection exposedAssemblies = new HostTypeCollection("mscorlib", "System", "System.Core", "Barotrauma");
      // exposedAssemblies.AddAssembly(typeof(Mod).Assembly);
      exposedAssemblies.AddAssembly(typeof(Harmony).Assembly);
      exposedAssemblies.AddAssembly(typeof(Vector2).Assembly);
      exposedAssemblies.AddAssembly(typeof(List<int>).Assembly);

      Engine.AddHostObject("lib", HostItemFlags.PrivateAccess, exposedAssemblies);


      //FIXME it's borked, if two mods have same namespace it won't be added
      HostTypeCollection modAssemblies = new HostTypeCollection();
      foreach (Assembly assembly in Utils.AllModAssemblies())
      {
        try
        {
          modAssemblies.AddAssembly(assembly);
        }
        catch (InvalidOperationException e)
        {
          Mod.Logger.Warning($"Couldn't add [{assembly}] to modAssemblies");
        }
      }
      Engine.AddHostObject("modlib", HostItemFlags.PrivateAccess, modAssemblies);

      // Engine.Execute("function Throw(e){ throw e }");
    }
  }

}