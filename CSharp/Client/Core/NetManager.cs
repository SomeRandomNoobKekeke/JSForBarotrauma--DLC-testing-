
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
    public Dictionary<string, Action<string>> Listeners { get; } = new();

    public void ListenFor(string header, Action<string> listener)
    {
      if (GameMain.IsSingleplayer) return;
      Listeners[header] = listener;
    }

    public void EmptyHandler(IReadMessage msg) { }
    public void MessageHandler(IReadMessage msg)
    {
      string header = msg.ReadString();
      string data = msg.ReadString();

      if (Listeners.ContainsKey(header))
      {
        Listeners[header].Invoke(data);
      }
    }
  }
}