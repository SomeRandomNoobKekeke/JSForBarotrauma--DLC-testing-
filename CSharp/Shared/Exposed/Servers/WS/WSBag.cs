
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

namespace JSForBarotrauma
{

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
            Server.ClientConnected += Mod.Engine.HostFunctions.del<EventHandler<ConnectionEventArgs>>(value);
          }
        ),
        ["ClientDisconnected"] = new ProxyProp(
          set: (value) =>
          {
            Server.ClientDisconnected += Mod.Engine.HostFunctions.del<EventHandler<DisconnectionEventArgs>>(value);
          }
        ),
        ["MessageReceived"] = new ProxyProp(
          set: (value) =>
          {
            Server.MessageReceived += Mod.Engine.HostFunctions.del<EventHandler<MessageReceivedEventArgs>>(value);
          }
        ),
        ["Stop"] = new ProxyProp(
          () => () => Server.Stop()
        ),
        ["Start"] = new ProxyProp(
          () => () => Server.Start()
        ),
      };


      Hints = FakeProps.Keys.ToHashSet();
    }
  }



}