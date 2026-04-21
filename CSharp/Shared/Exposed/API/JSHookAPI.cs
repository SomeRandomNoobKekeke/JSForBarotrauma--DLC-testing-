
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
    public static PatchTracker<JSHook.JSPrefix> Prefixes => JSHook.Prefixes;
    public static PatchTracker<JSHook.JSPostfix> Postfixes => JSHook.Postfixes;
    public static PatchTracker<JSHook.JSFinalizer> Finalizers => JSHook.Finalizers;
    public static void Clear() => JSHook.Clear();
  }
}