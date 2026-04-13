
using BaroJunk;
using Barotrauma;
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
  public partial class PatchTracker<DelegateT> where DelegateT : Delegate
  {
    public class PatchInfoCollection<DelegateT> : IEnumerable<DelegateT>
    {
      public int MaxID { get; private set; } = 0;
      public List<PatchInfo<DelegateT>> Patches { get; } = new();

      public IEnumerable<DelegateT> Enumerate()
      {
        for (int i = Patches.Count - 1; i >= 0; i--)
        {
          yield return Patches[i].Delegate;
        }
      }

      public PatchInfo<DelegateT> Get(int id) => Patches.Find(patch => patch.ID == id);

      public int Add(DelegateT patch, int priority)
      {
        int ID = MaxID++;

        Patches.Add(new PatchInfo<DelegateT>(patch, ID, priority));
        Patches.Sort((a, b) => a.Priority - b.Priority);
        return ID;
      }

      public bool Remove(int id)
      {
        int index = Patches.FindIndex(patch => patch.ID == id);
        if (index == -1) return false;

        Patches.RemoveAt(index);
        // Patches.Sort((a, b) => a.Priority > b.Priority); // akshually why?
        return true;
      }

      public void Clear() => Patches.Clear();

      IEnumerator<DelegateT> IEnumerable<DelegateT>.GetEnumerator() => Enumerate().GetEnumerator();
      IEnumerator IEnumerable.GetEnumerator() => Enumerate().GetEnumerator();
    }
  }

}