using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class ChangePinIntoGameSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Quizzes_Pin",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "QrCode",
                table: "Quizzes");

            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "GameSessions",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 16, 2, 0, 21, 578, DateTimeKind.Utc).AddTicks(3430));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 16, 2, 0, 21, 578, DateTimeKind.Utc).AddTicks(3433));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 16, 2, 0, 21, 578, DateTimeKind.Utc).AddTicks(3434));

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_Pin",
                table: "GameSessions",
                column: "Pin",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GameSessions_Pin",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "GameSessions");

            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "Quizzes",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                table: "Quizzes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 16, 1, 55, 36, 123, DateTimeKind.Utc).AddTicks(5642));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 16, 1, 55, 36, 123, DateTimeKind.Utc).AddTicks(5645));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 16, 1, 55, 36, 123, DateTimeKind.Utc).AddTicks(5647));

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_Pin",
                table: "Quizzes",
                column: "Pin",
                unique: true);
        }
    }
}
