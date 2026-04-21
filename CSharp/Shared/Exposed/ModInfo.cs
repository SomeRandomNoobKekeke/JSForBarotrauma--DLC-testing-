
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
using System.IO;
namespace JSForBarotrauma
{

  public static class ModInfo
  {
    public static string FullPath => Path.Combine(
      Path.GetDirectoryName(Environment.ProcessPath),
      Package.Dir
    );

    public static ContentPackage Package => Mod.Engine?.ScriptLoader.CurrentPackage;
  }
}