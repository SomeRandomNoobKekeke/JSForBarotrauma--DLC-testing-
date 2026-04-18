
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
  //Why context, i thought mb also expose settings, permissions other metadata here
  public static class PackageContext
  {
    public static ContentPackage Package => Mod.Engine?.ScriptLoader.CurrentPackage;
  }
}