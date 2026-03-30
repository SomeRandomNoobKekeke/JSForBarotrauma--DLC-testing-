
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

      foreach (JSPostfix postfix in info.Patches.Values)
      {
        try
        {
          FakeRefObject Result = new FakeRefObject(__result);
          postfix.Invoke(__instance, __args, Result);
          Result.MapBack(ref __result);
        }
        catch (Exception e)
        {
          Mod.Logger.Error($"Error in JS Postfix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
          Mod.Logger.Error(e);
        }
      }
    }

    public static bool GenericPrefix(
      MethodBase __originalMethod, object __instance, object[] __args, ref object __result
    )
    {
      var info = Prefixes.PatchedMethods[__originalMethod];

      bool shouldRun = true;

      foreach (JSPrefix prefix in info.Patches.Values)
      {
        try
        {
          FakeRefObject Result = new FakeRefObject(__result);
          shouldRun = shouldRun && prefix.Invoke(__instance, __args, Result);
          Result.MapBack(ref __result);
        }
        catch (Exception e)
        {
          Mod.Logger.Error($"Error in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
          Mod.Logger.Error(e);
        }
      }

      return shouldRun;
    }


    // public static bool GenericFinalizer(MethodBase __originalMethod, object __instance, object[] __args)
    // {
    //   if (!Postfixes.ContainsKey(__originalMethod)) return true;

    //   bool shouldRun = true;

    //   foreach (JSPrefix prefix in Prefixes[__originalMethod].Values)
    //   {
    //     try
    //     {
    //       shouldRun = shouldRun && prefix.Invoke(__instance, __args);
    //     }
    //     catch (Exception e)
    //     {
    //       Mod.Logger.Error($"Error in JS Prefix to [{__originalMethod.DeclaringType}.{__originalMethod}]:");
    //       Mod.Logger.Error(e);
    //     }
    //   }

    //   return shouldRun;
    // }
  }
}