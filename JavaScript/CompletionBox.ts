class WikipediaCompletionBox extends HTMLElement {
    private _visible: boolean;
    private _items: CompletionItem[] = [];

    static {
        customElements.define("wikirace-complete", WikipediaCompletionBox);
    }

    get forElement(): HTMLInputElement {
        return document.getElementById(this.getAttribute("for")!) as HTMLInputElement;
    }

    public renderStyles(): string {
        return `
            .root {
                visibility: ${this._visible ? "visible" : "hidden"};
                position: absolute;
                background-color: white;
                width: 500px;
                border: 1px solid black;
                z-index: 100;
            }

            .item {
                display: flex;
                align-items: center;
                padding: 10px;
                border-bottom: 1px solid black;
            }

            .image {
                width: 50px;
                height: 50px;
                margin-right: 10px;
            }

            .title {
                font-weight: bold;
            }

            .description {
                font-size: 0.8em;
            }
        `;
    }


    render() {
        this.shadowRoot!.innerHTML = `
            <style>${this.renderStyles()}</style>
            <div class="root">
                ${this._items.map((item) => `
                    <div class="item">
                        <img class="image" src="${item.imageUrl ?? "/nopage.svg"}" />
                        <div>
                            <div class="title">${item.title}</div>
                            <div class="description">${item.description ?? ""}</div>
                        </div>
                    </div>
                `).join("")}
            </div>
        `;

        this.shadowRoot!.querySelectorAll(".item").forEach((item, index) => {
            item.addEventListener("click", () => {
                this.forElement.value = this._items[index].title;
                this.render();
            });
        });
    }
    constructor() {
        super();
        this._visible = false;
        this.attachShadow({ mode: "open" });
        this.render();
        this.bindToInput(this.forElement);
    }

    bindToInput(inputElement: HTMLInputElement) {
        inputElement.addEventListener("click", () => {
            this._visible = true;
            this.render();
        });

        inputElement.addEventListener("blur", (ev) => {
            this._visible = false;

            setTimeout(() => {
                this.render();
            }, 200);
        });

        inputElement.addEventListener("input", () => {
            this._visible = true;
            this.getResults(inputElement.value).then((results) => {
                this._items = results;
                this.render();
            });
        });
    }

    getResults(query: string): Promise<CompletionItem[]> {
        return fetch(`/session/completions?q=${query}`).then((res) => res.json());
    }
}

interface CompletionItem {
    title: string;
    description: string;
    imageUrl: string;
}
