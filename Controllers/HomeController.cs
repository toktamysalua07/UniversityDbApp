using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UniversityDbApp.Data;
using UniversityDbApp.Models;
using UniversityDbApp.ViewModels;

namespace UniversityDbApp.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var teachers = db.Teachers.AsNoTracking();
        var vm = new DashboardViewModel
        {
            DepartmentCount = await db.Departments.CountAsync(cancellationToken),
            TeacherCount = await teachers.CountAsync(cancellationToken),
            FacultyCount = await db.Departments.Select(d => d.Faculty).Distinct().CountAsync(cancellationToken),
            AverageExperience = await teachers.AnyAsync(cancellationToken) ? await teachers.AverageAsync(t => (double)(DateTime.Today.Year - t.HireDate.Year), cancellationToken) : 0,
            Departments = await db.Departments.AsNoTracking().Include(d => d.Teachers).OrderByDescending(d => d.Teachers.Count).Take(4).ToListAsync(cancellationToken),
            RecentTeachers = await teachers.Include(t => t.Department).OrderByDescending(t => t.HireDate).Take(5).ToListAsync(cancellationToken)
        };
        var byDepartment = await db.Departments.AsNoTracking().Select(d => new { d.Name, Count = d.Teachers.Count }).OrderByDescending(x => x.Count).ToListAsync(cancellationToken);
        vm.DepartmentLabels = byDepartment.Select(x => x.Name).ToList(); vm.TeacherCounts = byDepartment.Select(x => x.Count).ToList();
        var degrees = await teachers.GroupBy(t => t.AcademicDegree ?? "Not specified").Select(g => new { Name = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).ToListAsync(cancellationToken);
        vm.DegreeLabels = degrees.Select(x => x.Name).ToList(); vm.DegreeCounts = degrees.Select(x => x.Count).ToList();
        var types = await teachers.GroupBy(t => t.EmploymentType ?? "Not specified").Select(g => new { Name = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).ToListAsync(cancellationToken);
        vm.EmploymentLabels = types.Select(x => x.Name).ToList(); vm.EmploymentCounts = types.Select(x => x.Count).ToList();
        return View(vm);
    }

    public async Task<IActionResult> Search(string? q, CancellationToken cancellationToken)
    {
        var vm = new SearchViewModel { Query = q?.Trim() ?? "" };
        if (vm.Query.Length > 0)
        {
            var term = vm.Query;
            vm.Departments = await db.Departments.AsNoTracking().Where(d => d.Name.Contains(term) || d.Faculty.Contains(term) || d.Code.Contains(term)).Take(20).ToListAsync(cancellationToken);
            vm.Teachers = await db.Teachers.AsNoTracking().Include(t => t.Department).Where(t => t.FirstName.Contains(term) || t.LastName.Contains(term) || (t.Specialization != null && t.Specialization.Contains(term))).Take(30).ToListAsync(cancellationToken);
        }
        return View(vm);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
