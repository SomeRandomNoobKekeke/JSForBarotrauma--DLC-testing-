
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
  public enum PatchType
  {
    Prefix, Postfix, Finalizer,
  }

  public class PatchTracker<DelegateT> where DelegateT : Delegate
  {
    public int MaxID { get; private set; } = 0;

    public HashSet<MethodBase> PatchedMethods { get; } = new();
    public Dictionary<MethodBase, Dictionary<int, DelegateT>> Patches { get; } = new();

    public required Func<HarmonyMethod> HarmonyMethodFactory { get; init; }
    public required PatchType PatchType { get; init; }

    public bool WasPatched(MethodBase original) => PatchedMethods.Contains(original);

    public int Add(MethodBase original, DelegateT patch)
    {
      if (!PatchedMethods.Contains(original))
      {
        switch (PatchType)
        {
          case PatchType.Prefix:
            Mod.Harmony.Patch(original, prefix: HarmonyMethodFactory());
            break;

          case PatchType.Postfix:
            Mod.Harmony.Patch(original, postfix: HarmonyMethodFactory());
            break;

          case PatchType.Finalizer:
            Mod.Harmony.Patch(original, finalizer: HarmonyMethodFactory());
            break;
        }

        PatchedMethods.Add(original);
      }

      if (!Patches.ContainsKey(original))
      {
        Patches[original] = new();
      }

      int ID = MaxID++;

      Patches[original][ID] = patch;

      return ID;
    }

    public void Remove(MethodBase original, int ID)
    {
      if (!PatchedMethods.Contains(original)) return;
      if (!Patches.ContainsKey(original)) return;

      Patches[original].Remove(ID);
    }

    public void Clear()
    {
      Patches.Clear();
    }
  }
}