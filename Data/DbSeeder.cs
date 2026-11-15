using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Models;

namespace UniversityDbApp.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, ILogger logger, CancellationToken cancellationToken = default)
    {
        var db = services.GetRequiredService<AppDbContext>();
        if (!await db.Database.CanConnectAsync(cancellationToken)) return;
        if (await db.Departments.AnyAsync(cancellationToken)) return;

        var departments = new[]
        {
            new Department { Name = "Computer Engineering", Code = "CE", Faculty = "Faculty of Engineering", Location = "North Hall · 2nd floor", HeadOfDepartment = "Aida Nurgaliyeva", EstablishedDate = new DateTime(2008, 9, 1), Description = "Building the next generation of intelligent systems and digital infrastructure." },
            new Department { Name = "Information Systems", Code = "IS", Faculty = "Faculty of Engineering", Location = "Innovation Center · 3rd floor", HeadOfDepartment = "Timur Sadykov", EstablishedDate = new DateTime(2011, 9, 1), Description = "Connecting people, data, and technology through thoughtful systems design." },
            new Department { Name = "Applied Mathematics", Code = "AM", Faculty = "Faculty of Science", Location = "Science Block · 1st floor", HeadOfDepartment = "Madina Omarova", EstablishedDate = new DateTime(2004, 9, 1), Description = "Exploring mathematical ideas that solve real-world challenges." },
            new Department { Name = "Languages & Communication", Code = "LC", Faculty = "Faculty of Humanities", Location = "West Wing · 2nd floor", HeadOfDepartment = "Dana Beketova", EstablishedDate = new DateTime(2002, 9, 1), Description = "Helping voices, cultures, and ideas travel further." },
            new Department { Name = "Natural Sciences", Code = "NS", Faculty = "Faculty of Science", Location = "Science Block · 4th floor", HeadOfDepartment = "Arman Tulegen", EstablishedDate = new DateTime(1998, 9, 1), Description = "Curiosity-led teaching and research across the natural world." },
            new Department { Name = "Social Sciences", Code = "SS", Faculty = "Faculty of Humanities", Location = "West Wing · 4th floor", HeadOfDepartment = "Aruzhan Ilyas", EstablishedDate = new DateTime(2006, 9, 1), Description = "Understanding societies and shaping more inclusive futures." }
        };
        db.Departments.AddRange(departments);
        await db.SaveChangesAsync(cancellationToken);

        var people = new (string First, string Last, string Email, string Position, string Degree, string Specialty, string Type, int Department)[]
        {
            ("Ayan", "Kassymov", "ayan.kassymov@university.edu", "Professor", "PhD", "Artificial intelligence", "Full-time", 0), ("Mira", "Zhaksylyk", "mira.zhaksylyk@university.edu", "Associate Professor", "PhD", "Computer vision", "Full-time", 0), ("Nurlan", "Akhmet", "nurlan.akhmet@university.edu", "Senior Lecturer", "MSc", "Embedded systems", "Full-time", 0), ("Aliya", "Serik", "aliya.serik@university.edu", "Lecturer", "MSc", "Networks", "Part-time", 0),
            ("Dias", "Murat", "dias.murat@university.edu", "Professor", "PhD", "Data science", "Full-time", 1), ("Amina", "Tursun", "amina.tursun@university.edu", "Senior Lecturer", "MSc", "Product design", "Full-time", 1), ("Samat", "Yessen", "samat.yessen@university.edu", "Lecturer", "MSc", "Cybersecurity", "Part-time", 1),
            ("Madina", "Omarova", "madina.omarova@university.edu", "Professor", "PhD", "Statistical modelling", "Full-time", 2), ("Erlan", "Nazar", "erlan.nazar@university.edu", "Associate Professor", "PhD", "Applied analysis", "Full-time", 2), ("Zarina", "Abil", "zarina.abil@university.edu", "Lecturer", "MSc", "Operations research", "Part-time", 2),
            ("Dana", "Beketova", "dana.beketova@university.edu", "Professor", "PhD", "Applied linguistics", "Full-time", 3), ("Ruslan", "Kairat", "ruslan.kairat@university.edu", "Senior Lecturer", "MA", "Translation studies", "Full-time", 3), ("Aruzhan", "Ilyas", "aruzhan.ilyas@university.edu", "Lecturer", "MA", "Academic writing", "Part-time", 3),
            ("Arman", "Tulegen", "arman.tulegen@university.edu", "Professor", "PhD", "Ecology", "Full-time", 4), ("Kamila", "Baur", "kamila.baur@university.edu", "Associate Professor", "PhD", "Biotechnology", "Full-time", 4), ("Maksat", "Ospan", "maksat.ospan@university.edu", "Lecturer", "MSc", "Physics", "Full-time", 4),
            ("Aruzhan", "Ilyas", "a.ilyas@university.edu", "Professor", "PhD", "Public policy", "Full-time", 5), ("Bibigul", "Sapar", "bibigul.sapar@university.edu", "Senior Lecturer", "PhD", "Sociology", "Full-time", 5), ("Adil", "Saken", "adil.saken@university.edu", "Lecturer", "MSc", "Economics", "Part-time", 5)
        };
        var today = DateTime.Today;
        for (var i = 0; i < people.Length; i++)
        {
            var p = people[i];
            db.Teachers.Add(new Teacher { FirstName = p.First, LastName = p.Last, Email = p.Email, Position = p.Position, AcademicDegree = p.Degree, Specialization = p.Specialty, EmploymentType = p.Type, DepartmentId = departments[p.Department].Id, HireDate = today.AddYears(-((i % 14) + 1)), PhoneNumber = $"+7 7{(i % 10)}0 123 45 {i:00}" });
        }
        await db.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Development sample campus data was added.");
    }
}
