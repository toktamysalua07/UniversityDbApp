(() => {
  const stored = localStorage.getItem("campusflow-theme");
  if (stored === "dark") document.body.classList.add("dark-mode");
  const theme = document.getElementById("themeToggle");
  const icon = () => { if (theme) theme.innerHTML = document.body.classList.contains("dark-mode") ? '<i class="bi bi-sun"></i>' : '<i class="bi bi-moon-stars"></i>'; };
  icon();
  theme?.addEventListener("click", () => { document.body.classList.toggle("dark-mode"); localStorage.setItem("campusflow-theme", document.body.classList.contains("dark-mode") ? "dark" : "light"); icon(); });
  document.getElementById("mobileMenu")?.addEventListener("click", () => document.getElementById("sidebar")?.classList.toggle("open"));
  document.addEventListener("click", event => { const side = document.getElementById("sidebar"); if (side?.classList.contains("open") && !side.contains(event.target) && !document.getElementById("mobileMenu")?.contains(event.target)) side.classList.remove("open"); });
})();
