using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAO.Migrations
{
    /// <inheritdoc />
    public partial class ConvertCorrectOptionsToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedOption",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "CorrectOption",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "SelectedOptions",
                table: "Responses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CorrectOptions",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedOptions",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "CorrectOptions",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "SelectedOption",
                table: "Responses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CorrectOption",
                table: "Questions",
                type: "int",
                nullable: true);
        }
    }
}
