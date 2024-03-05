
function updateInputBoxWithValidationStyles(span: HTMLSpanElement): void {
    if (span.innerText === '') return;
    console.log(span.innerText);
    const forElementId = span.getAttribute('data-valmsg-for');
    if (!forElementId) return;
    const forElement = document.querySelector(`[name="${forElementId}"]`)
    if (!forElement) return;
    forElement.classList.add('border-red', 'border-2');

}

const observer = new MutationObserver((mutationsList) => {
    for (const mutation of mutationsList) {
        if (mutation.type === 'childList') {
            const addedNodes = Array.from(mutation.addedNodes);

            for (const node of addedNodes) {
                if (node instanceof HTMLElement) {
                    if (node.getAttribute('data-valmsg-replace')) {
                        updateInputBoxWithValidationStyles(node);
                    }
                }
            }
        }
    }
});

document.addEventListener('DOMContentLoaded', () => {
    const valMsgReplaces = document.querySelectorAll('[data-valmsg-replace]');
    valMsgReplaces.forEach((valMsgReplace) => {
        updateInputBoxWithValidationStyles(valMsgReplace as HTMLSpanElement);
    });
});

// Start observing the entire document for changes
observer.observe(document, { childList: true, subtree: true });