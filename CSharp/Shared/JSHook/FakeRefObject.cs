using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Barotrauma;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using System.Runtime.CompilerServices;
using System.IO;
using BaroJunk;


namespace JSForBarotrauma
{
  public class FakeRefObject
  {
    public bool WasModified { get; private set; }
    private object _value;
    public object Value
    {
      get => _value;
      set
      {
        _value = value;
        WasModified = true;
      }
    }

    public void Init(object value)
    {
      _value = value;
      WasModified = false;
    }

    public void Clear() => _value = null;

    public void MapBack(ref object param)
    {
      if (WasModified) param = _value;
    }
  }
}

