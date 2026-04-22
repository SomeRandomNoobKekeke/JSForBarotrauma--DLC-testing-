using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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
  public class LilParamTable : IPropertyBag
  {
    public object[] Args { get; set; }

    public Dictionary<string, int> Mapping { get; } = new();

    public object this[string key]
    {
      get => Args[Mapping[key]];
      set => Args[Mapping[key]] = value;
    }

    public LilParamTable(ParameterInfo[] parameters)
    {
      for (int i = 0; i < parameters.Length; i++)
      {
        Mapping[parameters[i].Name] = i;
        Mapping[i.ToString()] = i;
      }
    }


    #region IDictionary<string, object>
    public ICollection<string> Keys => Mapping.Keys;
    public ICollection<object> Values => Args;
    public bool ContainsKey(string key) => Mapping.ContainsKey(key);
    public void Add(string key, object value)
    {
      if (Mapping.ContainsKey(key)) Args[Mapping[key]] = value;
    }
    public bool Remove(string key) => throw new NotImplementedException();
    public bool TryGetValue(string key, out object value)
    {
      if (Mapping.ContainsKey(key))
      {
        value = Args[Mapping[key]];
      }
      else
      {
        value = null;
      }

      return Mapping.ContainsKey(key);
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

    public void Clear() => throw new NotImplementedException();
    public int Count => Args.Count();

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false; // eh
    #endregion




    public struct ProxyEnumerator : IEnumerator<KeyValuePair<string, object>>, IEnumerator
    {
      private LilParamTable Table;
      private IEnumerator<KeyValuePair<string, int>> Enumerator;

      public bool MoveNext() => Enumerator.MoveNext();
      public KeyValuePair<string, object> Current
        => new KeyValuePair<string, object>(Enumerator.Current.Key, Table.Args[Enumerator.Current.Value]);
      object? IEnumerator.Current => Current;
      void IEnumerator.Reset() => Enumerator.Reset();

      public ProxyEnumerator(LilParamTable table)
      {
        Table = table;
        Enumerator = table.Mapping.GetEnumerator();
      }

      public void Dispose()
      {
        Table = null;
        Enumerator = null;
      }
    }



    #region IEnumerable
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      => new ProxyEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => new ProxyEnumerator(this);
    #endregion


  }


}

