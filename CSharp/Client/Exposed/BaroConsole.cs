
using System;
using System.Reflection;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.ClearScript.V8;

namespace JSForBarotrauma
{
  public class BaroConsole
  {
    public void Log(object msg)
    {
      LuaCsLogger.LogMessage($"{msg ?? "null"}", Color.Cyan * 0.8f, Color.Cyan);
    }
  }
}