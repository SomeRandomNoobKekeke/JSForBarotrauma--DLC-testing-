using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using Microsoft.ClearScript;

namespace JSForBarotrauma
{
  public partial class Mod
  {
    public List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();
    public void AddCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("js", "", JS_Command));
      AddedCommands.Add(new DebugConsole.Command("js_restart", "", JSRestartCommand));

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public void JSRestartCommand(object[] args)
    {
      Mod.JS.Restart();
    }

    public void JS_Command(object[] args)
    {
      if (args.Length == 0) return;
      Mod.JS.Execute(string.Join(" ", args));
    }

    public void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }
  }
}