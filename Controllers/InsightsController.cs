using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.ViewModels;

namespace UniversityDbApp.Controllers;

public class InsightsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var teachers = db.Teachers.AsNoTracking();
        var vm = new InsightsViewModel { DepartmentCount = await db.Departments.CountAsync(ct), TeacherCount = await teachers.CountAsync(ct), FacultyCount = await db.Departments.Select(d => d.Faculty).Distinct().CountAsync(ct), AverageExperience = await teachers.AnyAsync(ct) ? await teachers.AverageAsync(t => (double)(DateTime.Today.Year - t.HireDate.Year), ct) : 0, AllDepartments = await db.Departments.AsNoTracking().Include(d => d.Teachers).OrderBy(d => d.Name).ToListAsync(ct) };
        var degrees = await teachers.GroupBy(t => t.AcademicDegree ?? "Not specified").Select(g => new { Name = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).ToListAsync(ct); vm.DegreeLabels = degrees.Select(x => x.Name).ToList(); vm.DegreeCounts = degrees.Select(x => x.Count).ToList();
        var types = await teachers.GroupBy(t => t.EmploymentType ?? "Not specified").Select(g => new { Name = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).ToListAsync(ct); vm.EmploymentLabels = types.Select(x => x.Name).ToList(); vm.EmploymentCounts = types.Select(x => x.Count).ToList();
        var departments = vm.AllDepartments; vm.DepartmentLabels = departments.Select(d => d.Name).ToList(); vm.TeacherCounts = departments.Select(d => d.Teachers.Count).ToList();
        var specialties = await teachers.Where(t => t.Specialization != null && t.Specialization != "").GroupBy(t => t.Specialization!).Select(g => new { Name = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).Take(8).ToListAsync(ct); vm.SpecializationLabels = specialties.Select(x => x.Name).ToList(); vm.SpecializationCounts = specialties.Select(x => x.Count).ToList();
        return View(vm);
    }
}
