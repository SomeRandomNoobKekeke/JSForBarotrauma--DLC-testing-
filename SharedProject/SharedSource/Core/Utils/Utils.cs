
using BaroJunk;
using Barotrauma;
using Barotrauma.Plugins;
using Barotrauma.Steam;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
namespace JSForBarotrauma
{
  public static partial class Utils
  {
    public static ContentPackage JSForBarotraumaPackage
    {
      get
      {
        foreach (PluginLoader.LoadedPlugin plugin in PluginLoader.LoadedPlugins)
        {
          if (plugin.Assembly == Assembly.GetExecutingAssembly())
          {
            return plugin.Info.ContentPackage;
          }
        }

        return null;
      }
    }

    public static void RunWithDelay(Action action, int delay = 100)
    {
      Task.Delay(delay).ContinueWith((t) => action());
    }

    public static FluentResults.Result IsValidURL(string url)
    {
      Uri uriResult;
      if (!Uri.TryCreate(url, UriKind.Absolute, out uriResult))
      {
        return FluentResults.Result.Fail($"it's not a valid url: [{url}]");
      }

      return FluentResults.Result.Ok();
      // return (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) ?
      //   FluentResults.Result.Ok() :
      //   FluentResults.Result.Fail($"it should be http or https scheme: [{url}]");
    }

    public static void PrintAllPatchedMethods()
    {
      foreach (MethodBase mb in Harmony.GetAllPatchedMethods())
      {
        Mod.Logger.Log($"{mb.DeclaringType}.{mb.Name}");

        Patches patches = Harmony.GetPatchInfo(mb);

        if (patches.Prefixes.Count() > 0 || patches.Postfixes.Count() > 0 || patches.Finalizers.Count() > 0)
        {
          Mod.Logger.Log($"{mb.DeclaringType}.{mb.Name}:");
          if (patches.Prefixes.Count() > 0)
          {
            Mod.Logger.Log($"    Prefixes:");
            foreach (Patch patch in patches.Prefixes) { Mod.Logger.Log($"        {patch.owner}"); }
          }

          if (patches.Postfixes.Count() > 0)
          {
            Mod.Logger.Log($"    Postfixes:");
            foreach (Patch patch in patches.Postfixes) { Mod.Logger.Log($"        {patch.owner}"); }
          }

          if (patches.Finalizers.Count() > 0)
          {
            Mod.Logger.Log($"    Finalizers:");
            foreach (Patch patch in patches.Finalizers) { Mod.Logger.Log($"        {patch.owner}"); }
          }
        }
      }
    }

    public static StackTrace GetStackTrace()
    {
      return new StackTrace(new StackTrace(1, true).GetFrames().SkipWhile(
          frame => frame.GetMethod().DeclaringType?.Assembly != typeof(JSHook).Assembly
        )//.Skip(1)
      );
    }

    //I've tried Array.from, it doesn't work well with PropertyBags
    public static ScriptObject ToJSArray(IEnumerable csEnumerable)
    {
      ScriptObject arr = (ScriptObject)Mod.Engine.Engine.Evaluate("[]");

      foreach (object item in csEnumerable)
      {
        arr.InvokeMethod("push", item);
      }

      return arr;
    }

    public static object[] ToCSArray(object scriptArray)
    {
      if (scriptArray is not ScriptObject so) throw new Exception("it's not an array");
      if (so["length"] is Undefined) throw new Exception("it's not an array");

      object[] array = new object[(int)so["length"]];
      for (int i = 0; i < array.Length; i++)
      {
        array[i] = so[i];
      }
      return array;
    }

#if CLIENT
    public static void OpenURLInSteam(string url)
    {
      if (Utils.IsValidURL(url).IsFailed)
      {
        UnifiedConsole.Error(Utils.IsValidURL(url).Errors.First().Message);
        return;
      }

      SteamManager.OverlayCustomUrl(url);
    }


    public static void OpenURL(string url)
    {
      if (Utils.IsValidURL(url).IsFailed)
      {
        UnifiedConsole.Error(Utils.IsValidURL(url).Errors.First().Message);
        return;
      }

      try
      {
        ToolBox.OpenFileWithShell(url);//BRUH why is this client only?
      }
      catch (Exception e)
      {
        UnifiedConsole.Error(e.Message); 
      }
    }
#endif



  }
}