using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers;

public class DepartmentController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? q, string? faculty, string sort = "name", int page = 1, CancellationToken cancellationToken = default)
    {
        const int pageSize = 8;
        var query = db.Departments.AsNoTracking().Include(d => d.Teachers).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(d => d.Name.Contains(q) || d.Code.Contains(q) || d.Faculty.Contains(q));
        if (!string.IsNullOrWhiteSpace(faculty)) query = query.Where(d => d.Faculty == faculty);
        query = sort == "faculty" ? query.OrderBy(d => d.Faculty).ThenBy(d => d.Name) : query.OrderBy(d => d.Name);
        var total = await query.CountAsync(cancellationToken);
        ViewBag.Page = page; ViewBag.Pages = (int)Math.Ceiling(total / (double)pageSize); ViewBag.Query = q; ViewBag.Faculty = faculty; ViewBag.Sort = sort;
        ViewBag.Faculties = await db.Departments.AsNoTracking().Select(d => d.Faculty).Distinct().OrderBy(x => x).ToListAsync(cancellationToken);
        return View(await query.Skip((Math.Max(page, 1) - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken));
    }

    public async Task<IActionResult> Details(int id, CancellationToken ct) => await db.Departments.AsNoTracking().Include(d => d.Teachers).FirstOrDefaultAsync(d => d.Id == id, ct) is { } d ? View(d) : NotFound();
    public IActionResult Create() => View(new Department { EstablishedDate = DateTime.Today });

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department, CancellationToken ct)
    {
        department.Code = department.Code.Trim().ToUpperInvariant();
        if (await db.Departments.AnyAsync(d => d.Code == department.Code, ct)) ModelState.AddModelError(nameof(department.Code), "This department code is already in use.");
        if (!ModelState.IsValid) return View(department);
        department.CreatedAt = DateTime.UtcNow;
        db.Add(department); await db.SaveChangesAsync(ct); TempData["Toast"] = "Department created successfully."; return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken ct) => await db.Departments.FindAsync([id], ct) is { } d ? View(d) : NotFound();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Department department, CancellationToken ct)
    {
        if (id != department.Id) return NotFound();
        department.Code = department.Code.Trim().ToUpperInvariant();
        if (await db.Departments.AnyAsync(d => d.Id != id && d.Code == department.Code, ct)) ModelState.AddModelError(nameof(department.Code), "This department code is already in use.");
        if (!ModelState.IsValid) return View(department);
        db.Update(department); db.Entry(department).Property(x => x.CreatedAt).IsModified = false;
        try { await db.SaveChangesAsync(ct); TempData["Toast"] = "Department changes saved."; return RedirectToAction(nameof(Index)); }
        catch (DbUpdateConcurrencyException) { if (!await db.Departments.AnyAsync(d => d.Id == id, ct)) return NotFound(); throw; }
    }

    public async Task<IActionResult> Delete(int id, CancellationToken ct) => await db.Departments.AsNoTracking().Include(d => d.Teachers).FirstOrDefaultAsync(d => d.Id == id, ct) is { } d ? View(d) : NotFound();

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
    {
        var department = await db.Departments.Include(d => d.Teachers).FirstOrDefaultAsync(d => d.Id == id, ct);
        if (department is null) return NotFound();
        if (department.Teachers.Count > 0) { TempData["ToastError"] = "Move or remove this department’s teachers before deleting it."; return RedirectToAction(nameof(Index)); }
        db.Remove(department); await db.SaveChangesAsync(ct); TempData["Toast"] = "Department deleted."; return RedirectToAction(nameof(Index));
    }
}
