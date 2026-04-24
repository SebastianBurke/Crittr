// Register a direct (non-passive) dragover listener on every drop zone so the
// browser allows drops regardless of how Blazor's event pipeline batches things.
const _dropZoneInited = new WeakSet();
window.initDropZones = function () {
    document.querySelectorAll('[data-drop-zone]').forEach(el => {
        if (_dropZoneInited.has(el)) return;
        _dropZoneInited.add(el);
        el.addEventListener('dragover', e => e.preventDefault(), false);
    });
};

const _tooltipInited = new WeakSet();

window.initCritterTooltips = function (ids) {
    ids.forEach(id => {
        const el = document.getElementById(id);
        if (!el || _tooltipInited.has(el)) return;
        _tooltipInited.add(el);

        const critterId = id.replace('critter-', '');
        const gap = 8;
        const pad = 8;

        el.addEventListener('mouseenter', () => {
            if (window.innerWidth < 640) return; // matches Tailwind sm breakpoint
            // Fresh lookup — Blazor may have re-rendered the tooltip div
            const tooltip = document.getElementById('tooltip-' + critterId);
            if (!tooltip || !tooltip.offsetWidth) return;

            const elRect = el.getBoundingClientRect();
            const tooltipW = tooltip.offsetWidth;
            const tooltipH = tooltip.offsetHeight;

            // Vertical: below if room, else above
            const spaceBelow = window.innerHeight - elRect.bottom;
            const top = spaceBelow >= tooltipH + gap
                ? elRect.bottom + gap
                : elRect.top - tooltipH - gap;

            // Horizontal: center on critter, clamped within viewport
            const centerX = elRect.left + elRect.width / 2;
            const left = Math.max(pad, Math.min(window.innerWidth - tooltipW - pad, centerX - tooltipW / 2));

            tooltip.style.top = top + 'px';
            tooltip.style.left = left + 'px';
            tooltip.style.opacity = '1';
        });

        el.addEventListener('mouseleave', () => {
            const tooltip = document.getElementById('tooltip-' + critterId);
            if (tooltip) tooltip.style.opacity = '0';
        });
    });
};

window.animateAllCritters = function (ids) {
    ids.forEach(id => {
        const el = document.getElementById(id);
        if (!el) return;

        anime({
            targets: el,
            translateX: () => Math.random() * 80 + '%',
            translateY: () => Math.random() * 80 + '%',
            duration: 1000,
            easing: 'easeInOutQuad',
            complete: () => {
                setInterval(() => {
                    anime({
                        targets: el,
                        translateX: () => Math.random() * 80 + '%',
                        translateY: () => Math.random() * 80 + '%',
                        duration: 1000,
                        easing: 'easeInOutQuad'
                    });
                }, 4000);
            }
        });
    });
};
