console.log("htmx-redirect.js loaded");

var api;

function hasEventSource(node) {
    return api.getInternalData(node).sseEventSource != null;
}

htmx.defineExtension("redirect", {
    onEvent: function (name, evt) {
        if (name == "htmx:configRequest") {
            if (evt.srcElement.hasAttribute("hx-alert")) {
                let message = evt.srcElement.getAttribute("hx-alert");
                window.alert(message);
            }
            if (evt.srcElement.hasAttribute("hx-redirect")) {
                let path = evt.detail.path;
                evt.detail.path = null;
                window.location.href = path;
            }
        }
    },
});
