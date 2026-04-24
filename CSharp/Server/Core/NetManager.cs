
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

    public void Broadcast(string header, string data) => Send(header, data, null);
    public void Send(string header, string data, Client client)
    {
      if (GameMain.IsSingleplayer) return;

      IWriteMessage msg = LuaCsSetup.Instance.Networking.Start(JSHeader);
      msg.WriteString(header);
      msg.WriteString(data);

      LuaCsSetup.Instance.Networking.Send(msg, client?.Connection);
    }

    public void DoHandshake()
    {
      ListenFor("__jshandshake", (string data, Client client) =>
      {
        Send("__jshandshake", "hi", client);
      });

      Broadcast("__jshandshake", "hi");
    }

    public void EmptyHandler(IReadMessage msg, Client connection) { }
    public void MessageHandler(IReadMessage msg, Client connection)
    {
      string header = msg.ReadString();
      string data = msg.ReadString();

      if (Listeners.ContainsKey(header))
      {
        foreach (Action<string, Client> listener in Listeners[header])
        {
          listener.Invoke(data, connection);
        }
      }
    }
  }
}