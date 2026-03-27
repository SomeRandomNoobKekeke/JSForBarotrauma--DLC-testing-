
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
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;

namespace JSForBarotrauma
{
  public class UnifiedConsole
  {
    private static V8ScriptEngine Engine => Mod.Engine.Engine;

    public static void Log(object msg)
    {
      Engine?.Script.console.log(msg);
      Mod.Logger.Log(msg);
    }

    public static void Warning(object msg)
    {
      Engine?.Script.console.warn(msg);
      Mod.Logger.Warning(msg);
    }

    public static void Error(object msg)
    {
      Engine?.Script.console.error(msg);
      Mod.Logger.Error(msg);
    }
  }
}