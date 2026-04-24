
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
    public static void ListenFor(string header, object scriptFunc)
    {
      Mod.Engine.NetManager.ListenFor(
        header,
        Mod.Engine.HostFunctions.del<Action<string, Client>>(scriptFunc)
      );
    }

    public static void Send(string header, string data)
      => Mod.Engine.NetManager.Send(header, data);
  }
}