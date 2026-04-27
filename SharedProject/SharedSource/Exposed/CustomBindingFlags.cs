
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
using System.IO;
using System.Reflection;

namespace JSForBarotrauma
{

  public static class CustomBindingFlags
  {
    public static BindingFlags Instance => BindingFlags.Instance;
    public static BindingFlags Static => BindingFlags.Static;
    public static BindingFlags Public => BindingFlags.Public;
    public static BindingFlags NonPublic => BindingFlags.NonPublic;

    public static BindingFlags PublicInstance => BindingFlags.Instance | BindingFlags.Public;
    public static BindingFlags PublicStatic => BindingFlags.Static | BindingFlags.Public;
    public static BindingFlags PrivateInstance => BindingFlags.Instance | BindingFlags.NonPublic;
    public static BindingFlags PrivateStatic => BindingFlags.Static | BindingFlags.NonPublic;

    public static BindingFlags AllStatic => BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    public static BindingFlags AllInstance => BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    public static BindingFlags All => BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public static BindingFlags FromInt(int value) => (BindingFlags)value;
  }
}