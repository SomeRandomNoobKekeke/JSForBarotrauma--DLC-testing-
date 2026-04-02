
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
    public class PatchInfo<DelegateT>
    {
      public LilParamTable PTable { get; }
      public Dictionary<int, DelegateT> Patches { get; } = new();

      public PatchInfo(MethodBase original)
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


    public Dictionary<MethodBase, PatchInfo<DelegateT>> PatchedMethods { get; } = new();

    public required Action<MethodBase> PatchAction { get; init; }

    public bool WasPatched(MethodBase original) => PatchedMethods.ContainsKey(original);

    public int Add(MethodBase original, DelegateT patch)
    {
      if (!WasPatched(original))
      {
        PatchedMethods[original] = new PatchInfo<DelegateT>(original);

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

      PatchedMethods[original].Patches[ID] = patch;

      return ID;
    }

    public void Remove(MethodBase original, int ID)
    {
      if (!WasPatched(original)) return;
      PatchedMethods[original].Patches.Remove(ID);
    }

    public void Clear()
    {
      foreach (var info in PatchedMethods.Values)
      {
        info.Clear();
      }
    }
  }
}