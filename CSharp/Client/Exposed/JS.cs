
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
  public class JS
  {
    public V8ScriptEngine Engine => Mod.Engine.Engine;

    public void Reload()
    {
      Mod.Engine.Stop();

      GameMain.LuaCs.Timer.Wait((args) =>
      {
        Mod.Engine.Start();
        Mod.ScriptLoader.LoadScripts();
      }, 1000);

    }
  }

}