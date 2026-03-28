
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
    public delegate void JSPostfix(object __instance, object[] __args);
    public delegate bool JSPrefix(object __instance, object[] __args);
    public delegate bool JSFinalizer(object __instance, object[] __args, Exception __exception);

    public static HashSet<MethodBase> PatchedWithGenericPostfix { get; } = new();
    public static HashSet<MethodBase> PatchedWithGenericPrefix { get; } = new();
    public static Dictionary<MethodBase, Dictionary<string, JSPostfix>> Postfixes { get; } = new();
    public static Dictionary<MethodBase, Dictionary<string, JSPrefix>> Prefixes { get; } = new();

    public static void Clear()
    {
      Postfixes.Clear();
      Prefixes.Clear();
      PatchedWithGenericPostfix.Clear();
      PatchedWithGenericPrefix.Clear();
      Mod.Harmony.UnpatchSelf();
    }

    public static void RemoveAll()=> Clear();

    public static void AddPostfix(string hookID, MethodBase original, JSPostfix callback)
    {
      if (!PatchedWithGenericPostfix.Contains(original))
      {
        Mod.Harmony.Patch(original, postfix: new HarmonyMethod(GenericPostfix));
        PatchedWithGenericPostfix.Add(original);
      }

      if (!Postfixes.ContainsKey(original))
      {
        Postfixes[original] = new();
      }

      Postfixes[original][hookID] = callback;
    }

    public static void RemovePostfix(string hookID, MethodBase original)
    {
      if (!Postfixes.ContainsKey(original)) return;
      Postfixes[original].Remove(hookID);
    }


    public static void AddPrefix(string hookID, MethodBase original, JSPrefix callback)
    {
      if (!PatchedWithGenericPrefix.Contains(original))
      {
        Mod.Harmony.Patch(original, prefix: new HarmonyMethod(GenericPrefix));
        PatchedWithGenericPrefix.Add(original);
      }

      if (!Prefixes.ContainsKey(original))
      {
        Prefixes[original] = new();
      }

      Prefixes[original][hookID] = callback;
    }

    public static void RemovePrefix(string hookID, MethodBase original)
    {
      if (!Prefixes.ContainsKey(original)) return;
      Prefixes[original].Remove(hookID);
    }


  }
}