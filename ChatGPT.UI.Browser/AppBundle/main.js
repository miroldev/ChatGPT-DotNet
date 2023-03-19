import { dotnet } from './dotnet.js'
import { registerAvaloniaModule } from './avalonia.js';

const is_browser = typeof window != "undefined";
if (!is_browser) throw new Error(`Expected to be running in a browser`);

const dotnetRuntime = await dotnet
    .withDiagnosticTracing(false)
    .withApplicationArgumentsFromQuery()
    .create();

await registerAvaloniaModule(dotnetRuntime);

const config = dotnetRuntime.getConfig();
const exports = await dotnetRuntime.getAssemblyExports(config.mainAssemblyName);
/*
window.onbeforeunload = async function(event) {
    //event.preventDefault();
    console.log("[JS] Saving settings...");
    await exports.Interop.SaveSettings();
    console.log("[JS] Saved settings.");
    event.returnValue = false;
}
*/

window.addEventListener('beforeunload', async (event) => {
    event.preventDefault();
    console.log("[JS] Saving settings...");
    await exports.Interop.SaveSettings();
    console.log("[JS] Saved settings.");
    event.returnValue = false;
});

const terminationEvent = 'onpagehide' in self ? 'pagehide' : 'unload';

window.addEventListener(terminationEvent, async (event) => {
    console.log("[JS] Saving settings...");
    await exports.Interop.SaveSettings();
    console.log("[JS] Saved settings.");
});


await dotnetRuntime.runMainAndExit(config.mainAssemblyName, [window.location.search]);
