using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;

namespace UniversityDbApp.Controllers;

public class StructureController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await db.Departments.AsNoTracking().Include(d => d.Teachers).OrderBy(d => d.Faculty).ThenBy(d => d.Name).ToListAsync(ct));
}
