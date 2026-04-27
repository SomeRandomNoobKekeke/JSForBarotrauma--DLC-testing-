
using BaroJunk;
using Barotrauma;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace JSForBarotrauma
{
  public static class XMLHookAPI
  {
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["Add"] = (string name, object scriptFunc) => Add(name, scriptFunc),
      ["Remove"] = (string name) => Mod.Engine.XMLHookManager.Remove(name),
      ["Has"] = (string name) => Mod.Engine.XMLHookManager.Has(name),
    };

    public static void Add(string name, object scriptFunc)
    {
      Mod.Engine.XMLHookManager.Add(
        name,
        Mod.Engine.HostFunctions.del<Action<object[]>>(scriptFunc)
      );
    }
  }
}