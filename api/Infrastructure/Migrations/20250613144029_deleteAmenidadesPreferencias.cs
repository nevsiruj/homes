using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteAmenidadesPreferencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciasAmenidades_Preferencias_PreferenciasId",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades");

            migrationBuilder.RenameTable(
                name: "PreferenciasAmenidades",
                newName: "PreferenciaAmenidad");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciaAmenidad",
                table: "PreferenciaAmenidad",
                column: "PreferenciasId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciaAmenidad_Preferencias_PreferenciasId",
                table: "PreferenciaAmenidad",
                column: "PreferenciasId",
                principalTable: "Preferencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciaAmenidad_Preferencias_PreferenciasId",
                table: "PreferenciaAmenidad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciaAmenidad",
                table: "PreferenciaAmenidad");

            migrationBuilder.RenameTable(
                name: "PreferenciaAmenidad",
                newName: "PreferenciasAmenidades");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades",
                column: "PreferenciasId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciasAmenidades_Preferencias_PreferenciasId",
                table: "PreferenciasAmenidades",
                column: "PreferenciasId",
                principalTable: "Preferencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
