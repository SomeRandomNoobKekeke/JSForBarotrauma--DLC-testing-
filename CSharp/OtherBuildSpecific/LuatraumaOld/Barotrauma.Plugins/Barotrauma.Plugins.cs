
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Barotrauma;
using Microsoft.Xna.Framework;
using BaroJunk;
using HarmonyLib;

namespace Barotrauma.Plugins
{
  // Search "Barotrauma.Plugins compatibility" to find all code that needs to be changed
  public interface IBarotraumaPlugin : IAssemblyPlugin { }

  public enum CommandFlags
  {
    None = 0,
    IsCheat = 1,
    DoNotRelayToServer = 2
  }

  public interface IDebugConsole
  {
    public delegate void OnCommandExecutedDelegate(string[] args);

    public void NewMessage(string msg, Color color);

    public void RegisterCommand(string command, string helpMessage, CommandFlags flags, Action<string[]> onCommandExecuted, Func<string[][]> getValidArgs = null);
    public void DeregisterCommand(string command);
  }


  public interface ISettingsService { }
  public interface IItemComponentRegistrar { }
  public interface ISimpleHookService { }
  public interface IHarmonyProvider
  {
    public Harmony GetHarmony();
  }
  public interface IContentFileRegistrar { }
  public interface IGameNetwork { }
  public interface IStatusEffectService { }



  public class DebugConsoleProxy : IDebugConsole
  {
    public void NewMessage(string msg, Color color) => DebugConsole.NewMessage(msg, color);

    public void RegisterCommand(string command, string helpMessage, CommandFlags flags, Action<string[]> onCommandExecuted, Func<string[][]> getValidArgs = null)
    {
      PluginCommands.Add(command, onCommandExecuted);
    }
    public void DeregisterCommand(string command)
    {
      PluginCommands.Remove(command);
    }
  }

  public class HarmonyProvider : IHarmonyProvider
  {
    public Harmony GetHarmony() => new Harmony("JSForBarotrauma");
  }


  public static class PluginServiceProvider
  {
    public static T GetService<T>() where T : class
    {
      if (typeof(T) == typeof(IDebugConsole)) return new DebugConsoleProxy() as T;
      if (typeof(T) == typeof(IHarmonyProvider)) return new HarmonyProvider() as T;

      return default;
    }
  }

}