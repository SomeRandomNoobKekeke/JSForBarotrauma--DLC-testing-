
using BaroJunk;
using Barotrauma;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace JSForBarotrauma
{
  public enum PatchType
  {
    Prefix, Postfix, Finalizer,
  }


  public partial class PatchTracker<DelegateT> where DelegateT : Delegate
  {
    public record PatchInfo<DelegateT>(DelegateT Delegate, int ID, int Priority);
    public class PatchedMethodInfo<DelegateT>
    {
      public LilParamTable PTable { get; }

      public PatchInfoCollection<DelegateT> Patches { get; } = new();

      public PatchedMethodInfo(MethodBase original)
      {
        PTable = new LilParamTable(original.GetParameters());
      }

      public void Clear()
      {
        PTable.Args = null;
        Patches.Clear();
      }
    }

    public required Action<MethodBase> PatchAction { get; init; }

    public Dictionary<MethodBase, PatchedMethodInfo<DelegateT>> PatchedMethodInfos { get; } = new();
    //HACK i track it separately to avoid crashes or if(PatchedMethodInfos.ContainsKey) checks in dead patches
    public HashSet<MethodBase> PatchedMethods { get; } = new();


    public bool WasPatched(MethodBase original) => PatchedMethods.Contains(original);

    public int Add(MethodBase original, DelegateT patch, int priority = Priority.Normal)
    {
      if (!WasPatched(original))
      {
        PatchedMethodInfos[original] = new PatchedMethodInfo<DelegateT>(original);
        PatchedMethods.Add(original);

        try
        {
          PatchAction(original);
        }
        catch (Exception e)
        {
          Mod.Logger.Error($"Harmony error:");
          throw e;
        }
      }

      return PatchedMethodInfos[original].Patches.Add(patch, priority);
    }

    public void Remove(MethodBase original, int ID)
    {
      if (!WasPatched(original)) return;

      PatchedMethodInfos[original].Patches.Remove(ID);
    }

    public void Clear()
    {
      foreach (var info in PatchedMethodInfos.Values)
      {
        info.Clear();
      }

      PatchedMethods.Clear();
    }
  }
}