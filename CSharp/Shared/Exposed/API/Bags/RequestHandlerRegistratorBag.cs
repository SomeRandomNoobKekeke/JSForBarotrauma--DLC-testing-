
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

using System.Net;
using Qoollo.Net.Http;

namespace JSForBarotrauma
{
  public class RequestHandlerRegistratorBag : ProxyBag
  {
    public RequestHandlerRegistrator Registrator { get; }

    public RequestHandlerRegistratorBag(RequestHandlerRegistrator registrator)
    {
      Registrator = registrator;

      Get = (key) => registrator[key];
      Set = (key, value) => registrator[key] = Mod.Engine.HostFunctions.del<Func<HttpListenerRequest, string>>(value);
      Has = (key) => registrator.Handlers.ContainsKey(key);
      GetKeys = () => registrator.Handlers.Keys;
    }
  }



}