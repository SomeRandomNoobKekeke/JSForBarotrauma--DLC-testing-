
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Barotrauma;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Runtime.CompilerServices;
using System.IO;
using BaroJunk;


namespace JSForBarotrauma
{
  public static partial class JSHook
  {
    public static Harmony Harmony { get; private set; } = new Harmony("JSForBarotraumaHook");

    public delegate void JSPostfix(object __instance, LilParamTable __args, FakeRefObject __result);
    public delegate bool JSPrefix(object __instance, LilParamTable __args, FakeRefObject __result);
    public delegate Exception JSFinalizer(object __instance, LilParamTable __args, FakeRefObject __result, Exception __exception);


    //BRUH it's spreading
    public static PatchTracker<JSPrefix> Prefixes { get; } = new()
    {
      PatchAction = (original) =>
      {
        if (original is MethodInfo method && method.ReturnType != typeof(void))
        {
          Harmony.Patch(original, prefix: new HarmonyMethod(GenericPrefix));
        }
        else
        {
          Harmony.Patch(original, prefix: new HarmonyMethod(GenericVoidPrefix));
        }
      },
    };

    public static PatchTracker<JSPostfix> Postfixes { get; } = new()
    {
      PatchAction = (original) =>
      {
        if (original is MethodInfo method && method.ReturnType != typeof(void))
        {
          Harmony.Patch(original, postfix: new HarmonyMethod(GenericPostfix));
        }
        else
        {
          Harmony.Patch(original, postfix: new HarmonyMethod(GenericVoidPostfix));
        }
      },
    };

    public static PatchTracker<JSFinalizer> Finalizers { get; } = new()
    {
      PatchAction = (original) =>
      {
        if (original is MethodInfo method && method.ReturnType != typeof(void))
        {
          Harmony.Patch(original, finalizer: new HarmonyMethod(GenericFinalizer));
        }
        else
        {
          Harmony.Patch(original, finalizer: new HarmonyMethod(GenericVoidFinalizer));
        }
      },
    };


    public static void Clear()
    {
      Mod.Logger.Log($"Harmony unpatched");
      Harmony.UnpatchSelf();
      Harmony = null;

      Mod.Logger.Log($"hooks cleared");
      Prefixes.Clear();
      Postfixes.Clear();
      Finalizers.Clear();
    }
  }
}