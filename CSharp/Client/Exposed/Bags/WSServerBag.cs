
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
using BaroJunk;

using System.Threading;
using System.Threading.Tasks;
using WatsonWebsocket;
using System.Text;

namespace JSForBarotrauma
{
  public class WSServerBag : PropBag
  {
    public WatsonWsServer Server { get; }

    public WSServerBag(WatsonWsServer server)
    {
      Server = server;

      Props["ClientConnected"] = new Prop(
        set: (value) =>
        {
          Action<Guid> callback = Mod.Engine.HostFunctions.del<Action<Guid>>(value);
          if (callback is null) return;
          Server.ClientConnected += (object sender, ConnectionEventArgs args) =>
          {
            callback(args.Client.Guid);
          };
        }
      );

      Props["ClientDisconnected"] = new Prop(
        set: (value) =>
        {
          Action<Guid> callback = Mod.Engine.HostFunctions.del<Action<Guid>>(value);
          if (callback is null) return;
          Server.ClientDisconnected += (object sender, DisconnectionEventArgs args) =>
          {
            callback(args.Client.Guid);
          };
        }
      );

      Props["MessageReceived"] = new Prop(
        set: (value) =>
        {
          Action<Guid, string> callback = Mod.Engine.HostFunctions.del<Action<Guid, string>>(value);
          if (callback is null) return;
          Server.MessageReceived += (object sender, MessageReceivedEventArgs args) =>
          {
            callback(args.Client.Guid, Encoding.UTF8.GetString(args.Data));
          };
        }
      );

      Props["Stop"] = new Prop(
        () => () => Server.Stop()
      );

      Props["Start"] = new Prop(
        () => () => Server.Start()
      );

      Props["Send"] = new Prop(
        () => (Guid clientId, string data) => Server.SendAsync(clientId, data)
      );
    }
  }



}