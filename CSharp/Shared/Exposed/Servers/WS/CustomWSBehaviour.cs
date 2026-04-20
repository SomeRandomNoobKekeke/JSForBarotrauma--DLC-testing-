
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
  public class CustomWSBehaviour : WebSocketBehavior
  {
    public object MessageScriptFunc
    {
      set
      {
        MessageCallback = Mod.Engine.HostFunctions.del<Action<MessageEventArgs>>(value);
      }
    }
    public object OpenScriptFunc
    {
      set
      {
        OpenCallback = Mod.Engine.HostFunctions.del<Action>(value);
      }
    }
    public object ErroScriptFunc
    {
      set
      {
        ErrorCallback = Mod.Engine.HostFunctions.del<Action<WebSocketSharp.ErrorEventArgs>>(value);
      }
    }
    public object CloseScriptFunc
    {
      set
      {
        CloseCallback = Mod.Engine.HostFunctions.del<Action<WebSocketSharp.CloseEventArgs>>(value);
      }
    }

    public Action<MessageEventArgs> MessageCallback { get; set; }
    public Action OpenCallback { get; set; }
    public Action<WebSocketSharp.ErrorEventArgs> ErrorCallback { get; set; }
    public Action<WebSocketSharp.CloseEventArgs> CloseCallback { get; set; }



    public void Send(string data) => base.Send(data);

    protected override void OnMessage(MessageEventArgs e)
    {
      UnifiedConsole.Log($"WS: message {e}");
      MessageCallback?.Invoke(e);
    }

    protected override void OnOpen()
    {
      UnifiedConsole.Log($"WS: open");
      OpenCallback?.Invoke();
    }

    protected override void OnError(WebSocketSharp.ErrorEventArgs e)
    {
      UnifiedConsole.Log($"WS: error");
      ErrorCallback?.Invoke(e);
    }

    protected override void OnClose(WebSocketSharp.CloseEventArgs e)
    {
      UnifiedConsole.Log($"WS: close {e.Code} {e.Reason} {e.WasClean}");
      CloseCallback?.Invoke(e);
    }

  }
}