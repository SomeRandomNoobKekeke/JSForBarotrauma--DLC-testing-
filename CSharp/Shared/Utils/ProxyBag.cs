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
  public class ProxyBag : IPropertyBag
  {
    public Action<string, object> OnSet { get; set; }
    public Func<string, object> OnGet { get; set; }

    public HashSet<string> Hints { get; set; } = [];

    public object this[string key]
    {
      get => OnGet(key);
      set => OnSet(key, value);
    }

    #region IDictionary<string, object>
    public ICollection<string> Keys => Hints;
    public ICollection<object> Values => Hints.Select(key => OnGet(key)).ToArray();
    public bool ContainsKey(string key) => Hints.Contains(key);
    public void Add(string key, object value) => OnSet(key, value);
    public bool Remove(string key) => false;
    public bool TryGetValue(string key, out object value)
    {
      if (ContainsKey(key))
      {
        value = OnGet(key); return true;
      }
      value = null; return false;
    }
    #endregion


    #region ICollection<KeyValuePair<string, object>>
    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> kvp) { }
    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> kvp)
      => ContainsKey(kvp.Key);
    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> kvp)
      => false;
    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int index) { }

    public void Clear() { }
    public int Count => Hints.Count();

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;
    #endregion

    #region IEnumerable
    public IEnumerable<KeyValuePair<string, object>> Enumerate()
    {
      foreach (string key in Hints)
      {
        yield return new KeyValuePair<string, object>(key, OnGet(key));
      }
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      => Enumerate().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => Enumerate().GetEnumerator();
    #endregion


  }


}

