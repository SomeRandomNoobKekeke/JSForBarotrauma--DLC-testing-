
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
using WatsonWebsocket;
using System.Text;

namespace JSForBarotrauma
{
  public class ServerManager
  {
    public Dictionary<int, Qoollo.Net.Http.HttpServer> HttpServers { get; } = new();
    public Dictionary<int, WatsonWsServer> WSServers { get; } = new();


    public bool HasHttpServer(int port) => HttpServers.ContainsKey(port);

    public HttpServerBag CreateHttpServer(int port)
    {
      if (HttpServers.ContainsKey(port))
      {
        throw new Exception($"Http server at [{port}] is already running");
      }

      HttpServers[port] = new Qoollo.Net.Http.HttpServer(port);
      return new HttpServerBag(HttpServers[port]);
    }

    public bool RemoveHttpServer(int port)
    {
      if (HttpServers.ContainsKey(port))
      {
        if (HttpServers[port].IsListening)
        {
          HttpServers[port].Stop();
        }
        HttpServers.Remove(port);
        return true;
      }

      return false;
    }


    public bool HasWSServer(int port) => WSServers.ContainsKey(port);

    public WSBag CreateWSServer(int port)
    {
      if (WSServers.ContainsKey(port))
      {
        throw new Exception($"WS server at [{port}] is already running");
      }

      WSServers[port] = new WatsonWsServer("localhost", port, ssl: false);

      return new WSBag(WSServers[port]);
    }




    public bool RemoveWSServer(int port)
    {
      if (!WSServers.ContainsKey(port))
      {
        return false;
      }

      WSServers[port].Stop();
      WSServers.Remove(port);
      return true;
    }


    public void Clear()
    {
      foreach (var server in HttpServers.Values)
      {
        server.Stop();
      }
      HttpServers.Clear();

      foreach (var server in WSServers.Values)
      {
        if (server.IsListening) server.Stop();
      }
      WSServers.Clear();
    }
  }
}