
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
  public class JSCommandManager
  {
    public Dictionary<string, DebugConsole.Command> AddedCommands = new();

    public void AddCommand(string name, object scriptFunc)
    {
      if (AddedCommands.ContainsKey(name))
      {
        throw new Exception($"[{name}] command is already added");
      }

      Action<object> action = Mod.Engine.HostFunctions.del<Action<object>>(scriptFunc);

      DebugConsole.Command command = new DebugConsole.Command(name, "", (string[] args) => action(args));

#if CLIENT
      command.RelayToServer = false;
#endif

      AddedCommands[name] = command;
      DebugConsole.Commands.Insert(0, command);
    }

    public bool CommandExists(string name)
      => AddedCommands.ContainsKey(name);

    public void ExecuteCommand(string command)
      => DebugConsole.ExecuteCommand(command);
    public void RemoveCommand(string name)
    {
      if (!AddedCommands.ContainsKey(name))
      {
        throw new Exception($"no such command [{name}]");
      }

      DebugConsole.Commands.Remove(AddedCommands[name]);
      AddedCommands.Remove(name);
    }

    public void Clear()
    {
      foreach (DebugConsole.Command command in AddedCommands.Values)
      {
        DebugConsole.Commands.Remove(command);
      }
      AddedCommands.Clear();
    }
  }
}