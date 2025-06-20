using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurisManager.Migrations
{
    /// <inheritdoc />
    public partial class AddPaisDestinoIdToCidadeDestino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CidadesDestinos_PaisesDestinos_PaisDestinoId",
                table: "CidadesDestinos");

            migrationBuilder.AlterColumn<int>(
                name: "PaisDestinoId",
                table: "CidadesDestinos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CidadesDestinos_PaisesDestinos_PaisDestinoId",
                table: "CidadesDestinos",
                column: "PaisDestinoId",
                principalTable: "PaisesDestinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CidadesDestinos_PaisesDestinos_PaisDestinoId",
                table: "CidadesDestinos");

            migrationBuilder.AlterColumn<int>(
                name: "PaisDestinoId",
                table: "CidadesDestinos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CidadesDestinos_PaisesDestinos_PaisDestinoId",
                table: "CidadesDestinos",
                column: "PaisDestinoId",
                principalTable: "PaisesDestinos",
                principalColumn: "Id");
        }
    }
}
