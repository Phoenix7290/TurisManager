using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurisManager.Migrations
{
    /// <inheritdoc />
    public partial class AddIsConfirmadaToPacoteTuristico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmada",
                table: "Reservas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmada",
                table: "PacotesTuristicos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmada",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "IsConfirmada",
                table: "PacotesTuristicos");
        }
    }
}
