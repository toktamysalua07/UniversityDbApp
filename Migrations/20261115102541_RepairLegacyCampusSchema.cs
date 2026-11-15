using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityDbApp.Migrations
{
    /// <inheritdoc />
    public partial class RepairLegacyCampusSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Departments', N'EstablishedDate') IS NULL
                    ALTER TABLE dbo.Departments ADD EstablishedDate datetime2 NULL;
                IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Departments', N'HeadOfDepartment') IS NULL
                    ALTER TABLE dbo.Departments ADD HeadOfDepartment nvarchar(160) NULL;
                IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Departments', N'Location') IS NULL
                    ALTER TABLE dbo.Departments ADD Location nvarchar(150) NULL;
                IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Teachers', N'EmploymentType') IS NULL
                    ALTER TABLE dbo.Teachers ADD EmploymentType nvarchar(80) NULL;
                IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Teachers', N'Specialization') IS NULL
                    ALTER TABLE dbo.Teachers ADD Specialization nvarchar(150) NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Teachers', N'Specialization') IS NOT NULL
                    ALTER TABLE dbo.Teachers DROP COLUMN Specialization;
                IF OBJECT_ID(N'dbo.Teachers', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Teachers', N'EmploymentType') IS NOT NULL
                    ALTER TABLE dbo.Teachers DROP COLUMN EmploymentType;
                IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Departments', N'Location') IS NOT NULL
                    ALTER TABLE dbo.Departments DROP COLUMN Location;
                IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Departments', N'HeadOfDepartment') IS NOT NULL
                    ALTER TABLE dbo.Departments DROP COLUMN HeadOfDepartment;
                IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL AND COL_LENGTH(N'dbo.Departments', N'EstablishedDate') IS NOT NULL
                    ALTER TABLE dbo.Departments DROP COLUMN EstablishedDate;
                """);
        }
    }
}
