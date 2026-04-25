
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
using System.Runtime.CompilerServices;

namespace JSForBarotrauma
{
  public partial class EngineWrapper
  {
    private void ExposeStuff()
    {
      AddExtraObjects();
    }

    public HostFunctions HostFunctions { get; private set; }
    public ExtendedHostFunctions ExtendedHostFunctions { get; private set; }

    private void AddExtraObjects()
    {
      HostFunctions = new();
      ExtendedHostFunctions = new();

      Engine.AddHostObject("host", HostFunctions);
      Engine.AddHostObject("xHost", ExtendedHostFunctions);

      Engine.AddHostType("JS", typeof(JS));
      Engine.AddHostType("Console", typeof(UnifiedConsole));
      Engine.AddHostType("ModInfo", typeof(ModInfo));
      Engine.AddHostType("ObjectExtentions", typeof(HiddenNamespace.ObjectExtentions));


      Engine.AddHostObject("API", API.ToBag());

      Engine.Global["setTimeout"] = (object scriptFunc, int delay) =>
      {
        Utils.RunWithDelay(HostFunctions.del<Action>(scriptFunc), delay);
        return (object)null; //TODO here should be a cancelation token
      };

      HostTypeCollection exposedAssemblies = new HostTypeCollection();
      exposedAssemblies.AddAssembly("mscorlib", NoExtensionTypes);
      exposedAssemblies.AddAssembly("System", NoExtensionTypes);
      exposedAssemblies.AddAssembly("System.Core", NoExtensionTypes);
      // exposedAssemblies.AddType(typeof(System.Array));


      exposedAssemblies.AddAssembly(typeof(GameMain).Assembly, NoExtensionTypes);
      exposedAssemblies.AddAssembly(typeof(Harmony).Assembly, NoExtensionTypes);
      exposedAssemblies.AddAssembly(typeof(Vector2).Assembly, NoExtensionTypes);
      exposedAssemblies.AddAssembly(typeof(Steamworks.SteamFriends).Assembly, NoExtensionTypes);

      Engine.AddHostObject("lib", HostItemFlags.PrivateAccess, exposedAssemblies);
    }

    public static bool NoExtensionTypes(Type T) => !IsExtensionType(T);
    public static bool IsExtensionType(Type T)
      => T.IsSealed && !T.IsGenericType && !T.IsNested &&
         T.GetMethods(BindingFlags.Static | BindingFlags.Public).Any(mi =>
           mi.IsDefined(typeof(ExtensionAttribute), false)
         );



    public static void PrintAllExtensionClasses(Assembly assembly)
    {
      foreach (Type T in assembly.GetTypes())
      {
        if (IsExtensionType(T))
        {
          Mod.Logger.Log($"extension class: [{T}]");
        }
      }
    }
  }

}