
using System;
using System.Collections;
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
  public static class UtilsAPI
  {
    public static PropertyBag ToBag() => new PropertyBag()
    {
      ["GetStackTrace"] = () => GetStackTrace(),
      ["ToJSArray"] = (IEnumerable csEnumerable) => ToJSArray(csEnumerable),
    };

    public static StackTrace GetStackTrace() => Utils.GetStackTrace();

    public static ScriptObject ToJSArray(IEnumerable csEnumerable)
      => Utils.ToJSArray(csEnumerable);

  }
}