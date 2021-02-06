using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreAPI_Template_v3_with_auth.Migrations
{
    public partial class OrderCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("42f106fb-a798-4483-80bf-2dc9c09dbde0"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("4ec3db99-2fec-433c-bb7f-aa67fe5b6af7"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("57fb82f6-9bc6-4f6c-b581-e67fd0549ba5"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("62a0d233-610d-45d2-8fa8-daa6bec4dd42"));

            migrationBuilder.InsertData(
                schema: "auth",
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("6200d70b-07c2-4df7-a6b4-77782174bfec"), "user" },
                    { new Guid("3793bccb-ea45-4eb4-876b-c272b9ac4931"), "Manager" },
                    { new Guid("59e892f0-19cc-4ff7-ad63-5d0d0ec574ff"), "Admin" },
                    { new Guid("1eb719bb-fe61-426f-86e0-495d6c349816"), "Developer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("1eb719bb-fe61-426f-86e0-495d6c349816"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3793bccb-ea45-4eb4-876b-c272b9ac4931"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("59e892f0-19cc-4ff7-ad63-5d0d0ec574ff"));

            migrationBuilder.DeleteData(
                schema: "auth",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("6200d70b-07c2-4df7-a6b4-77782174bfec"));

            migrationBuilder.InsertData(
                schema: "auth",
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("62a0d233-610d-45d2-8fa8-daa6bec4dd42"), "user" },
                    { new Guid("42f106fb-a798-4483-80bf-2dc9c09dbde0"), "Manager" },
                    { new Guid("4ec3db99-2fec-433c-bb7f-aa67fe5b6af7"), "Admin" },
                    { new Guid("57fb82f6-9bc6-4f6c-b581-e67fd0549ba5"), "Developer" }
                });
        }
    }
}
