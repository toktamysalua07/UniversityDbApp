using System.ComponentModel.DataAnnotations;

namespace UniversityDbApp.Models;

public class Teacher
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(180)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(40)]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [Required, StringLength(120)]
    public string Position { get; set; } = string.Empty;

    [StringLength(150)]
    public string? Specialization { get; set; }

    [StringLength(100)]
    [Display(Name = "Academic degree")]
    public string? AcademicDegree { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Hire date")]
    public DateTime HireDate { get; set; } = DateTime.Today;

    [StringLength(80)]
    [Display(Name = "Employment type")]
    public string? EmploymentType { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select a department.")]
    [Display(Name = "Department")]
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";
}
