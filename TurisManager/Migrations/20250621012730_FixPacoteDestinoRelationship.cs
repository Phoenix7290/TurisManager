using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TurisManager.Migrations
{
    /// <inheritdoc />
    public partial class FixPacoteDestinoRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CidadesDestinos_PacotesTuristicos_PacoteTuristicoId",
                table: "CidadesDestinos");

            migrationBuilder.DropForeignKey(
                name: "FK_CidadesDestinos_PaisesDestinos_PaisDestinoId",
                table: "CidadesDestinos");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Clientes_ClienteId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_PacotesTuristicos_PacoteTuristicoId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_CidadesDestinos_PacoteTuristicoId",
                table: "CidadesDestinos");

            migrationBuilder.DropColumn(
                name: "PacoteTuristicoId",
                table: "CidadesDestinos");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "PacotesTuristicos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "CidadesDestinos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "CidadesDestinos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PacoteTuristicoDestinos",
                columns: table => new
                {
                    DestinosId = table.Column<int>(type: "int", nullable: false),
                    PacotesTuristicosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacoteTuristicoDestinos", x => new { x.DestinosId, x.PacotesTuristicosId });
                    table.ForeignKey(
                        name: "FK_PacoteTuristicoDestinos_CidadesDestinos_DestinosId",
                        column: x => x.DestinosId,
                        principalTable: "CidadesDestinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PacoteTuristicoDestinos_PacotesTuristicos_PacotesTuristicosId",
                        column: x => x.PacotesTuristicosId,
                        principalTable: "PacotesTuristicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PacoteTuristicoDestinos_PacotesTuristicosId",
                table: "PacoteTuristicoDestinos",
                column: "PacotesTuristicosId");

            migrationBuilder.AddForeignKey(
                name: "FK_CidadesDestinos_PaisesDestinos_PaisDestinoId",
                table: "CidadesDestinos",
                column: "PaisDestinoId",
                principalTable: "PaisesDestinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Clientes_ClienteId",
                table: "Reservas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_PacotesTuristicos_PacoteTuristicoId",
                table: "Reservas",
                column: "PacoteTuristicoId",
                principalTable: "PacotesTuristicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CidadesDestinos_PaisesDestinos_PaisDestinoId",
                table: "CidadesDestinos");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Clientes_ClienteId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_PacotesTuristicos_PacoteTuristicoId",
                table: "Reservas");

            migrationBuilder.DropTable(
                name: "PacoteTuristicoDestinos");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "PacotesTuristicos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Pais",
                table: "CidadesDestinos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "CidadesDestinos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "PacoteTuristicoId",
                table: "CidadesDestinos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CidadesDestinos_PacoteTuristicoId",
                table: "CidadesDestinos",
                column: "PacoteTuristicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CidadesDestinos_PacotesTuristicos_PacoteTuristicoId",
                table: "CidadesDestinos",
                column: "PacoteTuristicoId",
                principalTable: "PacotesTuristicos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CidadesDestinos_PaisesDestinos_PaisDestinoId",
                table: "CidadesDestinos",
                column: "PaisDestinoId",
                principalTable: "PaisesDestinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Clientes_ClienteId",
                table: "Reservas",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_PacotesTuristicos_PacoteTuristicoId",
                table: "Reservas",
                column: "PacoteTuristicoId",
                principalTable: "PacotesTuristicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
