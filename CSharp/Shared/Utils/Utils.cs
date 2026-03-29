
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Runtime.CompilerServices;
using System.IO;
using BaroJunk;

using System.Threading.Tasks;

namespace JSForBarotrauma
{
  public static class Utils
  {
    // Barotrauma.Plugins compatibility
    public static IEnumerable<Assembly> GetPackageAssemblies(ContentPackage package)
    {
      CsPackageManager _ = GameMain.LuaCs.PluginPackageManager;

      if (!_._loadedCompiledPackageAssemblies.Keys.Contains(package)) yield break;

      Guid guid = _._loadedCompiledPackageAssemblies[package];

      foreach (Type T in _._pluginTypes[guid])
      {
        yield return T.Assembly;
      }
    }

    public static IEnumerable<Assembly> AllModAssemblies()
    {
      CsPackageManager _ = GameMain.LuaCs.PluginPackageManager;

      foreach (ImmutableHashSet<Type> set in _._pluginTypes.Values)
      {
        foreach (Type T in set)
        {
          yield return T.Assembly;
        }
      }
    }

    public static void RunWithDelay(Action action, int delay = 100)
    {
      Task.Delay(delay).ContinueWith((t) => action());
    }

    // i don't get how to use it
    // public static void RunWithDelay(Action action, float delay = 100)
    // {
    //   CoroutineManager.Invoke(action, delay);
    // }

    // rip
    // public static void RunWithDelay(Action action, double delay = 100)
    // {
    //   GameMain.LuaCs.Timer.Wait((args) =>
    //   {
    //     action();
    //   }, 100);
    // }
  }
}