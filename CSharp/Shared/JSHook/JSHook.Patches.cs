
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
      try
      {
        var info = Postfixes.PatchedMethodInfos[__originalMethod];

        FakeRefObject Result = new FakeRefObject(__result);
        info.PTable.Args = __args;
        foreach (var postfix in info.Patches.Values)
        {
          postfix.Delegate.Invoke(__instance, info.PTable, Result);
        }
        info.PTable.Args = null;
        Result.MapBack(ref __result);
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"Error in JS Postfix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.Message);
      }
    }

    public static void GenericVoidPostfix(
      MethodBase __originalMethod, object __instance, object[] __args
    )
    {
      try
      {
        var info = Postfixes.PatchedMethodInfos[__originalMethod];

        FakeRefObject Result = new FakeRefObject(null);
        info.PTable.Args = __args;
        foreach (var postfix in info.Patches.Values)
        {
          postfix.Delegate.Invoke(__instance, info.PTable, Result);
        }
        info.PTable.Args = null;
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"Error in JS Postfix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.Message);
      }
    }

    /// <summary>
    /// Note: object[] __args here is actually source of truth
    /// when you change __args you change actual method params
    /// </summary>
    public static bool GenericPrefix(
      MethodBase __originalMethod, object __instance, object[] __args, ref object __result
    )
    {
      try
      {
        var info = Prefixes.PatchedMethodInfos[__originalMethod];

        bool shouldRun = true;

        FakeRefObject Result = new FakeRefObject(__result);
        info.PTable.Args = __args;
        foreach (var prefix in info.Patches.Values)
        {
          shouldRun = shouldRun && prefix.Delegate.Invoke(__instance, info.PTable, Result);
        }
        info.PTable.Args = null;
        Result.MapBack(ref __result);

        return shouldRun;
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"Error in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.Message);
      }
      return true;
    }
    public static bool GenericVoidPrefix(
      MethodBase __originalMethod, object __instance, object[] __args
    )
    {
      try
      {
        var info = Prefixes.PatchedMethodInfos[__originalMethod];

        bool shouldRun = true;

        FakeRefObject Result = new FakeRefObject(null);
        info.PTable.Args = __args;
        foreach (var prefix in info.Patches.Values)
        {
          shouldRun = shouldRun && prefix.Delegate.Invoke(__instance, info.PTable, Result);
        }
        info.PTable.Args = null;

        return shouldRun;
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"Error in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.Message);
      }
      return true;
    }


    public static Exception GenericFinalizer(
      MethodBase __originalMethod, object __instance, object[] __args, ref object __result, Exception __exception
    )
    {
      try
      {
        var info = Finalizers.PatchedMethodInfos[__originalMethod];

        FakeRefObject Result = new FakeRefObject(__result);
        info.PTable.Args = __args;
        foreach (var finalizer in info.Patches.Values)
        {
          __exception = finalizer.Delegate.Invoke(__instance, info.PTable, Result, __exception);
        }
        info.PTable.Args = null;
        Result.MapBack(ref __result);
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"Error in JS Finalizer to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.Message);
      }

      return __exception;
    }

    public static Exception GenericVoidFinalizer(
      MethodBase __originalMethod, object __instance, object[] __args, Exception __exception
    )
    {
      try
      {
        var info = Finalizers.PatchedMethodInfos[__originalMethod];

        FakeRefObject Result = new FakeRefObject(null);
        info.PTable.Args = __args;
        foreach (var finalizer in info.Patches.Values)
        {
          __exception = finalizer.Delegate.Invoke(__instance, info.PTable, Result, __exception);
        }
        info.PTable.Args = null;
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"Error in JS Finalizer to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.Message);
      }

      return __exception;
    }
  }
}