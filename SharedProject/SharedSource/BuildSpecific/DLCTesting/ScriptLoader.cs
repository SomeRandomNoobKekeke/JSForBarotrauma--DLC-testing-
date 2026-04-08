
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
using System.IO;
using BaroJunk;

namespace JSForBarotrauma
{
  public partial class ScriptLoader
  {
    public void LoadScriptsFromMod(ContentPackage package)
    {
      if (package == null) return;
      LoadScriptsFromMod(Path.Combine(Directory.GetCurrentDirectory(), package.Dir));
    }
  }
}