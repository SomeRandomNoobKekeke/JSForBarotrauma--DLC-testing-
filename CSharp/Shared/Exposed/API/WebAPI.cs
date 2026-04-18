
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
using Barotrauma.Steam;

namespace JSForBarotrauma
{
  public static class WebAPI
  {
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["IsValidURL"] = (string url) => IsValidURL(url),
#if CLIENT
      ["OpenURLInSteam"] = (string url) => OpenURLInSteam(url),
      ["OpenURL"] = (string url) => OpenURL(url),
#endif
    };

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

#if CLIENT
    public static void OpenURLInSteam(string url)
    {
      if (IsValidURL(url).IsFailed)
      {
        UnifiedConsole.Error(IsValidURL(url).Errors.First().Message);
        return;
      }

      SteamManager.OverlayCustomUrl(url);
    }


    public static void OpenURL(string url)
    {
      if (IsValidURL(url).IsFailed)
      {
        UnifiedConsole.Error(IsValidURL(url).Errors.First().Message);
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