
using BaroJunk;
using Barotrauma;
using Barotrauma.Networking;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

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

      harmony.Patch(
        original: typeof(DebugConsole).GetMethod("Update", AccessTools.all),
        prefix: new HarmonyMethod(typeof(ConsoleInterface).GetMethod("DebugConsole_Update_Enter"))
      );

      harmony.Patch(
        original: typeof(DebugConsole).GetMethod("Update", AccessTools.all),
        postfix: new HarmonyMethod(typeof(ConsoleInterface).GetMethod("DebugConsole_Update_Exit"))
      );
    }


    public static bool FromDebugConsole = false;
    public static void DebugConsole_Update_Enter() { FromDebugConsole = true; }
    public static void DebugConsole_Update_Exit() { FromDebugConsole = false; }


    public static void InterceptJSREPL(string inputtedCommands, ref bool __runOriginal)
    {
      if (Mod.ConsoleInterface is null) return;
      if (!FromDebugConsole) return;

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
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;

        Mod.Logger.Error(e.ErrorDetails);
      }
    }


    public EngineWrapper EngineWrapper { get; }
    private bool repl; public bool REPL
    {
      get => repl;
      set
      {
        repl = value;
        Mod.Logger.Log(WrapInBraces($"JS REPL mode [{Logger.WrapInColor(REPL ? " Enabled " : " Disabled ", "white")}]"));
      }
    }
    public void ToggleRepl()
    {
      REPL = !REPL;
    }

    public void ExecuteJSCommand(string command)
    {
      //Mod.Logger.Print($">> {command}", Color.White);

      try
      {
        Mod.Logger.Log(EngineWrapper.Engine.Evaluate(command));
      }
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;

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
      AddedCommands.Add(new DebugConsole.Command("crash", "", Crash_Command));
      AddedCommands.Add(new DebugConsole.Command("printallharmonypatches", "", PrintAllHarmonyPatches));

#if CLIENT
      foreach (DebugConsole.Command command in AddedCommands)
      {
        command.RelayToServer = false;
      }
#endif

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
    public void PrintAllHarmonyPatches(object[] args) => Utils.PrintAllPatchedMethods();

    public void JS_Command(object[] args)
    {
      if (args.Length == 0) return;
      ExecuteJSCommand(string.Join(" ", args));
    }

    public void Crash_Command(object[] args) => throw new ExecutionEngineException("You Died!");

    public ConsoleInterface(EngineWrapper engineWrapper) => EngineWrapper = engineWrapper;
  }

}