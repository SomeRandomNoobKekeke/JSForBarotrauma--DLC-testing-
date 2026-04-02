
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
#if CLIENT
    public static void PermitCommands(Identifier command, GameClient client, ref bool __result)
    {
      if (Mod.ConsoleInterface is null) return;
      if (Mod.ConsoleInterface.AddedCommands.Any(c => c.Names.Contains(command.Value))) __result = true;
    }
#endif
  }

}