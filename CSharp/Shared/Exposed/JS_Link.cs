
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
using System.Threading;
using BaroJunk;


namespace JSForBarotrauma
{
  public partial class EngineWrapper
  {
    public class JS_Link
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

      public void Throw(Exception exception)
      {
        throw exception;
      }

      public void Reload()
      {
        Utils.RunWithDelay(() => Mod.Engine.Reload());
      }
      public void Stop()
      {
        Utils.RunWithDelay(() => Mod.Engine.Stop());
      }
      public void Start() => EngineWrapper.Start(); // xd

      public JS_Link(EngineWrapper engineWrapper) => EngineWrapper = engineWrapper;
    }
  }

}