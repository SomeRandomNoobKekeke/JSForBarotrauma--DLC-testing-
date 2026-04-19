
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
    public HashSet<JSServer> RunningServers { get; } = new();

    public JSServer Start(string root, int port)
    {
      UnifiedConsole.Log("123");
      var server = new JSServer(root, port);
      UnifiedConsole.Log("123");
      server.Run();
      RunningServers.Add(server);
      return server;
    }

    public void Dispose()
    {
      foreach (var server in RunningServers)
      {
        server.Stop();
      }
      RunningServers.Clear();
    }

    // public static CancellationTokenSource Spam()
    // {
    //   CancellationTokenSource cancelTokenSource = new();
    //   CancellationToken token = cancelTokenSource.Token;

    //   Task.Factory.StartNew(() => _Spam(token), token);
    //   return cancelTokenSource;
    // }

    // private static async Task _Spam(CancellationToken token)
    // {
    //   for (int i = 0; i < 30; i++)
    //   {
    //     if (token.IsCancellationRequested)
    //     {
    //       UnifiedConsole.Log($"Canceled");
    //       token.ThrowIfCancellationRequested();
    //     }

    //     UnifiedConsole.Log($"Bruh {i}");
    //     await Task.Delay(1000);
    //   }
    // }
  }
}