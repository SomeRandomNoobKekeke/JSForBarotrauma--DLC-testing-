
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

namespace JSForBarotrauma
{
  public class UnifiedConsole
  {
    private static V8ScriptEngine Engine => Mod.Engine.Engine;

    public static void Log(object arg1)
    {
      Engine?.Script.console.log(arg1);
      Mod.Logger.Log(arg1);
    }

    public static void Log(object arg1, object arg2)
    {
      Engine?.Script.console.log(arg1, arg2);
      Mod.Logger.Log(arg1, arg2);
    }

    public static void Log(object arg1, object arg2, object arg3)
    {
      Engine?.Script.console.log(arg1, arg2, arg3);
      Mod.Logger.Log(arg1, arg2, arg3);
    }

    //TODO add more overloads
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