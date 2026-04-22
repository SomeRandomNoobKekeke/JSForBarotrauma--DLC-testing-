
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
      ["HasHttpServer"] = (int port) => HasHttpServer(port),
      ["CreateHttpServer"] = (int port) => CreateHttpServer(port),
      ["RemoveHttpServer"] = (int port) => RemoveHttpServer(port),
      ["HasWSServer"] = (int port) => HasWSServer(port),
      ["CreateWSServer"] = (int port) => CreateWSServer(port),
      ["RemoveWSServer"] = (int port) => RemoveWSServer(port),
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

    public static bool HasWSServer(int port)
      => Mod.Engine.ServerManager.HasWSServer(port);
    public static WSBag CreateWSServer(int port)
      => Mod.Engine.ServerManager.CreateWSServer(port);

    public static bool RemoveWSServer(int port)
      => Mod.Engine.ServerManager.RemoveWSServer(port);

    public static bool IsValidURL(string url) => Utils.IsValidURL(url).IsFailed;

#if CLIENT
    public static void OpenURLInSteam(string url) => Utils.OpenURLInSteam(url);
    public static void OpenURL(string url) => Utils.OpenURL(url);
#endif

  }
}