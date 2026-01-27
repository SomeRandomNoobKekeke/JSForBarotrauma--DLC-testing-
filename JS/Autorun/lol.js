require("./kek.js")

// let harmony = new lib.HarmonyLib.Harmony("gigabruh")

// harmony.Patch(
//   original: typeof (LuaGame).GetMethod("IsCustomCommandPermitted", AccessTools.all),
//   postfix: new HarmonyMethod(typeof (ConsoleInterface).GetMethod("PermitCommands"))
// );


2 +

var connection = JS.StopEvent.connect(new lib.System.Action(() => {
  console.log("js stop")
  Logger.Log("js stop")
}));

