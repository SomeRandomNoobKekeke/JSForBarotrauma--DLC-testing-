
using System;
using System.Reflection;
using System.Linq;
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
  public class PostfixParamTable
  {
    public bool ResultWasModified { get; private set; }
    private object result;
    public object Result
    {
      get => result;
      set
      {
        result = value;
        ResultWasModified = true;
      }
    }

    public PostfixParamTable(object result) => this.result = result;
  }

  public class PTable
  {
    public bool PreventExecution { get; set; }

    public object Result { get; set; }
  }

}