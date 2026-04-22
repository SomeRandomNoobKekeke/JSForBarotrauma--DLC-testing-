
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Threading;
using BaroJunk;


namespace JSForBarotrauma
{
  public class PatchTrackerBag<DelegateT> : PropBag where DelegateT : Delegate
  {
    public PatchTracker<DelegateT> PatchTracker { get; }
    public PatchTrackerBag(PatchTracker<DelegateT> patchTracker) : base()
    {
      PatchTracker = patchTracker;

      Props = new Dictionary<string, ProxyProp>();

      Props["Add"] = new ProxyProp(
        () => (MethodBase original, object scriptFunc, int priority = Priority.Normal) =>
        {
          return PatchTracker.Add(
            original,
            Mod.Engine.HostFunctions.del<DelegateT>(scriptFunc),
            priority
          );
        }
      );

      Props["WasPatched"] = new ProxyProp(
        () => (MethodBase original) => PatchTracker.WasPatched(original)
      );

      Props["Remove"] = new ProxyProp(
        () => (MethodBase original, int ID) => PatchTracker.Remove(original, ID)
      );

      Props["Clear"] = new ProxyProp(
        () => () => PatchTracker.Clear()
      );

      Hints = Props.Keys.ToHashSet();
    }
  }
}