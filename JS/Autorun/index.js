JS.Global["runWith"] = function (expression, context) {
  let keys = Object.keys(context);
  let values = Object.values(context);
  const func = new Function(...keys, expression);
  return func(...values);
}

JS.Global.GetMethod = (path, flags = lib.HarmonyLib.AccessTools.all) => {
  if (typeof path != 'string') {
    Console.Warning('path should be string in this format: Barotrauma.GameMain.Update')
    return
  }

  let parts = path.split('.')

  if (parts.length < 2) {
    Console.Warning(`Not enough parts for a method path`)
    return
  }

  let obj = lib[parts[0]]
  for (let i = 1; i < parts.length - 1; i++) {
    obj = obj?.[parts[i]]
  }

  return xHost.typeOf(obj).GetMethod(parts.at(-1), flags)
}