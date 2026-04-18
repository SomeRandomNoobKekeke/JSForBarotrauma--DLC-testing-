
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
  public static class JSHookExposed
  {
    public delegate void JSPostfix(object __instance, LilParamTable __args, FakeRefObject __result);
    public delegate bool JSPrefix(object __instance, LilParamTable __args, FakeRefObject __result);
    public delegate Exception JSFinalizer(object __instance, LilParamTable __args, FakeRefObject __result, Exception __exception);

    public static PatchTracker<JSPrefix> Prefixes => JSHook.Prefixes;
    public static PatchTracker<JSPostfix> Postfixes => JSHook.Postfixes;
    public static PatchTracker<JSFinalizer> Finalizers => JSHook.Finalizers;
    public static void Clear() => JSHook.Clear();
  }
}