
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
  /// <summary>
  /// WTF is this???
  /// this is fake IPropertyBag object that is passed to js when you create new WS
  /// It provides access to both the server and CustomWSBehaviour instance
  /// Also CustomWSBehaviour is created by the server itself and injected here later
  /// It's cringe and idk what i can do to make it less cringe
  /// </summary>
  public class CustomWSBehaviourBag : ProxyBag
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


    public WebSocketServer Server { get; }
    private CustomWSBehaviour behaviour; public CustomWSBehaviour Behaviour
    {
      get => behaviour;
      set
      {
        behaviour = value;
        SyncValuesWithBehaviour(behaviour);
      }
    }
    private void SyncValuesWithBehaviour(CustomWSBehaviour behaviour)
    {
      if (MessageScriptFunc != null) behaviour.MessageScriptFunc = MessageScriptFunc;
      if (OpenScriptFunc != null) behaviour.OpenScriptFunc = OpenScriptFunc;
      if (CloseScriptFunc != null) behaviour.CloseScriptFunc = CloseScriptFunc;
      if (ErroScriptFunc != null) behaviour.ErroScriptFunc = ErroScriptFunc;
    }


    private object _MessageScriptFunc; public object MessageScriptFunc
    {
      get => _MessageScriptFunc;
      set
      {
        _MessageScriptFunc = value;
        if (Behaviour != null) Behaviour.MessageScriptFunc = value;
      }
    }

    private object _OpenScriptFunc; public object OpenScriptFunc
    {
      get => _OpenScriptFunc;
      set
      {
        _OpenScriptFunc = value;
        if (Behaviour != null) Behaviour.OpenScriptFunc = value;
      }
    }

    private object _CloseScriptFunc; public object CloseScriptFunc
    {
      get => _CloseScriptFunc;
      set
      {
        _CloseScriptFunc = value;
        if (Behaviour != null) Behaviour.CloseScriptFunc = value;
      }
    }

    private object _ErroScriptFunc; public object ErroScriptFunc
    {
      get => _ErroScriptFunc;
      set
      {
        _ErroScriptFunc = value;
        if (Behaviour != null) Behaviour.ErroScriptFunc = value;
      }
    }

    public Dictionary<string, ProxyProp> FakeProps { get; }

    public CustomWSBehaviourBag(WebSocketServer server)
    {
      Server = server;

      OnGet = (key) =>
      {
        if (!FakeProps.ContainsKey(key)) return null;
        return FakeProps[key].Value;
      };
      OnSet = (key, value) =>
      {
        if (FakeProps.ContainsKey(key)) return;
        FakeProps[key].Value = value;
      };

      FakeProps = new Dictionary<string, ProxyProp>()
      {
        ["onMessage"] = new ProxyProp(
          () => MessageScriptFunc,
          (value) => MessageScriptFunc = value
        ),
        ["onOpen"] = new ProxyProp(
          () => OpenScriptFunc,
          (value) => OpenScriptFunc = value
        ),
        ["onClose"] = new ProxyProp(
          () => CloseScriptFunc,
          (value) => CloseScriptFunc = value
        ),
        ["onError"] = new ProxyProp(
          () => ErroScriptFunc,
          (value) => ErroScriptFunc = value
        ),
        ["Stop"] = new ProxyProp(
          () => () => Server.Stop()
        ),
        ["Start"] = new ProxyProp(
          () => () => Server.Start()
        ),
        ["Send"] = new ProxyProp(
          () => (string data) => Behaviour?.Send(data)
        ),
      };


      Hints = FakeProps.Keys.ToHashSet();
    }
  }



}