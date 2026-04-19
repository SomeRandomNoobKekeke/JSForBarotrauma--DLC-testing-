
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
  public class JSServer : Qoollo.Net.Http.HttpServer
  {
    public JSServer(string root, int port) : base(port)
    {
      Get["/"] = _ => "Hello world!";

      ServeStatic(new DirectoryInfo(root), "static");
    }
  }
}