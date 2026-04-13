
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
using System.Collections.Concurrent;

using System.Threading.Tasks;

namespace JSForBarotrauma
{


  public static partial class Utils
  {
    public static ContentPackage JSForBarotraumaPackage => Mod.Package;

    public static ConcurrentDictionary<Type, ContentPackage> PluginPackageLookup
      => (Mod.Instance.PluginService as PluginManagementService)._pluginPackageLookup;

    public static IEnumerable<Assembly> AllModAssemblies()
      => PluginPackageLookup.Keys.Select(t => t.Assembly);
  }
}