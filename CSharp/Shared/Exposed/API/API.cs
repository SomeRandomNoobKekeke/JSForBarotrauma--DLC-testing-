
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
using Barotrauma.Steam;

namespace JSForBarotrauma
{
  public static class API
  {
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["Web"] = WebAPI.ToBag(),
      ["Console"] = ConsoleAPI.ToBag(),
    };
  }
}