
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using Barotrauma;
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
        foreach (JSPostfix postfix in info.Patches)
        {
          postfix.Invoke(__instance, info.PTable, Result);
        }
        info.PTable.Args = null;
        Result.MapBack(ref __result);
      }
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;
        Mod.Logger.Error($"JS Error in JS Postfix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.ErrorDetails);
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"C# Exception in JS Postfix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
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
        foreach (JSPostfix postfix in info.Patches)
        {
          postfix.Invoke(__instance, info.PTable, Result);
        }
        info.PTable.Args = null;
      }
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;
        Mod.Logger.Error($"JS Error in JS Postfix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.ErrorDetails);
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"C# Exception in JS Postfix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
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
        foreach (JSPrefix prefix in info.Patches)
        {
          shouldRun = shouldRun && prefix.Invoke(__instance, info.PTable, Result);
        }
        info.PTable.Args = null;
        Result.MapBack(ref __result);

        return shouldRun;
      }
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;
        Mod.Logger.Error($"JS Error in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.ErrorDetails);
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"C# Exception in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
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
        foreach (JSPrefix prefix in info.Patches)
        {
          shouldRun = shouldRun && prefix.Invoke(__instance, info.PTable, Result);
        }
        info.PTable.Args = null;

        return shouldRun;
      }
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;
        Mod.Logger.Error($"JS Error in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.ErrorDetails);
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"C# Exception in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
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
        foreach (JSFinalizer finalizer in info.Patches)
        {
          __exception = finalizer.Invoke(__instance, info.PTable, Result, __exception);
        }
        info.PTable.Args = null;
        Result.MapBack(ref __result);
      }
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;
        Mod.Logger.Error($"JS Error in JS Finalizer to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.ErrorDetails);
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"C# Exception in JS Finalizer to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
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
        foreach (JSFinalizer finalizer in info.Patches)
        {
          __exception = finalizer.Invoke(__instance, info.PTable, Result, __exception);
        }
        info.PTable.Args = null;
      }
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;
        Mod.Logger.Error($"JS Error in JS Finalizer to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.ErrorDetails);
      }
      catch (Exception e)
      {
        Mod.Logger.Error($"C# Exception in JS Finalizer to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
        Mod.Logger.Error(e.Message);
      }

      return __exception;
    }
  }
}