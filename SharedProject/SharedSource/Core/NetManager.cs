
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

  public enum NetworkHeader
  {
    JSNetMsg
  }

  [NetworkSerialize]
  public readonly record struct JSNetMessage(string header, string data) : INetSerializableStruct;


  public partial class NetManager
  {
    public static string JSHeader = "JS";

    public void Init()
    {
      if (GameMain.IsSingleplayer) return;

      Mod.GameNetwork.RegisterNetworkHeaders<NetworkHeader>();
      Mod.GameNetwork.RegisterHandler<NetworkHeader, JSNetMessage>(NetworkHeader.JSNetMsg, MessageHandler);


      DoHandshake();
    }


    public void Dispose()
    {
      if (GameMain.IsSingleplayer) return;
      Listeners.Clear();
#if CLIENT
      OnConnected.Clear();
#endif
      
    }
  }
}