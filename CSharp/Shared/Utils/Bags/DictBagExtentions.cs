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


  public static class DictBagExtentions
  {
    public static ProxyBag ToBag<TValue>(this Dictionary<string, TValue> dict) where TValue : class
    {
      return new ProxyBag()
      {
        Get = (key) => dict[key],
        Set = (key, value) =>
        {
          try
          {
            dict[key] = (TValue)value;
          }
          catch (InvalidCastException e)
          {
            throw new InvalidCastException($"Error in ProxyBag, Can't cast {value.GetType().Name} to {typeof(TValue)}");
          }
        },
        Has = (key) => dict.ContainsKey(key),
        GetKeys = () => dict.Keys,
      };
    }
  }


}

