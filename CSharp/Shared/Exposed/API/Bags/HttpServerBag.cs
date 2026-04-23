
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
      this["Get"] = Server.Get.Handlers.ToBag();
      this["Post"] = Server.Post.Handlers.ToBag();
      this["Put"] = Server.Put.Handlers.ToBag();
      this["Delete"] = Server.Delete.Handlers.ToBag();
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