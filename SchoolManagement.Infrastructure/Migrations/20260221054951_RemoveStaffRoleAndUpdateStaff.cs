using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStaffRoleAndUpdateStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_StaffRoles_StaffRoleId",
                table: "Staff");

            migrationBuilder.DropTable(
                name: "StaffRoles");

            migrationBuilder.DropIndex(
                name: "IX_Staff_StaffRoleId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "JoiningDate",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "StaffRoleId",
                table: "Staff");

            migrationBuilder.RenameColumn(
                name: "Class",
                table: "Students",
                newName: "ClassName");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Staff",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Staff",
                newName: "Dob");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Staff",
                newName: "StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_Staff_EmployeeId",
                table: "Staff",
                newName: "IX_Staff_StaffId");

            migrationBuilder.RenameColumn(
                name: "Class",
                table: "Results",
                newName: "ClassName");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Staff",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Staff",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Staff",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Staff",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Staff",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Staff");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Students",
                newName: "Class");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Staff",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "Staff",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "Dob",
                table: "Staff",
                newName: "FirstName");

            migrationBuilder.RenameIndex(
                name: "IX_Staff_StaffId",
                table: "Staff",
                newName: "IX_Staff_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Results",
                newName: "Class");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Staff",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "JoiningDate",
                table: "Staff",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Staff",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "StaffRoleId",
                table: "Staff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StaffRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staff_StaffRoleId",
                table: "Staff",
                column: "StaffRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffRoles_RoleName",
                table: "StaffRoles",
                column: "RoleName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_StaffRoles_StaffRoleId",
                table: "Staff",
                column: "StaffRoleId",
                principalTable: "StaffRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
