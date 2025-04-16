using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToUserWithDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "Password", "Username" },
                values: new object[] { 1, new DateTime(2025, 4, 16, 1, 6, 25, 719, DateTimeKind.Utc).AddTicks(440), "admin@example.com", "hashedpassword", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "Password", "Role", "Username" },
                values: new object[,]
                {
                    { 2, new DateTime(2025, 4, 16, 1, 6, 25, 719, DateTimeKind.Utc).AddTicks(443), "teacher1@example.com", "hashedpassword", 1, "teacher1" },
                    { 3, new DateTime(2025, 4, 16, 1, 6, 25, 719, DateTimeKind.Utc).AddTicks(444), "student1@example.com", "hashedpassword", 2, "student1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");
        }
    }
}
