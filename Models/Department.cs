using System.ComponentModel.DataAnnotations;

namespace UniversityDbApp.Models;

public class Department
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    [Display(Name = "Department name")]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(20)]
    [Display(Name = "Department code")]
    public string Code { get; set; } = string.Empty;

    [Required, StringLength(150)]
    public string Faculty { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(150)]
    public string? Location { get; set; }

    [StringLength(160)]
    [Display(Name = "Head of department")]
    public string? HeadOfDepartment { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Established date")]
    public DateTime? EstablishedDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
