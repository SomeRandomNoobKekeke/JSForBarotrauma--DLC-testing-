
using BaroJunk;
using Barotrauma;
using Barotrauma.Plugins;
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
  public static partial class JSHook
  {
    public static Harmony Harmony { get; private set; } = new Harmony("JSForBarotraumaHook");

    //HACK whatever i throw from js gets converted to ScriptEngineException, only the message remains

    //BRUH i can't declare same delegates in two places and this class should be secret
    // public delegate void JSPostfix(object __instance, LilParamTable __args, FakeRefObject __result);
    // public delegate bool JSPrefix(object __instance, LilParamTable __args, FakeRefObject __result);
    // public delegate Exception JSFinalizer(object __instance, LilParamTable __args, FakeRefObject __result, Exception __exception);

    public static StackTrace GetStackTrace()
    {
      return new StackTrace(new StackTrace(1, true).GetFrames().SkipWhile(
          frame => frame.GetMethod().DeclaringType?.Assembly != typeof(JSHook).Assembly
        )//.Skip(1)
      );
    }

    //BRUH it's spreading
    public static PatchTracker<JSHookExposed.JSPrefix> Prefixes { get; } = new()
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

    public static PatchTracker<JSHookExposed.JSPostfix> Postfixes { get; } = new()
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

    public static PatchTracker<JSHookExposed.JSFinalizer> Finalizers { get; } = new()
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
      //Note: it seems that Harmony.UnpatchSelf() is not instantaneous, so it doesn't matter if i unpatch first, some dead patches might be invoked one more time at least
      Harmony.UnpatchSelf();

      Prefixes.Clear();
      Postfixes.Clear();
      Finalizers.Clear();
    }
  }
}