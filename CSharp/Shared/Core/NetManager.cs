
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
    public static string JSHeader = "JS";

    public void Send(string header, string data)
    {
      if (GameMain.IsSingleplayer) return;

      IWriteMessage msg = LuaCsSetup.Instance.Networking.Start(JSHeader);
      msg.WriteString(header);
      msg.WriteString(data);
      LuaCsSetup.Instance.Networking.Send(msg);
    }

    public void Init()
    {
      if (GameMain.IsSingleplayer) return;
      LuaCsSetup.Instance.Networking.Receive(JSHeader, MessageHandler);
    }

    public void Dispose()
    {
      if (GameMain.IsSingleplayer) return;
      Listeners.Clear();
      LuaCsSetup.Instance.Networking.Receive(JSHeader, EmptyHandler);
    }
  }
}