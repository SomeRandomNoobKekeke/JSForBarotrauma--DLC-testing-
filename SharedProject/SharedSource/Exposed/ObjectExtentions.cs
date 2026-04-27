
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
using System.Threading;
using BaroJunk;
using System.IO;

namespace HiddenNamespace
{
  public static class ObjectExtentions
  {
    public static ReflectionGetter _Get(this object self) => new ReflectionGetter(self);
    public static ReflectionSetter _Set(this object self) => new ReflectionSetter(self);
  }

  public class ReflectionGetter(object Self)
  {
    public Type Type => Self.GetType();

    public FieldInfo FieldInfo(string name)
      => Self.GetType()
             .GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    public object Field(string name)
      => Self.GetType()
             .GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?.GetValue(Self);

    public object PropertyInfo(string name)
      => Self.GetType()
             .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    public object Property(string name)
      => Self.GetType()
             .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?.GetValue(Self);

    public MethodInfo MethodInfo(string name, params Type[] paramTypes)
      => paramTypes.Length == 0
        ? Self.GetType().GetMethod(
            name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
          )
        : Self.GetType().GetMethod(
            name,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            paramTypes
          );

    public ConstructorInfo ConstructorInfo(string name, params Type[] paramTypes)
      => paramTypes.Length == 0
        ? Self.GetType().GetConstructors()[0]
        : Self.GetType().GetConstructor(
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            paramTypes
          );
  }


  public class ReflectionSetter(object Self)
  {
    public void Field(string name, object value)
      => Self.GetType()
             .GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?.SetValue(Self, value);
    public void Property(string name, object value)
      => Self.GetType()
             .GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?.SetValue(Self, value);
  }


}