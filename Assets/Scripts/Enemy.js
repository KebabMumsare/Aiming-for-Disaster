import * as Currency from './Currency.js';

// Example: award 10 currency on death
const REWARD = 10;

export function onEnemyDeath(enemy) {
    // enemy argument depends on your engine; just call this when the enemy dies
    Currency.add(REWARD);

    // optional: spawn UI feedback
    if (typeof document !== "undefined") {
        const el = document.createElement('div');
        el.textContent = `+${REWARD}`;
        el.className = 'float-text reward';
        document.body.appendChild(el);
        setTimeout(() => el.remove(), 900); 
    }
}