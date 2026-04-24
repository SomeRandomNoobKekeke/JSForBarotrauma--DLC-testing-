
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
using BaroJunk;

using System.Threading;
using System.Threading.Tasks;

namespace JSForBarotrauma
{
  public class XMLHookManager
  {
    public HashSet<string> AddedHookNames { get; } = new();


    public void Add(string name, Action<object[]> callback)
    {
      if (AddedHookNames.Contains(name))
      {
        throw new Exception($"{name} hook already added");
      }

      LuaCsSetup.Instance.Hook.Add(name, "js xml hook", (object[] args) =>
      {
        callback(args);
        return null;
      });
    }

    public bool Has(string name) => AddedHookNames.Contains(name);

    public void Remove(string name)
    {
      if (AddedHookNames.Contains(name))
      {
        LuaCsSetup.Instance.Hook.Remove(name, name);
        AddedHookNames.Remove(name);
      }
    }

    public void Clear()
    {
      foreach (string name in AddedHookNames)
      {
        LuaCsSetup.Instance.Hook.Remove(name, name);
      }

      AddedHookNames.Clear();
    }
  }
}