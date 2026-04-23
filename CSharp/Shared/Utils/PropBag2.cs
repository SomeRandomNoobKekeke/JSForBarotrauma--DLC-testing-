
using System;
using System.Collections;
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
using BaroJunk;

using System.Threading;
using System.Threading.Tasks;
using WatsonWebsocket;
using System.Text;

namespace JSForBarotrauma
{

  public class PropBag2 : IPropertyBag
  {
    public class Prop
    {
      public Func<object> Getter { get; }
      public Action<object> Setter { get; }

      public object Value
      {
        get => Getter();
        set => Setter(value);
      }

      public Prop(Func<object> get = null, Action<object> set = null)
      {
        Getter = get ?? new Func<object>(() => null);
        Setter = set ?? new Action<object>((value) => { });
      }
    }

    protected Dictionary<string, Prop> Props { get; set; }

    public object this[string key]
    {
      get => Props[key].Getter();
      set => Props[key].Setter(value);
    }

    #region IDictionary<string, object>
    public ICollection<string> Keys => Props.Keys;
    public ICollection<object> Values => Props.Values.Select(p => p.Getter()).ToArray();
    public bool ContainsKey(string key) => Props.ContainsKey(key);
    public void Add(string key, object value) => this[key] = value;
    public bool Remove(string key) => Props.Remove(key);
    public bool TryGetValue(string key, out object value)
    {
      if (ContainsKey(key))
      {
        value = this[key]; return true;
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
      => Remove(kvp.Key);
    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int index)
      => throw new NotImplementedException("too lazy");

    public void Clear() => Props.Clear();
    public int Count => Props.Count;

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;
    #endregion

    #region IEnumerable
    public IEnumerable<KeyValuePair<string, object>> Enumerate()
    {
      foreach (string key in Props.Keys)
      {
        yield return new KeyValuePair<string, object>(key, Props[key].Getter());
      }
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      => Enumerate().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => Enumerate().GetEnumerator();
    #endregion
  }



}