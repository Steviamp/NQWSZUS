document.addEventListener("DOMContentLoaded", () => {
    const storeSelect = document.getElementById("store-select");
    const servicesContainer = document.getElementById("services-container");
    const apiBase = "/api";

    // Populate store dropdown
    stores.forEach(store => {
        const opt = document.createElement("option");
        opt.value = store.id;
        opt.textContent = store.name;
        storeSelect.appendChild(opt);
    });

    // On store change, load service types dynamically
    storeSelect.addEventListener("change", async () => {
        servicesContainer.innerHTML = "";
        const store = stores.find(s => s.id == storeSelect.value);
        if (!store) return;

        let serviceTypes = [];
        try {
            const response = await fetch(
                `${apiBase}/ServiceType?host=${encodeURIComponent(store.host)}&port=${store.port}`
            );
            serviceTypes = await response.json();
        } catch (e) {
            console.error("Error fetching service types:", e);
            servicesContainer.textContent = "Failed to load services.";
            return;
        }

        // Render each service with toggle
        serviceTypes.forEach(svc => {
            const div = document.createElement("div");
            div.className = "service-item";

            const label = document.createElement("span");
            label.textContent = svc.Name || svc.name;

            const toggle = document.createElement("input");
            toggle.type = "checkbox";
            toggle.disabled = true;

            // Fetch current status
            (async () => {
                try {
                    const statusResp = await fetch(
                        `${apiBase}/servicetypestatus/${svc.ID || svc.id}?host=${encodeURIComponent(store.host)}&port=${store.port}`
                    );
                    const isActive = await statusResp.json();
                    toggle.checked = isActive;
                } catch (e) {
                    console.error("Error fetching status for service", svc, e);
                } finally {
                    toggle.disabled = false;
                }
            })();

            // Toggle change
            toggle.addEventListener("change", async () => {
                toggle.disabled = true;
                try {
                    const url =
                        `${apiBase}/servicetypestatus/${svc.ID || svc.id}?status=${toggle.checked}&host=${encodeURIComponent(store.host)}&port=${store.port}`;
                    const res = await fetch(url, { method: "POST" });
                    if (!res.ok) {
                        toggle.checked = !toggle.checked;
                        alert("Failed to update service status.");
                    }
                } catch (e) {
                    toggle.checked = !toggle.checked;
                    alert("Error updating service status.");
                    console.error(e);
                } finally {
                    toggle.disabled = false;
                }
            });

            div.appendChild(label);
            div.appendChild(toggle);
            servicesContainer.appendChild(div);
        });
    });
});