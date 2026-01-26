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
  public class DebugConsolePatch
  {
    public static void PatchClientDebugConsole(Harmony harmony)
    {
      harmony.Patch(
        original: typeof(DebugConsole).GetMethod("ExecuteCommand", AccessTools.all),
        prefix: new HarmonyMethod(typeof(DebugConsolePatch).GetMethod("DebugConsole_ExecuteCommand_Prefix"))
      );
    }


    public static void DebugConsole_ExecuteCommand_Prefix(string inputtedCommands, ref bool __runOriginal)
    {
      try
      {
        if (Mod.Instance == null) return;

        if (inputtedCommands == "js")
        {
          Mod.JS.ToggleRepl();
          __runOriginal = false;
          return;
        }

        if (Mod.JS.REPL)
        {
          __runOriginal = false;
          Mod.JS.Execute(inputtedCommands);
        }
      }
      catch (Exception e)
      {
        Mod.Logger.Error(e);
      }
    }

  }
}