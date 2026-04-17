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
