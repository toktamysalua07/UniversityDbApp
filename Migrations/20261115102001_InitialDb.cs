using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityDbApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adopt existing tables from earlier lab iterations. This lets the migration
            // history catch up without dropping or overwriting the user's data.
            migrationBuilder.Sql("""
                IF OBJECT_ID(N'dbo.Departments', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.Departments (
                        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Departments PRIMARY KEY,
                        Name nvarchar(150) NOT NULL,
                        Code nvarchar(20) NOT NULL,
                        Faculty nvarchar(150) NOT NULL,
                        Description nvarchar(1000) NULL,
                        Location nvarchar(150) NULL,
                        HeadOfDepartment nvarchar(160) NULL,
                        EstablishedDate datetime2 NULL,
                        CreatedAt datetime2 NOT NULL CONSTRAINT DF_Departments_CreatedAt DEFAULT (GETUTCDATE())
                    );
                END;

                IF OBJECT_ID(N'dbo.Teachers', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.Teachers (
                        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Teachers PRIMARY KEY,
                        FirstName nvarchar(80) NOT NULL,
                        LastName nvarchar(80) NOT NULL,
                        Email nvarchar(180) NOT NULL,
                        PhoneNumber nvarchar(40) NULL,
                        Position nvarchar(120) NOT NULL,
                        Specialization nvarchar(150) NULL,
                        AcademicDegree nvarchar(100) NULL,
                        HireDate datetime2 NOT NULL,
                        EmploymentType nvarchar(80) NULL,
                        DepartmentId int NOT NULL
                    );
                END;

                IF COL_LENGTH(N'dbo.Departments', N'Code') IS NOT NULL
                    AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Departments') AND name = N'IX_Departments_Code')
                    CREATE UNIQUE INDEX IX_Departments_Code ON dbo.Departments(Code);

                IF COL_LENGTH(N'dbo.Teachers', N'DepartmentId') IS NOT NULL
                    AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Teachers') AND name = N'IX_Teachers_DepartmentId')
                    CREATE INDEX IX_Teachers_DepartmentId ON dbo.Teachers(DepartmentId);

                IF COL_LENGTH(N'dbo.Teachers', N'Email') IS NOT NULL
                    AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Teachers') AND name = N'IX_Teachers_Email')
                    CREATE UNIQUE INDEX IX_Teachers_Email ON dbo.Teachers(Email);

                IF COL_LENGTH(N'dbo.Teachers', N'DepartmentId') IS NOT NULL
                    AND OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL
                    AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'dbo.Teachers') AND referenced_object_id = OBJECT_ID(N'dbo.Departments'))
                    ALTER TABLE dbo.Teachers WITH CHECK ADD CONSTRAINT FK_Teachers_Departments_DepartmentId FOREIGN KEY (DepartmentId) REFERENCES dbo.Departments(Id) ON DELETE NO ACTION;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally non-destructive: this migration may have adopted tables that
            // existed before EF migration history was introduced.
        }
    }
}
