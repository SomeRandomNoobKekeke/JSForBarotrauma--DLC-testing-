
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
using Qoollo.Net.Http;

namespace JSForBarotrauma
{
  public class HttpServerBag : PropertyBag
  {
    public HttpServer Server { get; }

    public HttpServerBag(HttpServer server)
    {
      Server = server;

      this["Port"] = Server.Port;
      this["ServeStatic"] = ServeStatic;
      this["Run"] = () => Server.Run();
      this["Stop"] = () => Server.Stop();
      this["IsRunning"] = () => Server.IsListening;
      this["Get"] = new RequestHandlerRegistratorBag(Server.Get);
      this["Post"] = new RequestHandlerRegistratorBag(Server.Post);
      this["Put"] = new RequestHandlerRegistratorBag(Server.Put);
      this["Delete"] = new RequestHandlerRegistratorBag(Server.Delete);
    }



    public void ServeStatic(string rootDir, string url)
    {
      if (!Directory.Exists(rootDir))
      {
        throw new Exception($"No such directory -> can't serve it: [{rootDir}]");
      }
      Server.ServeStatic(new DirectoryInfo(rootDir), url);
    }
  }
}