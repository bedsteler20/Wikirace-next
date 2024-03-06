import { css, html } from "./Utils";

export class PageFrame extends HTMLElement {

    public static Styles = css`
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

    public static Template = html`
            <link rel="stylesheet" href="/lib/wikirace/wikipedia.css">
            <style>
                ${PageFrame.Styles}
            </style>
            <div id="progress-bar" styles="visible"></div>
            <div id="page-frame"></div>
        `;

    public get CurrentPage(): string | null {
        return this.getAttribute('current-page');
    }

    public set CurrentPage(value: string | null) {
        if (value) {
            this.setAttribute('current-page', value);
        } else {
            this.removeAttribute('current-page');
        }
    }

    public get UpdatePageEndpoint(): string | null | undefined {
        return this.getAttribute('update-page-endpoint')?.replace("%7B", "{").replace("%7D", "}");
    }

    public get ProgressBar(): HTMLElement {
        return this.shadowRoot!.getElementById('progress-bar')!;
    }

    public get PageFrame(): HTMLElement {
        return this.shadowRoot!.getElementById('page-frame')!;
    }

    public get IsLoading(): boolean {
        return this.ProgressBar.classList.contains('visible');
    }

    public set IsLoading(value: boolean) {
        if (value) {
            this.ProgressBar.classList.remove('hidden');
            this.ProgressBar.classList.add('visible');
        } else {
            this.ProgressBar.classList.remove('visible');
            this.ProgressBar.classList.add('hidden');
        }
    }

    public async OnClick(event: Event): Promise<void> {
        console.log("Click")
        event.preventDefault();
        const target = event.target as HTMLAnchorElement;
        if (target && target.tagName === 'A' && target.hasAttribute("href")) {
            const href = target.getAttribute("href");
            if (href && href.startsWith("./")) {
                const page = href.substring(2);
                console.log("Clicked Page:", page)
                if (href.startsWith("./File:")) {
                    return;
                } else if (href.startsWith("./Category:")) {
                    return;
                } else if (href.startsWith("./Template:")) {
                    return;
                } else if (href.startsWith("./Wikipedia:")) {
                    return;
                } else if (href.startsWith("./Help:")) {
                    return;
                }
                await this.UpdatePage(page);
            }
        }

    }

    public async UpdatePage(page: string): Promise<void> {
        this.IsLoading = true;
        const html = await this.DownloadHtml(page);
        this.PageFrame.innerHTML = html.querySelector("body")!.innerHTML;
        this.CurrentPage = page;
        this.IsLoading = false;
        await fetch(this.UpdatePageEndpoint!.replace("{}", page), {
            method: 'POST'
        });
    }


    private async DownloadHtml(page: string): Promise<HTMLElement> {
        const response = await fetch(
            "https://en.wikipedia.org/api/rest_v1/page/html/" + page
        );
        const html = await response.text();
        const shadow = document.createElement("html");
        shadow.innerHTML = html;
        shadow.querySelector("#References")?.parentElement?.remove();
        shadow.querySelector("#See_also")?.parentElement?.remove();
        shadow.querySelector("#External_links")?.parentElement?.remove();
        shadow.querySelector("#Further_reading")?.parentElement?.remove();
        shadow.querySelector("#Notes")?.parentElement?.remove();
        shadow.querySelectorAll(".reference").forEach(e => e.remove());
        shadow.querySelectorAll(".ext-phonos").forEach(e => e.remove());
        shadow.querySelectorAll(".mw-editsection").forEach(e => e.remove());
        shadow.querySelectorAll(".external").forEach(e => e.replaceWith(
            document.createTextNode(e.textContent!)
        ));
        shadow.querySelectorAll(".noprint").forEach(e => e.remove());
        shadow.querySelectorAll("audio").forEach(e => e.remove());

        return shadow;
    }

    static {
        customElements.define('wikirace-page-frame', PageFrame);
    }

    constructor() {
        super();
        this.attachShadow({ mode: 'open' });
        this.shadowRoot!.innerHTML = PageFrame.Template;
        this.shadowRoot!.addEventListener('click', this.OnClick.bind(this));
        this.UpdatePage(this.CurrentPage!);
    }
}

