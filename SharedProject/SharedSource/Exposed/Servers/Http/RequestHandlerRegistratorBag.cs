
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
using Qoollo.Net.Http;
using System.Net;
namespace JSForBarotrauma
{
  public class RequestHandlerRegistratorBag : IPropertyBag
  {
    public RequestHandlerRegistrator Registrator { get; }

    public RequestHandlerRegistratorBag(RequestHandlerRegistrator registrator)
    {
      this.Registrator = registrator;
    }

    public object this[string key]
    {
      get => Registrator.Handlers[key];
      set => Add(key, value);
    }

    #region IDictionary<string, object>
    public ICollection<string> Keys => Registrator.Handlers.Keys;
    public ICollection<object> Values => Registrator.Handlers.Values.ToArray();
    public bool ContainsKey(string key) => Registrator.Handlers.ContainsKey(key);
    public void Add(string key, object value)
    {
      Func<HttpListenerRequest, string> func
        = Mod.Engine.HostFunctions.del<Func<HttpListenerRequest, string>>(value);

      Registrator[key] = func;
    }
    public bool Remove(string key) => Registrator.Handlers.Remove(key);
    public bool TryGetValue(string key, out object value)
    {
      bool result = Registrator.Handlers.TryGetValue(key, out Func<HttpListenerRequest, string> bruh);
      value = bruh;
      return result;
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
      => throw new Exception("dont");

    public void Clear() => Registrator.Handlers.Clear();
    public int Count => Registrator.Handlers.Count();

    bool ICollection<KeyValuePair<string, object>>.IsReadOnly => false;
    #endregion

    #region IEnumerable

    public IEnumerable<KeyValuePair<string, object>> Enumerate()
    {
      foreach (var (key, value) in Registrator.Handlers)
      {
        yield return new KeyValuePair<string, object>(key, value);
      }
    }

    IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
      => Enumerate().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => Enumerate().GetEnumerator();
    #endregion
  }
}