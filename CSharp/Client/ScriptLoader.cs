
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
using System.IO;
using BaroJunk;

namespace JSForBarotrauma
{
  public class ScriptLoader
  {
    public static string AutorunPath = Path.Combine("JS", "Autorun");
    public static string JSPath = "JS";
    public static string JSFileSearchPattern = "*.js";

    public EngineWrapper EngineWrapper { get; }

    public void SetRootPath(string path) => EngineWrapper.SearchPath = path;

    public object LoadModuleFromPath(string path)
    {
      try
      {
        return EngineWrapper.Engine.Evaluate(
          new DocumentInfo { Category = ModuleCategory.CommonJS },
          File.ReadAllText(path)
        );
      }
      catch (ScriptEngineException e)
      {
        Mod.Logger.Error(e.ErrorDetails);
        return null;
      }
    }

    public void LoadScriptsFromMod(string path)
    {
      if (!Directory.Exists(Path.Combine(path, AutorunPath))) return;

      SetRootPath(Path.Combine(path, JSPath));

      foreach (string file in Directory.GetFiles(Path.Combine(path, AutorunPath), JSFileSearchPattern))
      {
        LoadModuleFromPath(file);
      }
    }

    public void LoadScripts()
    {
      LoadScriptsFromMod(ModInfo.ModDir<Mod>());
    }




    public ScriptLoader(EngineWrapper engineWrapper) => EngineWrapper = engineWrapper;
  }

}