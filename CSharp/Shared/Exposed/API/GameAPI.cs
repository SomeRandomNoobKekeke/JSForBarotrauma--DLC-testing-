
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


namespace JSForBarotrauma
{
  public static class GameAPI
  {
    public static IPropertyBag ToBag() => new PropBag(new Dictionary<string, PropBag.ProxyProp>()
    {
      ["IsClient"] = new PropBag.ProxyProp(
        get: () => IsClient
      ),
      ["IsServer"] = new PropBag.ProxyProp(
        get: () => IsServer
      ),
      ["IsSingleplayer"] = new PropBag.ProxyProp(
        get: () => IsSingleplayer
      ),
      ["IsMultiplayer"] = new PropBag.ProxyProp(
        get: () => IsMultiplayer
      ),
    });

    public static bool IsClient => IsSingleplayer || GameMain.NetworkMember.IsClient;
    public static bool IsServer => IsMultiplayer && GameMain.NetworkMember.IsServer;
    public static bool IsSingleplayer => GameMain.IsSingleplayer;
    public static bool IsMultiplayer => GameMain.IsMultiplayer;
  }
}