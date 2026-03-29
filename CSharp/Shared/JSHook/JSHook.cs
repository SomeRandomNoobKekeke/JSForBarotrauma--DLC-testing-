
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
    //TODO move args to ptable
    public delegate void JSPostfix(object __instance, ParamTableBase ptable);
    public delegate bool JSPrefix(object __instance, object[] __args);
    public delegate bool JSFinalizer(object __instance, object[] __args, Exception __exception);



    // public static PatchTracker<JSPrefix> Prefixes { get; } = new()
    // {
    //   PatchAction = (original) => Mod.Harmony.Patch(original, prefix: new HarmonyMethod(GenericPrefix)),
    // };

    public static PatchTracker<JSPostfix> Postfixes { get; } = new()
    {
      PatchAction = (original) => Mod.Harmony.Patch(original, postfix: new HarmonyMethod(GenericPostfix)),
    };

    // public PatchTracker<JSPrefix> Prefixes { get; } = new()
    // {
    //   HarmonyMethodFactory = () => new HarmonyMethod(GenericPrefix),
    //   PatchType = PatchType.Prefix,
    // };

    public static void Clear()
    {
      // Prefixes.Clear();
      Postfixes.Clear();

      //TODO mb i should have separate harmony just for hooks
      Mod.Harmony.UnpatchSelf();
    }
  }
}