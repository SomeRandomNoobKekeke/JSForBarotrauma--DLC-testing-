
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


  public class PatchTracker<DelegateT> where DelegateT : Delegate
  {
    public record PatchInfo<DelegateT>(DelegateT Delegate, int ID, int Priority);
    public class PatchedMethodInfo<DelegateT>
    {
      public LilParamTable PTable { get; }
      public SortedList<int, PatchInfo<DelegateT>> Patches { get; } = new();

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



    public int MaxID { get; private set; } = 0;


    public Dictionary<MethodBase, PatchedMethodInfo<DelegateT>> PatchedMethodInfos { get; } = new();

    public HashSet<MethodBase> PatchedMethods { get; } = new();

    public required Action<MethodBase> PatchAction { get; init; }

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

      int ID = MaxID++;

      PatchedMethodInfos[original].Patches.Add(priority, new PatchInfo<DelegateT>(patch, ID, priority));

      foreach (var item in PatchedMethodInfos[original].Patches)
      {
        Mod.Logger.Log(item);
      }

      return ID;
    }

    public void Remove(MethodBase original, int ID)
    {
      if (!WasPatched(original)) return;

      //no RemoveAll? bruh
      for (int i = 0; i < PatchedMethodInfos[original].Patches.Count; i++)
      {
        if (PatchedMethodInfos[original].Patches.Values[i].ID == ID)
        {
          PatchedMethodInfos[original].Patches.RemoveAt(i);
        }
      }
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