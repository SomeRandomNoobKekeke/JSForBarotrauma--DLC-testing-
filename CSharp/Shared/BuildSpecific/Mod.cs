
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


namespace JSForBarotrauma
{
  public partial class Mod : IAssemblyPlugin
  {
    public IPluginManagementService PluginService { get; set; }

    public static ContentPackage Package { get; private set; }

    public void Initialize() => Init();

    public partial void InitBuildSpecific()
    {
      PluginService.TryGetPackageForPlugin<Mod>(out ContentPackage package);
      Package = package;
    }
    public partial void DisposeBuildSpecific()
    {
      Package = null;
    }
  }
}