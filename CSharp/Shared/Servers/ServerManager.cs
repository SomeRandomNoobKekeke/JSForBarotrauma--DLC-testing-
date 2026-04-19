
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
    public Dictionary<int, JSServer> RunningServers { get; } = new();

    public JSServer Start(string root, int port)
    {
      if (RunningServers.ContainsKey(port))
      {
        throw new Exception($"JSServer at [{port}] is already running");
      }

      if (!Directory.Exists(root))
      {
        throw new Exception($"No such directory -> can't serve it: [{root}]");
      }

      var server = new JSServer(root, port);
      server.Run();
      RunningServers[port] = server;
      return server;
    }

    public void Close(int port)
    {
      if (!RunningServers.ContainsKey(port))
      {
        throw new Exception($"no server running at [{port}]");
      }

      RunningServers[port].Stop();
    }

    public void Dispose()
    {
      foreach (var server in RunningServers.Values)
      {
        server.Stop();
      }
      RunningServers.Clear();
    }
  }
}