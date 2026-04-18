
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
    private void ExposeStuff()
    {
      AddExtraObjects();
    }

    private void AddExtraObjects()
    {
      Engine.AddHostObject("host", new HostFunctions());
      Engine.AddHostObject("xHost", new ExtendedHostFunctions());

      Engine.AddHostType("JS", typeof(JS));
      Engine.AddHostType("Game", typeof(JS_Game));
      Engine.AddHostType("JSHook", typeof(JSHookExposed));
      Engine.AddHostType("Console", typeof(UnifiedConsole));


      Engine.AddHostType("API.Web", typeof(WebAPI));
      // HostTypeCollection APICollection = new HostTypeCollection();
      // APICollection.AddType(typeof(WebAPI));
      // Engine.AddHostObject("API", HostItemFlags.PrivateAccess, APICollection);

      HostTypeCollection exposedAssemblies = new HostTypeCollection("mscorlib", "System", "System.Core");

#if CLIENT
      exposedAssemblies.AddAssembly("Barotrauma");
#elif SERVER
      exposedAssemblies.AddAssembly("DedicatedServer");
#endif
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