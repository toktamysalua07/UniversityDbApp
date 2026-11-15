(() => {
  const d = window.campusData;
  if (!d || !window.Chart) return;
  Chart.defaults.font.family = "DM Sans, sans-serif";
  Chart.defaults.color = "#81958b";
  const palette = ["#087f6d", "#44ad91", "#88c8a7", "#d4e8a0", "#83b7bd", "#c0ddd2", "#dda67d", "#a99acb"];
  const make = (id, config) => { const node = document.getElementById(id); if (node) new Chart(node, config); };
  make("departmentChart", { type: "bar", data: { labels: d.labels || [], datasets: [{ data: d.counts || [], backgroundColor: palette[0], borderRadius: 5, maxBarThickness: 17 }] }, options: { indexAxis: "y", maintainAspectRatio: false, plugins: { legend: { display: false }, tooltip: { displayColors: false } }, scales: { x: { beginAtZero: true, grid: { color: "#edf2ef" }, ticks: { precision: 0, font: { size: 9 } }, border: { display: false } }, y: { grid: { display: false }, ticks: { font: { size: 9 } }, border: { display: false } } } } });
  make("degreeChart", { type: "doughnut", data: { labels: d.degrees || [], datasets: [{ data: d.degreeCounts || [], backgroundColor: palette, borderWidth: 3, borderColor: getComputedStyle(document.body).getPropertyValue("--surface") || "#fff", hoverOffset: 4 }] }, options: { maintainAspectRatio: false, cutout: "74%", plugins: { legend: { display: false } } } });
  const legend = document.getElementById("degreeLegend");
  if (legend) (d.degrees || []).forEach((name, i) => legend.insertAdjacentHTML("beforeend", `<div class="legend-item"><span class="legend-dot" style="background:${palette[i % palette.length]}"></span>${name}<b>${(d.degreeCounts || [])[i] ?? 0}</b></div>`));
  make("specialtyChart", { type: "bar", data: { labels: d.specialties || [], datasets: [{ data: d.specialtyCounts || [], backgroundColor: palette[1], borderRadius: 5, maxBarThickness: 18 }] }, options: { indexAxis: "y", maintainAspectRatio: false, plugins: { legend: { display: false } }, scales: { x: { beginAtZero: true, grid: { color: "#edf2ef" }, ticks: { precision: 0, font: { size: 9 } }, border: { display: false } }, y: { grid: { display: false }, ticks: { font: { size: 9 } }, border: { display: false } } } } });
  make("employmentChart", { type: "bar", data: { labels: d.employment || [], datasets: [{ data: d.employmentCounts || [], backgroundColor: ["#087f6d", "#76c3a0", "#d4e8a0"], borderRadius: 6, maxBarThickness: 42 }] }, options: { maintainAspectRatio: false, plugins: { legend: { display: false } }, scales: { y: { beginAtZero: true, grid: { color: "#edf2ef" }, ticks: { precision: 0, font: { size: 9 } }, border: { display: false } }, x: { grid: { display: false }, ticks: { font: { size: 9 } }, border: { display: false } } } } });
})();
