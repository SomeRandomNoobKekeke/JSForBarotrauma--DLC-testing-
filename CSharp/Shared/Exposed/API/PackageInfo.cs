
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
  public static class PackageInfo
  {
    // public static PropertyBag ToBag() => new PropertyBag()
    // {
    //   ["IsValidURL"] = (string url) => IsValidURL(url),
    //   ["OpenURLInSteam"] = (string url) => OpenURLInSteam(url),
    //   ["OpenURL"] = (string url) => OpenURL(url),
    // };
  }
}