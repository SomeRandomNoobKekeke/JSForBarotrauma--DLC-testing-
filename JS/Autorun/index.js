JS.Global["runWith"] = function (expression, context) {
  let keys = Object.keys(context);
  let values = Object.values(context);
  const func = new Function(...keys, expression);
  return func(...values);
}