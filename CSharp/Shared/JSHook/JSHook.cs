
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
          Mod.Harmony.Patch(original, prefix: new HarmonyMethod(GenericPrefix));
        }
        else
        {
          Mod.Harmony.Patch(original, prefix: new HarmonyMethod(GenericVoidPrefix));
        }
      },
    };

    public static PatchTracker<JSPostfix> Postfixes { get; } = new()
    {
      PatchAction = (original) =>
      {
        if (original is MethodInfo method && method.ReturnType != typeof(void))
        {
          Mod.Harmony.Patch(original, postfix: new HarmonyMethod(GenericPostfix));
        }
        else
        {
          Mod.Harmony.Patch(original, postfix: new HarmonyMethod(GenericVoidPostfix));
        }
      },
    };

    public static PatchTracker<JSFinalizer> Finalizers { get; } = new()
    {
      PatchAction = (original) =>
      {
        if (original is MethodInfo method && method.ReturnType != typeof(void))
        {
          Mod.Harmony.Patch(original, finalizer: new HarmonyMethod(GenericFinalizer));
        }
        else
        {
          Mod.Harmony.Patch(original, finalizer: new HarmonyMethod(GenericVoidFinalizer));
        }
      },
    };


    public static void Clear()
    {
      //TODO mb i should have separate harmony just for hooks
      Mod.Harmony.UnpatchSelf();

      Prefixes.Clear();
      Postfixes.Clear();
      Finalizers.Clear();
    }
  }
}