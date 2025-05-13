document.addEventListener("DOMContentLoaded", () => {
    const storeSelect = document.getElementById("store-select");
    const servicesContainer = document.getElementById("services-container");
    const apiBase = "http://localhost:5068/api"; // adjust if needed

    // Populate store dropdown
    stores.forEach(store => {
        const opt = document.createElement("option");
        opt.value = store.id;
        opt.textContent = store.name;
        storeSelect.appendChild(opt);
    });

    // On store change, load services
    storeSelect.addEventListener("change", async () => {
        servicesContainer.innerHTML = "";
        const store = stores.find(s => s.id == storeSelect.value);
        if (!store) return;

        for (const svc of store.services) {
            const div = document.createElement("div");
            div.className = "service-item";

            const label = document.createElement("span");
            label.textContent = svc.name;

            const toggle = document.createElement("input");
            toggle.type = "checkbox";
            toggle.disabled = true;

            // Fetch current status
            try {
                const resp = await fetch(
                    `${apiBase}/servicetypestatus/${svc.id}?host=${store.host}&port=${store.port}`
                );
                const isActive = await resp.json();
                toggle.checked = isActive;
                toggle.disabled = false;
            } catch (e) {
                console.error(e);
            }

            // Toggle change
            toggle.addEventListener("change", async () => {
                toggle.disabled = true;
                try {
                    const url =
                        `${apiBase}/servicetypestatus/${svc.id}?status=${toggle.checked}&host=${store.host}&port=${store.port}`;
                    const res = await fetch(url, { method: "POST" });
                    if (!res.ok) {
                        toggle.checked = !toggle.checked;
                        alert("Failed to update status");
                    }
                } catch (e) {
                    toggle.checked = !toggle.checked;
                    alert("Error updating status");
                } finally {
                    toggle.disabled = false;
                }
            });

            div.appendChild(label);
            div.appendChild(toggle);
            servicesContainer.appendChild(div);
        }
    });
});