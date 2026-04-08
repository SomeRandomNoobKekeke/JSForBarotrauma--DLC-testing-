
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
using System.IO;
using BaroJunk;

namespace JSForBarotrauma
{
  public partial class ScriptLoader
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
          new DocumentInfo { Category = ModuleCategory.Standard },
          File.ReadAllText(path)
        );
      }
      catch (ScriptEngineException e)
      {
        if (e.ScriptExceptionAsObject is Exception) throw e.ScriptExceptionAsObject as Exception;

        Mod.Logger.Error($"JS | >> {path}");
        Mod.Logger.Error(e.ErrorDetails);

        EngineWrapper.Engine.Script.console.error($">> {path}");
        EngineWrapper.Engine.Script.console.error(e.ErrorDetails);
        // EngineWrapper.Engine.Script.console.error(e.ScriptException);

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
      LoadScriptsFromMod(Utils.JSForBarotraumaPackage);

      foreach (ContentPackage package in ContentPackageManager.EnabledPackages.All)
      {
        if (package == Utils.JSForBarotraumaPackage) continue;
        if (ModUsesJS(package)) LoadScriptsFromMod(package);
      }
    }

    public bool ModUsesJS(ContentPackage package)
    {
      return Directory.Exists(
        Path.Combine(package.Dir, AutorunPath)
      );
    }

    public ScriptLoader(EngineWrapper engineWrapper) => EngineWrapper = engineWrapper;
  }

}