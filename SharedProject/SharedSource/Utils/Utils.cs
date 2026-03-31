
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
using Barotrauma.Plugins;

using System.Threading.Tasks;

namespace JSForBarotrauma
{
  public static class Utils
  {
    public static IEnumerable<Assembly> AllModAssemblies()
      => PluginLoader.LoadedPlugins.Select(plugin => plugin.Assembly);

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