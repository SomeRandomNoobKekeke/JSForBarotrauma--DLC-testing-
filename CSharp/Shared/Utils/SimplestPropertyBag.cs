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
  public class SimplestPropertyBag : IPropertyBag
  {
    public Dictionary<string, object> Dict { get; } = new();

    public object this[string key]
    {
      get => Dict[key];
      set => Dict[key] = value;
    }

    #region IDictionary<string, object>
    public ICollection<string> Keys => Dict.Keys;
    public ICollection<object> Values => Dict.Values;
    public bool ContainsKey(string key) => Dict.ContainsKey(key);
    public void Add(string key, object value) => Dict.Add(key, value);
    public bool Remove(string key) => Dict.Remove(key);
    public bool TryGetValue(string key, out object value) => Dict.TryGetValue(key, out value);
    #endregion


    #region ICollection<KeyValuePair<string, object>>
    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> keyValuePair)
      => ((ICollection<KeyValuePair<string, object>>)Dict).Add(keyValuePair);
    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> keyValuePair)
      => ((ICollection<KeyValuePair<string, object>>)Dict).Contains(keyValuePair);
    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> keyValuePair)
      => ((ICollection<KeyValuePair<string, object>>)Dict).Remove(keyValuePair);
    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int index)
      => ((ICollection<KeyValuePair<string, object>>)Dict).CopyTo(array, index);

    public void Clear() => Dict.Clear();
    public int Count => Dict.Count();

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly
      => ((ICollection<KeyValuePair<string, object>>)Dict).IsReadOnly;
    #endregion

    #region IEnumerable
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      => ((IEnumerable<KeyValuePair<string, object>>)Dict).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dict).GetEnumerator();
    #endregion


  }


}

