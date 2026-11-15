using UniversityDbApp.Models;

namespace UniversityDbApp.ViewModels;

public class DashboardViewModel
{
    public int DepartmentCount { get; set; }
    public int TeacherCount { get; set; }
    public int FacultyCount { get; set; }
    public double AverageExperience { get; set; }
    public List<Department> Departments { get; set; } = [];
    public List<Teacher> RecentTeachers { get; set; } = [];
    public List<string> DepartmentLabels { get; set; } = [];
    public List<int> TeacherCounts { get; set; } = [];
    public List<string> DegreeLabels { get; set; } = [];
    public List<int> DegreeCounts { get; set; } = [];
    public List<string> EmploymentLabels { get; set; } = [];
    public List<int> EmploymentCounts { get; set; } = [];
}

public class InsightsViewModel : DashboardViewModel
{
    public List<string> SpecializationLabels { get; set; } = [];
    public List<int> SpecializationCounts { get; set; } = [];
    public List<Department> AllDepartments { get; set; } = [];
}

public class SearchViewModel
{
    public string Query { get; set; } = string.Empty;
    public List<Department> Departments { get; set; } = [];
    public List<Teacher> Teachers { get; set; } = [];
}
