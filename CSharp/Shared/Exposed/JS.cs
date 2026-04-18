
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
using System.Threading;
using BaroJunk;


namespace JSForBarotrauma
{
  public class JS
  {
    private EngineWrapper EngineWrapper { get; }
    public V8ScriptEngine Engine => EngineWrapper.Engine;

    public ClearableEvent OnStop { get; } = new();
    public ScriptObject Global => Engine?.Global;

    public bool REPL
    {
      get => Mod.ConsoleInterface.REPL;
      set => Mod.ConsoleInterface.REPL = value;
    }

    public void SetTimeout(Action action, int delay) => Utils.RunWithDelay(action, delay);


    public void Reload()
    {
      Utils.RunWithDelay(() => Mod.Engine.Reload());
    }
    public void Stop()
    {
      Utils.RunWithDelay(() => Mod.Engine.Stop());
    }
    public void Start() => EngineWrapper.Start();


    public JS(EngineWrapper engineWrapper) => EngineWrapper = engineWrapper;
  }
}