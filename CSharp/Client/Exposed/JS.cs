
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
using BaroJunk;
namespace JSForBarotrauma
{
  public class JS
  {
    public V8ScriptEngine Engine => Mod.Engine.Engine;


    //TODO remove engine callbacks after stop
    public event Action StopEvent;

    public void ReloadLua()
    {
      GameMain.LuaCs.Timer.Wait((args) =>
      {
        DebugConsole.ExecuteCommand("cl_reloadlua");
      }, 100);
    }

    public void Reload()
    {
      if (!Mod.Engine.IsRunning)
      {
        Start();
        return;
      }

      GameMain.LuaCs.Timer.Wait((args) =>
      {
        StopEvent?.Invoke();
        Mod.Engine.Stop();

        GameMain.LuaCs.Timer.Wait((args) =>
        {
          Mod.Engine.Start();
          Mod.ScriptLoader.LoadScripts();
          Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Reloaded", "White")));
        }, 100);
      }, 100);
    }

    public void Stop()
    {
      if (!Mod.Engine.IsRunning) return;

      GameMain.LuaCs.Timer.Wait((args) =>
      {
        StopEvent?.Invoke();
        Mod.Engine.Stop();
        Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Stopped", "White")));
      }, 100);
    }

    public void Start()
    {
      if (Mod.Engine.IsRunning) return;

      GameMain.LuaCs.Timer.Wait((args) =>
      {
        Mod.Engine.Start();
        Mod.ScriptLoader.LoadScripts();
        Mod.Logger.Log(ConsoleInterface.WrapInBraces(Logger.WrapInColor("JS Started", "White")));
      }, 100);
    }
  }

}