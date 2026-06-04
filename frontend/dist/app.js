"use strict";
const API_BASE = `http://${window.location.hostname}:5053`;
const buildingsList = document.getElementById("buildings-list");
const roomsList = document.getElementById("rooms-list");
const roomsHeading = document.getElementById("rooms-heading");
const roomsPlaceholder = document.getElementById("rooms-placeholder");
async function fetchJson(url) {
    const response = await fetch(url);
    if (!response.ok) {
        throw new Error(`${response.status} ${response.statusText}`);
    }
    return response.json();
}
function formatNumber(value, unit) {
    return value === null ? "-" : `${value.toFixed(1)} ${unit}`;
}
function statusLabel(status) {
    switch (status) {
        case "hot": return "Varmt";
        case "cold": return "Kallt";
        case "normal": return "Normalt";
        case "unknown": return "Ingen data";
        default: return "Okant";
    }
}
function renderRooms(buildingName, rooms) {
    roomsHeading.textContent = `Rum - ${buildingName}`;
    roomsPlaceholder.classList.add("hidden");
    roomsList.classList.remove("hidden");
    roomsList.innerHTML = "";
    for (const room of rooms) {
        const card = document.createElement("li");
        card.className = `room-card status-${room.status}`;
        card.innerHTML = `
      <div class="room-name">${room.name}</div>
      <div class="room-metrics">
        <span>Temperatur: <strong>${formatNumber(room.temperature, "C")}</strong></span>
        <span>El: <strong>${formatNumber(room.electricity, "kWh")}</strong></span>
      </div>
      <span class="status-badge ${room.status}">${statusLabel(room.status)}</span>
    `;
        roomsList.appendChild(card);
    }
}
async function selectBuilding(building, button) {
    document.querySelectorAll(".building-item button").forEach((el) => el.classList.remove("active"));
    button.classList.add("active");
    roomsList.innerHTML = "";
    roomsPlaceholder.textContent = "Hamtar rum...";
    roomsPlaceholder.classList.remove("hidden");
    roomsList.classList.add("hidden");
    try {
        const rooms = await fetchJson(`${API_BASE}/api/buildings/${building.id}/rooms`);
        renderRooms(building.name, rooms);
    }
    catch (err) {
        roomsPlaceholder.textContent = `Kunde inte hamta rum: ${err instanceof Error ? err.message : err}`;
        roomsPlaceholder.classList.remove("hidden");
        roomsList.classList.add("hidden");
    }
}
async function loadBuildings() {
    buildingsList.innerHTML = "<li class='placeholder'>Hamtar byggnader...</li>";
    try {
        const buildings = await fetchJson(`${API_BASE}/api/buildings`);
        buildingsList.innerHTML = "";
        for (const building of buildings) {
            const li = document.createElement("li");
            li.className = "building-item";
            const button = document.createElement("button");
            button.type = "button";
            button.textContent = building.name;
            button.addEventListener("click", () => selectBuilding(building, button));
            li.appendChild(button);
            buildingsList.appendChild(li);
        }
        if (buildings.length > 0) {
            const firstButton = buildingsList.querySelector("button");
            await selectBuilding(buildings[0], firstButton);
        }
    }
    catch (err) {
        buildingsList.innerHTML = `<li class="error">Kunde inte hamta byggnader: ${err instanceof Error ? err.message : err}</li>`;
    }
}
loadBuildings();
