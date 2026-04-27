
using BaroJunk;
using Barotrauma;
using Barotrauma.Networking;
using Barotrauma.Plugins;
using HarmonyLib;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace JSForBarotrauma
{
  public class JSStatusEffectAction : IStatusEffectAction
  {
    public string Name { get; }
    public StatusEffect Effect { get; }
    public XElement Element { get; } 
    public JSStatusEffectAction(StatusEffect statusEffect, XElement element)
    {
      Name = element.GetAttributeString("name", "");
      Effect = statusEffect;
      Element = element;
    }

    public void Apply(StatusEffectParams effectParams)
    {
      Mod.Engine.XMLHookManager.Call(Name, new object[]
      {
        Effect,
        effectParams.DeltaTime,
        effectParams.Entity,
        effectParams.Targets,
        effectParams.WorldPosition,
        Element
      });
    }
  }

  public class XMLHookManager
  {
    public Dictionary<string, Action<object[]>> AddedHooks { get; } = new();


    public static void RegisterJSHook()
    {
      Mod.StatusEffectService.RegisterAction(new StatusEffectActionFactory("Hook",
        (StatusEffect statusEffect, XElement element) => new JSStatusEffectAction(statusEffect, element)));
    }

    public void Call(string name, object[] args)
    {
      if (AddedHooks.ContainsKey(name))
      {
        AddedHooks[name].Invoke(args);
      }
    }

    public void Add(string name, Action<object[]> callback)
    {
      if (AddedHooks.ContainsKey(name))
      {
        throw new Exception($"{name} hook already added");
      }

      AddedHooks[name] = callback;
    }

    public bool Has(string name) => AddedHooks.ContainsKey(name);

    public void Remove(string name)
    {
      AddedHooks.Remove(name);
    }

    public void Clear()
    {
      AddedHooks.Clear();
    }
  }
}