
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
  public class CustomWSBehaviourBag : PropertyBag
  {

  }

  public class CustomWSBehaviour : WebSocketBehavior
  {


    protected override void OnMessage(MessageEventArgs e)
    {
      UnifiedConsole.Log($"WS: Recieved [{e.Data}]");
      Send($"Echo: {e.Data}");
    }

    protected override void OnOpen()
    {
      UnifiedConsole.Log($"WS: Open");
    }

    protected override void OnError(WebSocketSharp.ErrorEventArgs e)
    {
      UnifiedConsole.Log($"WS: Error [{e}]");
    }

    protected override void OnClose(WebSocketSharp.CloseEventArgs e)
    {
      UnifiedConsole.Log($"WS: Closed [{e}]");
    }

  }
}