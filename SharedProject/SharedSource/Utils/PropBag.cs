
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
using BaroJunk;

using System.Threading;
using System.Threading.Tasks;
using WatsonWebsocket;
using System.Text;

namespace JSForBarotrauma
{

  //BRUH mb make it implement IDictionary<string, ProxyProp>?
  public class PropBag : ProxyBag
  {
    public class ProxyProp
    {
      public Func<object> Getter { get; }
      public Action<object> Setter { get; }

      public object Value
      {
        get => Getter();
        set => Setter(value);
      }

      public ProxyProp(Func<object> get = null, Action<object> set = null)
      {
        Getter = get ?? new Func<object>(() => null);
        Setter = set ?? new Action<object>((value) => { });
      }
    }

    protected Dictionary<string, ProxyProp> Props { get; set; }

    public PropBag()
    {
      OnGet = (key) =>
      {
        if (!Props.ContainsKey(key)) return null;
        return Props[key].Value;
      };
      OnSet = (key, value) =>
      {
        if (!Props.ContainsKey(key)) return;
        Props[key].Value = value;
      };
    }

    public PropBag(Dictionary<string, ProxyProp> props) : this()
    {
      Props = props;
      Hints = Props.Keys.ToHashSet();
    }
  }



}