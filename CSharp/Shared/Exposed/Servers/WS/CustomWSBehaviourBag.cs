
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

using WebSocketSharp;
using WebSocketSharp.Server;

namespace JSForBarotrauma
{
  public class CustomWSBehaviourBag : ProxyBag
  {
    public CustomWSBehaviour Behaviour { get; }
    public WebSocketServer Server { get; }

    public Dictionary<string, Func<object>> Getters = new();
    public Dictionary<string, Action<object>> Setters = new();

    public void AddProp(string name, Func<object> get = null, Action<object> set = null)
    {
      if (get != null) Getters[name] = get;
      if (set != null) Setters[name] = set;
    }

    public CustomWSBehaviourBag(CustomWSBehaviour behaviour, WebSocketServer server)
    {
      Behaviour = behaviour;
      Server = server;

      OnGet = (key) =>
      {
        if (Getters.ContainsKey(key)) return Getters[key]();
        else return null;
      };
      OnSet = (key, value) =>
      {
        if (Setters.ContainsKey(key)) Setters[key](value);
      };

      AddProp("onMessage",
        () => Behaviour.MessageCallback,
        (value) => Behaviour.MessageScriptFunc = value
      );

      AddProp("onOpen",
        () => Behaviour.OpenCallback,
        (value) => Behaviour.OpenScriptFunc = value
      );

      AddProp("onClose",
        () => Behaviour.CloseCallback,
        (value) => Behaviour.CloseScriptFunc = value
      );

      AddProp("onError",
        () => Behaviour.ErrorCallback,
        (value) => Behaviour.ErroScriptFunc = value
      );

      AddProp("Stop",
        () => () => Server.Stop()
      );

      AddProp("Start",
        () => () => Server.Start()
      );

      AddProp("Send",
        () => (string data) => Behaviour.Send(data)
      );

      Hints = Getters.Keys.ToHashSet();
    }
  }



}