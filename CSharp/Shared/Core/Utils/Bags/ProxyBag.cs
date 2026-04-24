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

  /// <summary>
  /// It uses Get, Set, Has delegates to map IPropertyBag calls 
  /// Typically to some c# dict
  /// </summary>
  public class ProxyBag : IPropertyBag
  {
    public Action<string, object> Set { get; set; }
    public Func<string, object> Get { get; set; }
    public Func<string, bool> Has { get; set; }
    public Func<ICollection<string>> GetKeys { get; set; }

    public object this[string key]
    {
      get => Get(key);
      set => Set(key, value);
    }

    #region IDictionary<string, object>
    public ICollection<string> Keys => GetKeys();
    public ICollection<object> Values => GetKeys().Select(key => Get(key)).ToArray();
    public bool ContainsKey(string key) => Has(key);
    public void Add(string key, object value) => this[key] = value;
    public bool Remove(string key) => false;
    public bool TryGetValue(string key, out object value)
    {
      if (Has(key))
      {
        value = Get(key); return true;
      }
      value = null; return false;
    }
    #endregion


    #region ICollection<KeyValuePair<string, object>>
    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> kvp)
      => Add(kvp.Key, kvp.Value);
    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> kvp)
      => ContainsKey(kvp.Key);
    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> kvp)
      => false;
    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int index)
      => throw new NotImplementedException("too lazy");

    public void Clear() => throw new NotSupportedException("It's not a real bag");
    public int Count => GetKeys().Count;

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;
    #endregion

    #region IEnumerable
    public IEnumerable<KeyValuePair<string, object>> Enumerate()
    {
      foreach (string key in GetKeys())
      {
        yield return new KeyValuePair<string, object>(key, Get(key));
      }
    }
    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      => Enumerate().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => Enumerate().GetEnumerator();
    #endregion
  }


}

