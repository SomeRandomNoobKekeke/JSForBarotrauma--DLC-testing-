
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
using Barotrauma.Steam;

namespace JSForBarotrauma
{
  public static class ConsoleAPI
  {
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["AddCommand"] = (string name, object scriptFunc) => AddCommand(name, scriptFunc),
      ["CommandExists"] = (string name) => CommandExists(name),
      ["RemoveCommand"] = (string name) => RemoveCommand(name),
      ["ExecuteCommand"] = (string command) => ExecuteCommand(command),
    };

    public static void AddCommand(string name, object scriptFunc)
      => Mod.Engine.JSCommandManager.AddCommand(name, scriptFunc);

    public static bool CommandExists(string name)
      => Mod.Engine.JSCommandManager.CommandExists(name);

    public static void RemoveCommand(string name)
      => Mod.Engine.JSCommandManager.RemoveCommand(name);

    public static void ExecuteCommand(string command)
      => Mod.Engine.JSCommandManager.ExecuteCommand(command);
  }
}