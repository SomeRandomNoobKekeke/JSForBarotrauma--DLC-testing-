
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

namespace JSForBarotrauma
{
  public class ServerManager : IDisposable
  {
    public Dictionary<int, JSServer> RunningHttpServers { get; } = new();

    public JSServer StartHttpServer(string root, int port)
    {
      if (RunningHttpServers.ContainsKey(port))
      {
        throw new Exception($"Http server at [{port}] is already running");
      }

      if (!Directory.Exists(root))
      {
        throw new Exception($"No such directory -> can't serve it: [{root}]");
      }

      var server = new JSServer(root, port);
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
    }

    public void Dispose()
    {
      foreach (var server in RunningHttpServers.Values)
      {
        server.Stop();
      }
      RunningHttpServers.Clear();
    }
  }
}