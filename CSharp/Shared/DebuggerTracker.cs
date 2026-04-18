
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
  public class DebuggerTracker
  {

    public void OnDebuggerConnected(object sender, V8RuntimeDebuggerEventArgs args)
    {
      if (Mod.Engine != null) Mod.Engine.DebuggerAttached = true;
    }

    public void OnDebuggerDisconnected(object sender, V8RuntimeDebuggerEventArgs args)
    {
      if (Mod.Engine != null) Mod.Engine.DebuggerAttached = false;
    }

    public void Track()
    {
      V8Runtime.DebuggerConnected += OnDebuggerConnected;
      V8Runtime.DebuggerDisconnected += OnDebuggerDisconnected;
    }

    public void Untrack()
    {
      V8Runtime.DebuggerConnected -= OnDebuggerConnected;
      V8Runtime.DebuggerDisconnected -= OnDebuggerDisconnected;
    }
  }

}