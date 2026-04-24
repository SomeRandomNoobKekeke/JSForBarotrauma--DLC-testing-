
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
  public static class NetAPI
  {
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["Send"] = (string header, string data) => Send(header, data),
      ["ListenFor"] = (string header, object scriptFunc) => ListenFor(header, scriptFunc),
    };

    // https://github.com/evilfactory/LuaCsForBarotrauma/blob/8d90ccb4a30af3b43ec9ab58e7bbb45d56ae1267/Barotrauma/BarotraumaShared/SharedSource/LuaCs/_Services/_Interfaces/INetworkingService.cs#L9-L13
    // #if CLIENT
    // public delegate void NetMessageReceived(IReadMessage netMessage);
    // #elif SERVER
    // internal delegate void NetMessageReceived(IReadMessage netMessage, Client connection);
    // #endif

    public static void ListenFor(string header, object scriptFunc)
    {
      LuaCsSetup.Instance.Networking.Receive(
        header,
        Mod.Engine.HostFunctions.del<NetMessageReceived>(scriptFunc)
      );
    }

    public static void Send(string header, string data)
    {
      IWriteMessage msg = LuaCsSetup.Instance.Networking.Start(header);
      msg.WriteString(data);
      LuaCsSetup.Instance.Networking.Send(msg);
    }

  }
}