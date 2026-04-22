
using BaroJunk;
using Barotrauma;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace JSForBarotrauma
{
  public interface IPatchTracker
  {
    public bool WasPatched(MethodBase original);
    public int Add(MethodBase original, Delegate patch, int priority = Priority.Normal);
    public void Remove(MethodBase original, int ID);
    public void Clear();
  }



}