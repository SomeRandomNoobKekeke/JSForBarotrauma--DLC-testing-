
using BaroJunk;
using Barotrauma;
using Barotrauma.Items.Components;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace JSForBarotrauma
{
  public class LoadTimeTracker
  {
    public Dictionary<string, Stopwatch> Watches = new();
    public bool Enabled { get; set; } = false;

    public void EnsureExists(string name)
    {
      if (!Watches.ContainsKey(name)) Watches[name] = new();
    }

    public void Start(string name)
    {
      EnsureExists(name);
      Watches[name].Start();
    }

    public void Restart(string name)
    {
      EnsureExists(name);
      Watches[name].Restart();
    }

    public void Stop(string name)
    {
      EnsureExists(name);
      Watches[name].Stop();
    }

    public void Report()
    {
      if (!Enabled) return;

      Mod.Logger.Log($"--------------");
      Mod.Logger.Log($"Loading Times:");
      foreach (var (name, sw) in Watches)
      {
        Mod.Logger.Log($"{Logger.WrapInColor(name, "white")} - {Logger.WrapInColor($"{sw.ElapsedMilliseconds} ms", "white")}");
      }
    }
  }


}