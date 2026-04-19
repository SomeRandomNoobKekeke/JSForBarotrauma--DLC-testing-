
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
using WebSocketSharp.Server;
namespace JSForBarotrauma
{
  public static class WebAPI
  {
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["HasHttpServer"] = (int port) => HasHttpServer(port),
      ["CreateHttpServer"] = (int port) => CreateHttpServer(port),
      ["RemoveHttpServer"] = (int port) => RemoveHttpServer(port),
      ["StartWSServer"] = (int port) => StartWSServer(port),
      ["StopWSServer"] = (int port) => StopWSServer(port),
      ["IsValidURL"] = (string url) => IsValidURL(url),
#if CLIENT
      ["OpenURLInSteam"] = (string url) => OpenURLInSteam(url),
      ["OpenURL"] = (string url) => OpenURL(url),
#endif
    };

    public static bool HasHttpServer(int port)
      => Mod.Engine.ServerManager.HasHttpServer(port);

    public static HttpServerBag CreateHttpServer(int port)
      => Mod.Engine.ServerManager.CreateHttpServer(port);

    public static bool RemoveHttpServer(int port)
      => Mod.Engine.ServerManager.RemoveHttpServer(port);

    public static WebSocketServer StartWSServer(int port)
      => Mod.Engine.ServerManager.StartWSServer(port);

    public static bool StopWSServer(int port)
      => Mod.Engine.ServerManager.StopWSServer(port);

    public static bool IsValidURL(string url) => Utils.IsValidURL(url).IsFailed;

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