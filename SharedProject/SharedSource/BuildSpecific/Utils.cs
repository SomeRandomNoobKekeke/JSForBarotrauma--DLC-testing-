
using BaroJunk;
using Barotrauma;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JSForBarotrauma
{


  public static partial class Utils
  {
    public static ContentPackage JSForBarotraumaPackage
    {
      get
      {
        foreach (PluginLoader.LoadedPlugin plugin in PluginLoader.LoadedPlugins)
        {
          if (plugin.Assembly == Assembly.GetExecutingAssembly())
          {
            return plugin.Info.ContentPackage;
          }
        }

        return null;
      }
    }
  }
}