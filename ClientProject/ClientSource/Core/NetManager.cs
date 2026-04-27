
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
using Barotrauma.Networking;

namespace JSForBarotrauma
{
  public partial class NetManager
  {
    public Dictionary<string, HashSet<Action<string>>> Listeners { get; } = new();

    public bool Connected { get; private set; }
    public ClearableEvent OnConnected { get; } = new();

    public void ListenFor(string header, Action<string> listener)
    {
      if (GameMain.IsSingleplayer) return;

      if (!Listeners.ContainsKey(header)) Listeners[header] = new();
      Listeners[header].Add(listener);
    }


    public void Send(string header, string data)
    {
      if (GameMain.IsSingleplayer) return;

      Mod.GameNetwork.Send(NetworkHeader.JSNetMsg, new JSNetMessage(header, data));
    }

    public void DoHandshake()
    {
      Connected = false;
      ListenFor("__jshandshake", (string data) =>
      {
        if (Connected) return;
        Connected = true;
        OnConnected.Raise();
      });
      Send("__jshandshake", "hi");
    }



    public void MessageHandler(JSNetMessage msg)
    {
      if (Listeners.ContainsKey(msg.header))
      {
        foreach (Action<string> listener in Listeners[msg.header])
        {
          listener.Invoke(msg.data);
        }
      }
    }

    public NetManager()
    {
      OnConnected.OnSubscribed += (handler) =>
      {
        if (Connected) handler.Invoke();
        UnifiedConsole.Log("called OnConnected postfactum");
      };
    }
  }
}