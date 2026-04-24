
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

namespace HiddenNamespace
{

  public static class ObjectExtentions
  {
    public static object _Field(this object self, string name)
      => self.GetType()
             .GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
             ?.GetValue(self);

    public static object _Property(this object self, string name)
      => self.GetType()
             .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
             ?.GetValue(self);

    public static object _MethodInfo(this object self, string name, params Type[] paramTypes)
      => paramTypes.Length == 0
        ? self.GetType().GetMethod(
                name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
              )
        : self.GetType().GetMethod(
                name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                paramTypes
              );

  }
}