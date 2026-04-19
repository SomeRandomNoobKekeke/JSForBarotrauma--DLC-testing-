
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Runtime.CompilerServices;
using System.IO;
using BaroJunk;

using System.Threading.Tasks;

namespace JSForBarotrauma
{
  public static partial class Utils
  {

    // public static partial IEnumerable<Assembly> AllModAssemblies();
    // public static partial ContentPackage JSForBarotraumaPackage { get; }

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


  }
}