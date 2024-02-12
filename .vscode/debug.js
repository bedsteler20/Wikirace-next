const fs = require("fs");
const child_process = require("child_process");

// Write PID file
const pidFilePath = "/home/bedsteler20/Projects/Wikirace/.vscode/debug.pid";
fs.writeFileSync(pidFilePath, process.pid.toString());

const startedText = "Application started. Press Ctrl+C to shut down.";

const command = "dotnet";
const args = ["watch"];

const child = child_process.spawn(command, args, {
    stdio: ["inherit", "pipe", "inherit"],
});

child.stdout.on("data", (data) => {
    process.stdout.write(data);
    if (data.includes(startedText)) {
        startDebugger({
            name: ".NET Core Attach",
            type: "coreclr",
            request: "attach",
            processName: "Wikirace",
        });
    }
});

process.on("SIGINT", () => {
    child.kill("SIGINT");
    fs.rm(pidFilePath);
});

function startDebugger(config) {
    const url =
        "vscode://fabiospampinato.vscode-debug-launcher/launch?args=" +
        encodeURIComponent(JSON.stringify(config));
    if (process.platform === "linux") {
        child_process.exec(`xdg-open "${url}"`);
    } else if (process.platform === "darwin") {
        child_process.exec(`open "${url}"`);
    }
}
