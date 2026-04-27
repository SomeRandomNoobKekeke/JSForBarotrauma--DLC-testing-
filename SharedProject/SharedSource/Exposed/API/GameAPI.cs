
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
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["IsClient"] = GameMain.IsSingleplayer || GameMain.NetworkMember.IsClient,
      ["IsServer"] = GameMain.IsMultiplayer && GameMain.NetworkMember.IsServer,
      ["IsSingleplayer"] = GameMain.IsSingleplayer,
      ["IsMultiplayer"] = GameMain.IsMultiplayer,
    };
  }
}