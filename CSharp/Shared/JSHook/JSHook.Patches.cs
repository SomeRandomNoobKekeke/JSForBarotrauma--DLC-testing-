
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
  public partial class JSHook
  {


    public static void GenericPostfix(
      MethodBase __originalMethod, object __instance, object[] __args, ref object __result
    )
    {
      var info = Postfixes.PatchedMethods[__originalMethod];

      FakeRefObject Result = new FakeRefObject(__result);
      foreach (JSPostfix postfix in info.Patches.Values)
      {
        try
        {
          postfix.Invoke(__instance, __args, Result);
        }
        catch (Exception e)
        {
          Mod.Logger.Error($"Error in JS Postfix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
          Mod.Logger.Error(e.Message);
        }
      }

      Result.MapBack(ref __result);
    }

    /// <summary>
    /// Note: object[] __args here is actually source of truth
    /// when you change __args you change actual method params
    /// </summary>
    public static bool GenericPrefix(
      MethodBase __originalMethod, object __instance, object[] __args, ref object __result
    )
    {
      var info = Prefixes.PatchedMethods[__originalMethod];

      bool shouldRun = true;

      FakeRefObject Result = new FakeRefObject(__result);
      foreach (JSPrefix prefix in info.Patches.Values)
      {
        try
        {
          shouldRun = shouldRun && prefix.Invoke(__instance, __args, Result);
        }
        catch (Exception e)
        {
          Mod.Logger.Error($"Error in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
          Mod.Logger.Error(e.Message);
        }
      }
      Result.MapBack(ref __result);

      return shouldRun;
    }


    public static Exception GenericFinalizer(
      MethodBase __originalMethod, object __instance, object[] __args, ref object __result, Exception __exception
    )
    {
      var info = Finalizers.PatchedMethods[__originalMethod];

      FakeRefObject Result = new FakeRefObject(__result);
      foreach (JSFinalizer finalizer in info.Patches.Values)
      {
        try
        {
          __exception = finalizer.Invoke(__instance, __args, Result, __exception);
        }
        catch (Exception e)
        {
          Mod.Logger.Error($"Error in JS Finalizer to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
          Mod.Logger.Error(e.Message);
        }
      }
      Result.MapBack(ref __result);

      return __exception;
    }
  }
}