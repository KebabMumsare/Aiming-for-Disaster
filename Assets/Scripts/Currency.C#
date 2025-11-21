// Simple currency manager (ES module). Use Currency.add(amt) when an enemy dies.

let _balance = 0;
const _listeners = new Set();

function _notify() {
    _listeners.forEach(fn => {
        try { fn(_balance); } catch (e) { console.error(e); }
    });
    // auto-update a DOM element with id "currency" if present (optional)
    if (typeof document !== "undefined") {
        const el = document.getElementById("currency");
        if (el) el.textContent = String(_balance);
    }
}

export function getBalance() {
    return _balance;
}

export function add(amount = 0) {
    amount = Math.floor(Number(amount) || 0);
    if (amount <= 0) return;
    _balance += amount;
    _notify();
}

export function spend(amount = 0) {
    amount = Math.floor(Number(amount) || 0);
    if (amount <= 0) return false;
    if (amount > _balance) return false;
    _balance -= amount;
    _notify();
    return true;
}

export function setBalance(amount = 0) {
    _balance = Math.max(0, Math.floor(Number(amount) || 0));
    _notify();
}

export function onChange(fn) {
    if (typeof fn === "function") {
        _listeners.add(fn);
        // call immediately with current value
        fn(_balance);
        return () => _listeners.delete(fn);
    }
    return () => {};
}