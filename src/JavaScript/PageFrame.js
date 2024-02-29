var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var Wikirace;
(function (Wikirace) {
    var JavaScript;
    (function (JavaScript) {
        class PageFrame extends HTMLElement {
            get CurrentPage() {
                return this.getAttribute('current-page');
            }
            set CurrentPage(value) {
                if (value) {
                    this.setAttribute('current-page', value);
                }
                else {
                    this.removeAttribute('current-page');
                }
            }
            get UpdatePageEndpoint() {
                var _a;
                return (_a = this.getAttribute('update-page-endpoint')) === null || _a === void 0 ? void 0 : _a.replace("%7B", "{").replace("%7D", "}");
            }
            get ProgressBar() {
                return this.shadowRoot.getElementById('progress-bar');
            }
            get PageFrame() {
                return this.shadowRoot.getElementById('page-frame');
            }
            get IsLoading() {
                return this.ProgressBar.classList.contains('visible');
            }
            set IsLoading(value) {
                if (value) {
                    this.ProgressBar.classList.remove('hidden');
                    this.ProgressBar.classList.add('visible');
                }
                else {
                    this.ProgressBar.classList.remove('visible');
                    this.ProgressBar.classList.add('hidden');
                }
            }
            OnClick(event) {
                return __awaiter(this, void 0, void 0, function* () {
                    console.log("Click");
                    event.preventDefault();
                    const target = event.target;
                    if (target && target.tagName === 'A' && target.hasAttribute("href")) {
                        const href = target.getAttribute("href");
                        if (href && href.startsWith("./")) {
                            const page = href.substring(2);
                            console.log("Clicked Page:", page);
                            if (href.startsWith("./File:")) {
                                return;
                            }
                            else if (href.startsWith("./Category:")) {
                                return;
                            }
                            else if (href.startsWith("./Template:")) {
                                return;
                            }
                            else if (href.startsWith("./Wikipedia:")) {
                                return;
                            }
                            else if (href.startsWith("./Help:")) {
                                return;
                            }
                            yield this.UpdatePage(page);
                        }
                    }
                });
            }
            UpdatePage(page) {
                return __awaiter(this, void 0, void 0, function* () {
                    this.IsLoading = true;
                    const html = yield this.DownloadHtml(page);
                    this.PageFrame.innerHTML = html.querySelector("body").innerHTML;
                    this.CurrentPage = page;
                    this.IsLoading = false;
                    yield fetch(this.UpdatePageEndpoint.replace("{}", page), {
                        method: 'POST'
                    });
                });
            }
            DownloadHtml(page) {
                var _a, _b, _c, _d, _e, _f, _g, _h, _j, _k;
                return __awaiter(this, void 0, void 0, function* () {
                    const response = yield fetch("https://en.wikipedia.org/api/rest_v1/page/html/" + page);
                    const html = yield response.text();
                    const shadow = document.createElement("html");
                    shadow.innerHTML = html;
                    (_b = (_a = shadow.querySelector("#References")) === null || _a === void 0 ? void 0 : _a.parentElement) === null || _b === void 0 ? void 0 : _b.remove();
                    (_d = (_c = shadow.querySelector("#See_also")) === null || _c === void 0 ? void 0 : _c.parentElement) === null || _d === void 0 ? void 0 : _d.remove();
                    (_f = (_e = shadow.querySelector("#External_links")) === null || _e === void 0 ? void 0 : _e.parentElement) === null || _f === void 0 ? void 0 : _f.remove();
                    (_h = (_g = shadow.querySelector("#Further_reading")) === null || _g === void 0 ? void 0 : _g.parentElement) === null || _h === void 0 ? void 0 : _h.remove();
                    (_k = (_j = shadow.querySelector("#Notes")) === null || _j === void 0 ? void 0 : _j.parentElement) === null || _k === void 0 ? void 0 : _k.remove();
                    shadow.querySelectorAll(".reference").forEach(e => e.remove());
                    shadow.querySelectorAll(".ext-phonos").forEach(e => e.remove());
                    shadow.querySelectorAll(".mw-editsection").forEach(e => e.remove());
                    shadow.querySelectorAll(".external").forEach(e => e.replaceWith(document.createTextNode(e.textContent)));
                    shadow.querySelectorAll(".noprint").forEach(e => e.remove());
                    shadow.querySelectorAll("audio").forEach(e => e.remove());
                    return shadow;
                });
            }
            constructor() {
                super();
                this.attachShadow({ mode: 'open' });
                this.shadowRoot.innerHTML = PageFrame.Template;
                this.shadowRoot.addEventListener('click', this.OnClick.bind(this));
                this.UpdatePage(this.CurrentPage);
            }
        }
        PageFrame.Styles = `
            #progress-bar {
                height: 10px;
            }

            #progress-bar {
                width: 100%;
                background-color: rgb(5, 114, 206);
                animation: indeterminateAnimation 2s infinite linear;
                transform-origin: 0% 50%;
            }

            .hidden {
                visibility: hidden;
            }

            .visible {
                visibility: visible;
            }

            @keyframes indeterminateAnimation {
                0% {
                    transform: translateX(0) scaleX(0);
                }
                40% {
                    transform: translateX(0) scaleX(0.4);
                }
                100% {
                    transform: translateX(100%) scaleX(0.5);
                }
            }

            #page-frame {
                overflow-x: hidden;
                
                padding: 20px;
            }
        `;
        PageFrame.Template = `
            <link rel="stylesheet" href="/css/wikipedia.css">
            <style>
                ${PageFrame.Styles}
            </style>
            <div id="progress-bar" styles="visible"></div>
            <div id="page-frame"></div>
        `;
        (() => {
            customElements.define('wikirace-page-frame', PageFrame);
        })();
        JavaScript.PageFrame = PageFrame;
    })(JavaScript = Wikirace.JavaScript || (Wikirace.JavaScript = {}));
})(Wikirace || (Wikirace = {}));
