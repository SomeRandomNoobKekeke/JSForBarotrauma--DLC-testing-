
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
      ["StartHttpServer"] = (string root, int port = 7000) => StartHttpServer(root, port),
      ["StopHttpServer"] = (int port = 7000) => StopHttpServer(port),
#if CLIENT
      ["OpenURLInSteam"] = (string url) => OpenURLInSteam(url),
      ["OpenURL"] = (string url) => OpenURL(url),
#endif
    };

    public static JSServer StartHttpServer(string root, int port = 7000)
      => Mod.ServerManager.StartHttpServer(root, port);

    public static void StopHttpServer(int port = 7000)
      => Mod.ServerManager.StopHttpServer(port);

    public static bool IsValidURL(string url) => Utils.IsValidURL(url).IsFailed;

#if CLIENT
    public static void OpenURLInSteam(string url)
    {
      if (Utils.IsValidURL(url).IsFailed)
      {
        UnifiedConsole.Error(IsValidURL(url).Errors.First().Message);
        return;
      }

      SteamManager.OverlayCustomUrl(url);
    }


    public static void OpenURL(string url)
    {
      if (Utils.IsValidURL(url).IsFailed)
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