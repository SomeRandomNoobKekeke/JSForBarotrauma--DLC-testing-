
using BaroJunk;
using Barotrauma;
using Barotrauma.Items.Components;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Barotrauma.Plugins;

namespace JSForBarotrauma
{
  public partial class Mod : IBarotraumaPlugin
  {
    public void Initialize() => Init();

    public partial void InitBuildSpecific()
    {

    }
    public partial void DisposeBuildSpecific()
    {

    }

    public void OnContentLoaded()
    {

    }
  }
}