using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using UniversityDbApp.Data;

#nullable disable

namespace UniversityDbApp.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("202611150001_InitialDb")]
public partial class InitialDb : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable("Departments", table => new
        {
            Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
            Faculty = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
            Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
            Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
            HeadOfDepartment = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
            EstablishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
        }, constraints: table => table.PrimaryKey("PK_Departments", x => x.Id));

        migrationBuilder.CreateTable("Teachers", table => new
        {
            Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
            FirstName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
            LastName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
            Email = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
            PhoneNumber = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
            Position = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
            Specialization = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
            AcademicDegree = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            EmploymentType = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
            DepartmentId = table.Column<int>(type: "int", nullable: false)
        }, constraints: table =>
        {
            table.PrimaryKey("PK_Teachers", x => x.Id);
            table.ForeignKey("FK_Teachers_Departments_DepartmentId", x => x.DepartmentId, "Departments", "Id", onDelete: ReferentialAction.Restrict);
        });
        migrationBuilder.CreateIndex("IX_Departments_Code", "Departments", "Code", unique: true);
        migrationBuilder.CreateIndex("IX_Teachers_DepartmentId", "Teachers", "DepartmentId");
        migrationBuilder.CreateIndex("IX_Teachers_Email", "Teachers", "Email", unique: true);
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("Teachers");
        migrationBuilder.DropTable("Departments");
    }
}
