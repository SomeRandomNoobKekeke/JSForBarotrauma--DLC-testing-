require("./kek.js")

let reload = () => Host.ReloadJS()

const originalConsoleLog = console.log;
console.log = async function (...args) {
  originalConsoleLog("log")
  BaroConsole.Log(toString(args))
  originalConsoleLog(...args)
};

console.log("bruh")
BaroConsole.Log(123)



