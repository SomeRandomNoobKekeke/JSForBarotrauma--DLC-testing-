
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

  //TODO make it use PropBag, make PropBag not useless
  public class WSBag : ProxyBag
  {
    public class ProxyProp
    {
      public Func<object> Getter { get; }
      public Action<object> Setter { get; }

      public object Value
      {
        get => Getter();
        set => Setter(value);
      }

      public ProxyProp(Func<object> get = null, Action<object> set = null)
      {
        Getter = get ?? new Func<object>(() => null);
        Setter = set ?? new Action<object>((value) => { });
      }
    }

    public WatsonWsServer Server { get; }

    public Dictionary<string, ProxyProp> FakeProps { get; }

    public WSBag(WatsonWsServer server)
    {
      Server = server;

      OnGet = (key) =>
      {
        if (!FakeProps.ContainsKey(key)) return null;
        return FakeProps[key].Value;
      };
      OnSet = (key, value) =>
      {
        if (!FakeProps.ContainsKey(key)) return;
        FakeProps[key].Value = value;
      };

      FakeProps = new Dictionary<string, ProxyProp>()
      {
        ["ClientConnected"] = new ProxyProp(
          set: (value) =>
          {
            Action<Guid> callback = Mod.Engine.HostFunctions.del<Action<Guid>>(value);
            if (callback is null) return;
            Server.ClientConnected += (object sender, ConnectionEventArgs args) =>
            {
              callback(args.Client.Guid);
            };
          }
        ),
        ["ClientDisconnected"] = new ProxyProp(
          set: (value) =>
          {
            Action<Guid> callback = Mod.Engine.HostFunctions.del<Action<Guid>>(value);
            if (callback is null) return;
            Server.ClientDisconnected += (object sender, DisconnectionEventArgs args) =>
            {
              callback(args.Client.Guid);
            };
          }
        ),
        ["MessageReceived"] = new ProxyProp(
          set: (value) =>
          {
            Action<Guid, string> callback = Mod.Engine.HostFunctions.del<Action<Guid, string>>(value);
            if (callback is null) return;
            Server.MessageReceived += (object sender, MessageReceivedEventArgs args) =>
            {
              callback(args.Client.Guid, Encoding.UTF8.GetString(args.Data));
            };
          }
        ),
        ["Stop"] = new ProxyProp(
          () => () => Server.Stop()
        ),
        ["Start"] = new ProxyProp(
          () => () => Server.Start()
        ),
        ["Send"] = new ProxyProp(
          () => (Guid clientId, string data) => Server.SendAsync(clientId, data)
        ),
      };


      Hints = FakeProps.Keys.ToHashSet();
    }
  }



}