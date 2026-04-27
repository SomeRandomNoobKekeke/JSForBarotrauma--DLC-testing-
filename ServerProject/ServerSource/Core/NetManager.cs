
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
    public Dictionary<string, HashSet<Action<string, Client>>> Listeners { get; } = new();

    public void ListenFor(string header, Action<string, Client> listener)
    {
      if (GameMain.IsSingleplayer) return;

      if (!Listeners.ContainsKey(header)) Listeners[header] = new();
      Listeners[header].Add(listener);
    }

    public void Broadcast(string header, string data)
    {
      if (GameMain.IsSingleplayer) return;
      Mod.GameNetwork.Broadcast(NetworkHeader.JSNetMsg, new JSNetMessage(header, data));
    }
    public void Send(string header, string data, Client client)
    {
      if (GameMain.IsSingleplayer) return; 
      Mod.GameNetwork.SendToClient(client, NetworkHeader.JSNetMsg, new JSNetMessage(header, data));
    }

    public void DoHandshake()
    {
      ListenFor("__jshandshake", (string data, Client client) =>
      {
        Send("__jshandshake", "hi", client);
      });

      Broadcast("__jshandshake", "hi");
    }


    public void MessageHandler(JSNetMessage msg, Client client)
    {
      if (Listeners.ContainsKey(msg.header))
      {
        foreach (Action<string, Client> listener in Listeners[msg.header])
        {
          listener.Invoke(msg.data, client);
        }
      }
    }

  }
}