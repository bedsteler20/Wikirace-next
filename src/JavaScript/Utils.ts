export function html(strings: TemplateStringsArray, ...values: any[]) {
    let str = "";
    for (let i = 0; i < strings.length; i++) {
        str += strings[i];
        if (i < values.length) {
            str += values[i];
        }
    }
    return str;
}

export function css(strings: TemplateStringsArray, ...values: any[]) {
    let str = "";
    for (let i = 0; i < strings.length; i++) {
        str += strings[i];
        if (i < values.length) {
            str += values[i];
        }
    }
    return str;
}