
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
using WebSocketSharp.Server;

namespace JSForBarotrauma
{
  public class ServerManager
  {
    public Dictionary<int, JSHttpServer> RunningHttpServers { get; } = new();
    public Dictionary<int, WebSocketServer> RunningWSServers { get; } = new();


    public JSHttpServer StartHttpServer(string root, int port)
    {
      if (RunningHttpServers.ContainsKey(port))
      {
        throw new Exception($"Http server at [{port}] is already running");
      }

      if (!Directory.Exists(root))
      {
        throw new Exception($"No such directory -> can't serve it: [{root}]");
      }

      var server = new JSHttpServer(root, port);
      server.Run();
      RunningHttpServers[port] = server;
      return server;
    }

    public void StopHttpServer(int port)
    {
      if (!RunningHttpServers.ContainsKey(port))
      {
        throw new Exception($"No server running at [{port}]");
      }

      RunningHttpServers[port].Stop();
      RunningHttpServers.Remove(port);
    }



    public WebSocketServer StartWSServer(int port)
    {
      if (RunningWSServers.ContainsKey(port))
      {
        throw new Exception($"WS server at [{port}] is already running");
      }

      var wssv = new WebSocketServer($"ws://localhost:{port}/");


      wssv.AddWebSocketService<EchoWS>("/");
      RunningWSServers[port] = wssv;
      wssv.Start();
      return wssv;
    }

    public void StopWSServer(int port)
    {
      if (!RunningWSServers.ContainsKey(port))
      {
        throw new Exception($"No WS server running at [{port}]");
      }

      RunningWSServers[port].Stop();
      RunningWSServers.Remove(port);
    }


    public void Clear()
    {
      foreach (var server in RunningHttpServers.Values)
      {
        server.Stop();
      }
      RunningHttpServers.Clear();

      foreach (var server in RunningWSServers.Values)
      {
        server.Stop();
      }
      RunningWSServers.Clear();
    }
  }
}