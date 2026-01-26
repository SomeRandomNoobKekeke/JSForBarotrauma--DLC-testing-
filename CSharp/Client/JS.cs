
using System;
using System.Reflection;
using Barotrauma;
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
  public class JS : IDisposable
  {
    public V8ScriptEngine Engine;
    public int DebugPort = 9222;

    public bool REPL { get; set; }

    public void ToggleRepl()
    {
      REPL = !REPL;
      Mod.Logger.Log($"JS REPL mode [{Logger.WrapInColor((REPL ? "Enabled" : "Disabled"), "white")}]");
    }

    public void Start()
    {

      Engine = new V8ScriptEngine(V8ScriptEngineFlags.EnableDebugging, DebugPort)
      {
        // AccessContext = typeof(GameMain),
        ExposeHostObjectStaticMembers = true,
        DisableTypeRestriction = true,
        DocumentSettings = new DocumentSettings()
        {
          AccessFlags = DocumentAccessFlags.EnableFileLoading,
          SearchPath = Path.Combine(ModInfo.ModDir<Mod>(), "JS"),

        },
      };

      Engine.AddHostObject("BaroConsole", new BaroConsole());
      Engine.AddHostObject("lib", HostItemFlags.PrivateAccess, new HostTypeCollection("System", "Barotrauma", "mscorlib", "System.Core"));
      Engine.AddHostType(HostItemFlags.PrivateAccess, typeof(GameMain));
      Engine.AddHostType(HostItemFlags.PrivateAccess, typeof(LuaCsSetup));

      // Engine.AddHostObject("Engine", Engine);

      Engine.AddHostObject("JS", this);

    }

    public void Stop()
    {
      Dispose();
    }

    public void Restart()
    {
      Stop();
      Start();
      Mod.Logger.Log($"JS from Barotrauma restarted");
    }

    public void Execute(string command)
    {
      Mod.Logger.Print($">> {command}", Color.White);

      try
      {
        Mod.Logger.Log(Engine.Evaluate(command));
      }
      catch (ScriptEngineException e)
      {
        Mod.Logger.Error(e.ErrorDetails);
      }
    }

    public void LoadDirectory(string dir)
    {
      foreach (string file in Directory.GetFiles(dir, "*.js"))
      {
        LoadFile(file);
      }
    }

    public void LoadFile(string path)
    {
      try
      {
        Engine.ExecuteDocument(path, ModuleCategory.CommonJS);
        // Engine.Execute(File.ReadAllText(path));
      }
      catch (ScriptEngineException e)
      {
        Mod.Logger.Error($"{path}\n{e.ErrorDetails}");
      }

    }

    public void Dispose()
    {
      Engine.Dispose();
      Engine = null;
    }
  }
}