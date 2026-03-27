using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Barotrauma.Networking;

namespace JSForBarotrauma
{
  public partial class ConsoleInterface
  {
    public void AddPatches(Harmony harmony)
    {
      //harmony.Patch(
      //  original: typeof(DebugConsole).GetMethod("IsCommandPermitted", AccessTools.all),
      //  postfix: new HarmonyMethod(typeof(ConsoleInterface).GetMethod("PermitCommands"))
      //);

      //harmony.Patch(
      //  original: typeof(DebugConsole).GetMethod("ExecuteCommand", AccessTools.all),
      //  prefix: new HarmonyMethod(typeof(ConsoleInterface).GetMethod("InterceptJSREPL"))
      //);
    }

    //public static void PermitCommands(Identifier command, GameClient client, ref bool __result)
    //{
    //  if (Mod.ConsoleInterface is null) return;
    //  if (Mod.ConsoleInterface.AddedCommands.Any(c => c.Names.Contains(command.Value))) __result = true;
    //}

    //public static void InterceptJSREPL(string inputtedCommands, ref bool __runOriginal)
    //{
    //  if (Mod.ConsoleInterface is null) return;

    //  try
    //  {
    //    if (inputtedCommands == "js")
    //    {
    //      __runOriginal = false;
    //      Mod.ConsoleInterface.ToggleRepl();
    //      return;
    //    }

    //    if (Mod.ConsoleInterface.REPL)
    //    {
    //      __runOriginal = false;
    //      Mod.ConsoleInterface.ExecuteJSCommand(inputtedCommands);
    //    }
    //  }
    //  catch (Exception e)
    //  {
    //    Mod.Logger.Error(e);
    //  }
    //}

  }
}