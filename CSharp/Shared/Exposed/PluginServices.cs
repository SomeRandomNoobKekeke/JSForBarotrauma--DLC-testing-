
using Barotrauma;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JSForBarotrauma
{
  public class PluginServices
  {
    public IDebugConsole DebugConsole { get; }
    public ISettingsService SettingsService { get; }
    public IItemComponentRegistrar ItemComponentRegistrar { get; }
    public ISimpleHookService HookService { get; }
    public IHarmonyProvider HarmonyProvider { get; }
    public IContentFileRegistrar ContentFileRegistrar { get; }
    public IGameNetwork GameNetwork { get; }
    public IStatusEffectService StatusEffectService { get; }


    // Cursed initialization
    public PluginServices()
    {
      DebugConsole = Mod.DebugConsole;
      SettingsService = Mod.SettingsService;
      ItemComponentRegistrar = Mod.ItemComponentRegistrar;
      HookService = Mod.HookService;
      HarmonyProvider = Mod.HarmonyProvider;
      ContentFileRegistrar = Mod.ContentFileRegistrar;
      GameNetwork = Mod.GameNetwork;
      StatusEffectService = Mod.StatusEffectService;
    }
  }
}