# 0.4.1
- added BindingFlags because lib.HarmonyLib.AccessTools.all was removed by extension filter

# 0.4.0
- Removed extension classes from the lib
They just littering the object inspection and slow down c#->js conversion
If you for some reason need them add them with xHost.type('extension.typename'[,'assemblyname'])
- Added API.Utils.ToCSArray
- Added lil reflection helpers, all objects now have _Get(), _Set() methods which can get private fields, props, methods on an instance like this `item._Get().Field('connections')['signal_in']`

# 0.3.0
- Added changelog :BaroDev:
- Added NetAPI for sending net messages
- Added XMLHookAPI for reacting to xml hooks
- Also moved WebAPI to client only, there's some assembly conflict, still too lazy to investigate