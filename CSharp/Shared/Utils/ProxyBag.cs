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


  public class ProxyBag2 : IPropertyBag
  {
    public ProxyBag2(IDictionary<string, object> dict)
    {
      Dict = dict;
    }

    public IDictionary<string, object> Dict { get; set; }

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
    public bool TryGetValue(string key, out object value)
      => Dict.TryGetValue(key, out value);
    #endregion


    #region ICollection<KeyValuePair<string, object>>
    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> kvp)
      => Dict.Add(kvp);
    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> kvp)
      => Dict.Contains(kvp);
    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> kvp)
      => Dict.Remove(kvp);
    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int index)
      => Dict.CopyTo(array, index);

    public void Clear() => Dict.Clear();
    public int Count => Dict.Count;

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => Dict.IsReadOnly;
    #endregion

    #region IEnumerable
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      => Dict.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => Dict.GetEnumerator();
    #endregion


  }


}

