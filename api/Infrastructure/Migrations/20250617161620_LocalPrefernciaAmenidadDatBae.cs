using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LocalPrefernciaAmenidadDatBae : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciaAmenidades_Preferencias_PreferenciasId",
                table: "PreferenciaAmenidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciaAmenidades",
                table: "PreferenciaAmenidades");

            migrationBuilder.RenameTable(
                name: "PreferenciaAmenidades",
                newName: "PreferenciaAmenidad");

            migrationBuilder.RenameIndex(
                name: "IX_PreferenciaAmenidades_PreferenciasId",
                table: "PreferenciaAmenidad",
                newName: "IX_PreferenciaAmenidad_PreferenciasId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciaAmenidad",
                table: "PreferenciaAmenidad",
                column: "Id");

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
                newName: "PreferenciaAmenidades");

            migrationBuilder.RenameIndex(
                name: "IX_PreferenciaAmenidad_PreferenciasId",
                table: "PreferenciaAmenidades",
                newName: "IX_PreferenciaAmenidades_PreferenciasId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciaAmenidades",
                table: "PreferenciaAmenidades",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciaAmenidades_Preferencias_PreferenciasId",
                table: "PreferenciaAmenidades",
                column: "PreferenciasId",
                principalTable: "Preferencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
