
using BaroJunk;
using Barotrauma;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace JSForBarotrauma
{
  public static class JSHookAPI
  {
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["Prefixes"] = new PatchTrackerBag<JSHook.JSPrefix>(JSHook.Prefixes),
      ["Postfixes"] = new PatchTrackerBag<JSHook.JSPostfix>(JSHook.Postfixes),
      ["Finalizers"] = new PatchTrackerBag<JSHook.JSFinalizer>(JSHook.Finalizers),
      ["Clear"] = () => JSHook.Clear(),
    };
  }
}