using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers;

public class TeacherController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? q, int? departmentId, string? position, string sort = "name", int page = 1, CancellationToken ct = default)
    {
        const int size = 10;
        var query = db.Teachers.AsNoTracking().Include(t => t.Department).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(t => t.FirstName.Contains(q) || t.LastName.Contains(q) || t.Email.Contains(q) || (t.Specialization != null && t.Specialization.Contains(q)));
        if (departmentId.HasValue) query = query.Where(t => t.DepartmentId == departmentId);
        if (!string.IsNullOrWhiteSpace(position)) query = query.Where(t => t.Position == position);
        query = sort == "department" ? query.OrderBy(t => t.Department.Name).ThenBy(t => t.LastName) : sort == "newest" ? query.OrderByDescending(t => t.HireDate) : query.OrderBy(t => t.LastName).ThenBy(t => t.FirstName);
        var total = await query.CountAsync(ct); ViewBag.Page = page; ViewBag.Pages = (int)Math.Ceiling(total / (double)size); ViewBag.Query = q; ViewBag.DepartmentId = departmentId; ViewBag.Position = position; ViewBag.Sort = sort;
        ViewBag.Departments = new SelectList(await db.Departments.AsNoTracking().OrderBy(d => d.Name).ToListAsync(ct), "Id", "Name", departmentId);
        ViewBag.Positions = await db.Teachers.AsNoTracking().Select(t => t.Position).Distinct().OrderBy(x => x).ToListAsync(ct);
        return View(await query.Skip((Math.Max(page, 1) - 1) * size).Take(size).ToListAsync(ct));
    }
    public async Task<IActionResult> Details(int id, CancellationToken ct) => await db.Teachers.AsNoTracking().Include(t => t.Department).FirstOrDefaultAsync(t => t.Id == id, ct) is { } t ? View(t) : NotFound();
    public async Task<IActionResult> Create(CancellationToken ct) { await PopulateDepartments(ct); return View(new Teacher { HireDate = DateTime.Today }); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Teacher teacher, CancellationToken ct)
    {
        teacher.Email = teacher.Email.Trim();
        if (await db.Teachers.AnyAsync(t => t.Email == teacher.Email, ct)) ModelState.AddModelError(nameof(teacher.Email), "This email address is already in use.");
        if (!ModelState.IsValid) { await PopulateDepartments(ct, teacher.DepartmentId); return View(teacher); }
        db.Add(teacher); await db.SaveChangesAsync(ct); TempData["Toast"] = "Teacher profile created."; return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id, CancellationToken ct) { var t = await db.Teachers.FindAsync([id], ct); if (t is null) return NotFound(); await PopulateDepartments(ct, t.DepartmentId); return View(t); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Teacher teacher, CancellationToken ct)
    {
        if (id != teacher.Id) return NotFound();
        teacher.Email = teacher.Email.Trim();
        if (await db.Teachers.AnyAsync(t => t.Id != id && t.Email == teacher.Email, ct)) ModelState.AddModelError(nameof(teacher.Email), "This email address is already in use.");
        if (!ModelState.IsValid) { await PopulateDepartments(ct, teacher.DepartmentId); return View(teacher); }
        db.Update(teacher); try { await db.SaveChangesAsync(ct); TempData["Toast"] = "Teacher profile updated."; return RedirectToAction(nameof(Index)); }
        catch (DbUpdateConcurrencyException) { if (!await db.Teachers.AnyAsync(t => t.Id == id, ct)) return NotFound(); throw; }
    }
    public async Task<IActionResult> Delete(int id, CancellationToken ct) => await db.Teachers.AsNoTracking().Include(t => t.Department).FirstOrDefaultAsync(t => t.Id == id, ct) is { } t ? View(t) : NotFound();
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
    {
        var t = await db.Teachers.FindAsync([id], ct); if (t is null) return NotFound(); db.Remove(t); await db.SaveChangesAsync(ct); TempData["Toast"] = "Teacher removed from the directory."; return RedirectToAction(nameof(Index));
    }
    private async Task PopulateDepartments(CancellationToken ct, int? selected = null) => ViewBag.Departments = new SelectList(await db.Departments.AsNoTracking().OrderBy(d => d.Name).ToListAsync(ct), "Id", "Name", selected);
}
