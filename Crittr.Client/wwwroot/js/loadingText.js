export function cycleLoadingText(elId) {
    const el = document.getElementById(elId);
    if (!el) return;

    const messages = [
        "Warming up the heat lamps...",
        "Dusting the terrarium glass...",
        "Finding the lost cricket...",
        "Teaching a gecko to wink...",
        "Snapping turtle selfies...",
        "Untangling a snake knot...",
        "Convincing a chameleon to pick a color...",
        "Loading Crittr… 🦎",
        "Rehydrating moss... ☘️",
        "Patience, like a tortoise.",
        "Installing lizard lounge furniture...",
        "Calming a cranky iguana...",
        "Croaking softly in the moonlight...",
        "Analyzing poop consistency...",
        "Crittr is stretching its scales...",
        "Brewing critter-safe coffee...",
        "Sharpening claws... carefully.",
        "Camouflaging... please wait.",
        "Fetching data from the jungle 🐍",
        "Watching the chicken cross the road...",
        "Spinning the hamster wheel...",
        "Taking 2 birds out with one stone...",
        "Crittr is yawning..."
    ];

    const getRandomMessage = () =>
        messages[Math.floor(Math.random() * messages.length)];

    const updateMessage = () => {
        el.textContent = getRandomMessage();
    };

    updateMessage();
    setInterval(updateMessage, 2500);
}
