
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
using Barotrauma.LuaCs;
using Barotrauma.Networking;

namespace JSForBarotrauma
{
  public static partial class NetAPI
  {
    public static IPropertyBag ToBag() => new PropBag(new Dictionary<string, PropBag.Prop>()
    {
      ["Send"] = new PropBag.Prop(
        () => (string header, string data) => Mod.Engine.NetManager.Send(header, data)
      ),
      ["ListenFor"] = new PropBag.Prop(
        () => (string header, object scriptFunc) => ListenFor(header, scriptFunc)
      ),
      ["OnConnected"] = new PropBag.Prop(
        set: (object scriptFunc) => AddOnConnected(scriptFunc)
      ),
    });


    public static void ListenFor(string header, object scriptFunc)
    {
      Mod.Engine.NetManager.ListenFor(
        header,
        Mod.Engine.HostFunctions.del<Action<string>>(scriptFunc)
      );
    }

    public static void AddOnConnected(object scriptFunc)
    {
      Mod.Engine.NetManager.OnConnected.Add(
        Mod.Engine.HostFunctions.del<Action>(scriptFunc)
      );
    }
  }
}