import esbuild from "esbuild";
import { minifyHTMLLiteralsPlugin } from "esbuild-plugin-minify-html-literals";

const isDev = process.env.ASPNETCORE_ENVIRONMENT === "Development";
const shouldWatch = process.argv.includes("--watch");

console.log("Building for", isDev ? "development" : "production");
const ctx = await esbuild.context({
    outdir: "src/wwwroot/lib/wikirace/",
    entryPoints: ["src/JavaScript/index.ts"],
    bundle: true,
    minify: isDev ? false : true,
    sourcemap: isDev ? "inline" : false,
    plugins: [
        // minifyHTMLLiteralsPlugin({
        //     shouldMinify: isDev ? false : true,
        // }),
    ],
});

if (shouldWatch) {
    console.log("Watching for changes...");
    await ctx.watch();
} else {
    await ctx.rebuild();
    console.log("Build complete");
    esbuild.stop();
}
