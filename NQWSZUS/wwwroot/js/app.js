document.addEventListener('DOMContentLoaded', () => {
    const selector = document.getElementById('storeSelector');
    const tableWrapper = document.getElementById('tableWrapper');
    const tbody = document.querySelector('#servicesTable tbody');

    // Populate dropdown
    stores.forEach(s => {
        const opt = document.createElement('option');
        opt.value = JSON.stringify({ host: s.host, port: s.port });
        opt.textContent = s.name;
        selector.append(opt);
    });

    selector.addEventListener('change', async () => {
        if (!selector.value) return tableWrapper.classList.add('d-none');

        const { host, port } = JSON.parse(selector.value);
        const services = await (await fetch(`/api/ServiceType?host=${host}&port=${port}`)).json();

        tbody.innerHTML = '';
        tableWrapper.classList.remove('d-none');

        for (const svc of services) {
            const isActive = await (await fetch(`/api/ServiceTypeStatus/${svc.id}?host=${host}&port=${port}`)).json();
            const tr = document.createElement('tr');
            tr.innerHTML = `
        <td>${svc.name}</td>
        <td><span class="badge ${isActive ? 'bg-success' : 'bg-secondary'}">${isActive ? 'Open' : 'Closed'}</span></td>
        <td>
          <div class="form-check form-switch">
            <input class="form-check-input" type="checkbox" ${isActive ? 'checked' : ''}>
          </div>
        </td>
      `;
            tbody.append(tr);

            const toggle = tr.querySelector('.form-check-input');
            toggle.addEventListener('change', async () => {
                const newStatus = toggle.checked;
                const res = await fetch(`/api/ServiceTypeStatus/${svc.id}?status=${newStatus}&host=${host}&port=${port}`, { method: 'POST' });
                if (!res.ok) return Swal.fire('Error', 'Failed to update status.', 'error');

                const badge = tr.querySelector('.badge');
                badge.textContent = newStatus ? 'Open' : 'Closed';
                badge.className = `badge ${newStatus ? 'bg-success' : 'bg-secondary'}`;

                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: `Service "${svc.name}" is now ${newStatus ? 'open' : 'closed'}.`,
                    showConfirmButton: false,
                    timer: 2000
                });
            });
        }
    });
});