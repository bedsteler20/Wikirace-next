/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./src/Pages/**/*.cshtml",
        "./src/Views/**/*.cshtml",
        "./src/Javascript/**/*.ts",
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require("@catppuccin/tailwindcss")({
            // prefix to use, e.g. `text-pink` becomes `text-ctp-pink`.
            // default is `false`, which means no prefix
            // which flavour of colours to use by default, in the `:root`
            defaultFlavour: "mocha",
        }),
    ],
};
