
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
using Barotrauma.Networking;

namespace JSForBarotrauma
{
  public partial class ConsoleInterface
  {
    public void AddPatches(Harmony harmony)
    {
#if CLIENT
      harmony.Patch(
       original: typeof(DebugConsole).GetMethod("IsCommandPermitted", AccessTools.all),
       postfix: new HarmonyMethod(typeof(ConsoleInterface).GetMethod("PermitCommands"))
      );
#endif 

      harmony.Patch(
       original: typeof(DebugConsole).GetMethod("ExecuteCommand", AccessTools.all),
       prefix: new HarmonyMethod(typeof(ConsoleInterface).GetMethod("InterceptJSREPL"))
      );
    }

    public static void InterceptJSREPL(string inputtedCommands, ref bool __runOriginal)
    {
      if (Mod.ConsoleInterface is null) return;

      try
      {
        if (inputtedCommands == "js")
        {
          __runOriginal = false;
          Mod.ConsoleInterface.ToggleRepl();
          return;
        }

        if (Mod.ConsoleInterface.REPL)
        {
          __runOriginal = false;
          Mod.ConsoleInterface.ExecuteJSCommand(inputtedCommands);
        }
      }
      catch (Exception e)
      {
        Mod.Logger.Error(e);
      }
    }


    public EngineWrapper EngineWrapper { get; }
    public bool REPL { get; set; }
    public void ToggleRepl()
    {
      REPL = !REPL;
      Mod.Logger.Log(WrapInBraces($"JS REPL mode [{Logger.WrapInColor(REPL ? " Enabled " : " Disabled ", "white")}]"));
    }

    public void ExecuteJSCommand(string command)
    {
      Mod.Logger.Print($">> {command}", Color.White);

      try
      {
        Mod.Logger.Log(EngineWrapper.Engine.Evaluate(command));
      }
      catch (ScriptEngineException e)
      {
        Mod.Logger.Error(e.ErrorDetails);
      }
    }

    public static string WrapInBraces(object msg)
      => $"------------------------<<< {msg} >>>------------------------";


    public List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();
    public void AddCommands()
    {
      AddedCommands.Add(new DebugConsole.Command("js", "", JS_Command,
      () => new string[][] { EngineWrapper.Engine.Global.PropertyNames.ToArray() }));
      AddedCommands.Add(new DebugConsole.Command("js_reload", "", JSReloadCommand));
      AddedCommands.Add(new DebugConsole.Command("js_stop", "", JSStopCommand));
      AddedCommands.Add(new DebugConsole.Command("js_start", "", JSStartCommand));

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }

    public void JSReloadCommand(object[] args) => Mod.Engine?.Reload();
    public void JSStopCommand(object[] args) => Mod.Engine?.Stop();
    public void JSStartCommand(object[] args) => Mod.Engine?.Start();

    public void JS_Command(object[] args)
    {
      if (args.Length == 0) return;
      ExecuteJSCommand(string.Join(" ", args));
    }

    public ConsoleInterface(EngineWrapper engineWrapper) => EngineWrapper = engineWrapper;
  }

}