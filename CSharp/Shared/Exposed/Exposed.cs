
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
using System.Threading;
using BaroJunk;


namespace JSForBarotrauma
{
  public partial class EngineWrapper
  {
    private void ExposeStuff()
    {
      AddExtraObjects();
    }

    public HostFunctions HostFunctions { get; private set; }
    public ExtendedHostFunctions ExtendedHostFunctions { get; private set; }

    private void AddExtraObjects()
    {
      HostFunctions = new();
      ExtendedHostFunctions = new();


      Engine.AddHostObject("host", HostFunctions);
      Engine.AddHostObject("xHost", ExtendedHostFunctions);

      Engine.AddHostType("JS", typeof(JS));
      Engine.AddHostType("Console", typeof(UnifiedConsole));
      Engine.AddHostType("ModInfo", typeof(ModInfo));
      Engine.AddHostType("ObjectExtentions", typeof(HiddenNamespace.ObjectExtentions));


      Engine.AddHostObject("API", API.ToBag());

      Engine.Global["setTimeout"] = (object scriptFunc, int delay) =>
      {
        Utils.RunWithDelay(HostFunctions.del<Action>(scriptFunc), delay);
        return (object)null; //TODO here should be a cancelation token
      };
    }
  }

}