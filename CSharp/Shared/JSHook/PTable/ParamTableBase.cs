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
  /// <summary>
  /// not used yet
  /// </summary>
  public class ParamTableBase : IPropertyBag
  {
    public FakeRefObject Arg1 { get; } = new();
    public FakeRefObject Arg2 { get; } = new();
    public FakeRefObject Arg3 { get; } = new();
    public FakeRefObject Arg4 { get; } = new();

    public FakeRefObject Result { get; } = new();

    public Dictionary<string, FakeRefObject> Mapping { get; } = new();

    public object this[string key]
    {
      get => Mapping[key].Value;
      set => Mapping[key].Value = value;
    }

    public ParamTableBase(ParameterInfo[] parameters)
    {
      Mapping["Result"] = Result;

      if (parameters.Length > 0) Mapping[parameters[0].Name] = Arg1;
      if (parameters.Length > 1) Mapping[parameters[1].Name] = Arg2;
      if (parameters.Length > 2) Mapping[parameters[2].Name] = Arg3;
      if (parameters.Length > 3) Mapping[parameters[3].Name] = Arg4;
      if (parameters.Length > 4) throw new Exception("need more params");
    }


    #region IDictionary<string, object>
    public ICollection<string> Keys => Mapping.Keys;
    public ICollection<object> Values => Mapping.Values.Select(o => o.Value).ToList();
    public bool ContainsKey(string key) => Mapping.ContainsKey(key);
    public void Add(string key, object value)
    {
      if (Mapping.ContainsKey(key)) Mapping[key].Value = value;
    }
    public bool Remove(string key) => Mapping.Remove(key);
    public bool TryGetValue(string key, out object value)
    {
      bool result = Mapping.TryGetValue(key, out FakeRefObject o);
      value = o?.Value;
      return result;
    }
    #endregion


    #region ICollection<KeyValuePair<string, object>>
    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> keyValuePair)
      => Add(keyValuePair.Key, keyValuePair.Value);
    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> keyValuePair)
      => ContainsKey(keyValuePair.Key);
    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> keyValuePair)
      => Remove(keyValuePair.Key);
    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int index)
      => throw new Exception("too lazy to implement");

    public void Clear() => Mapping.Clear();
    public int Count => Mapping.Count();

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;
    #endregion




    public struct ProxyEnumerator : IEnumerator<KeyValuePair<string, object>>, IEnumerator
    {
      private IEnumerator<KeyValuePair<string, FakeRefObject>> Enumerator;

      public bool MoveNext() => Enumerator.MoveNext();
      public KeyValuePair<string, object> Current
        => new KeyValuePair<string, object>(Enumerator.Current.Key, Enumerator.Current.Value.Value);
      object? IEnumerator.Current => Enumerator.Current;
      void IEnumerator.Reset() => Enumerator.Reset();

      public ProxyEnumerator(IEnumerator<KeyValuePair<string, FakeRefObject>> enumerator) => Enumerator = enumerator;
      public void Dispose() { }
    }



    #region IEnumerable
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      => new ProxyEnumerator(Mapping.GetEnumerator());

    IEnumerator IEnumerable.GetEnumerator() => new ProxyEnumerator(Mapping.GetEnumerator());
    #endregion


  }


}

