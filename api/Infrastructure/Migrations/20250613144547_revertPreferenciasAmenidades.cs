using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class revertPreferenciasAmenidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
        name: "FK_PreferenciaAmenidad_Preferencias_PreferenciasId",
        table: "PreferenciaAmenidad");

            // Eliminar PK existente
            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciaAmenidad",
                table: "PreferenciaAmenidad");

            // Eliminar columna Id
            migrationBuilder.DropColumn(
                name: "Id",
                table: "PreferenciaAmenidad");

            // Renombrar la tabla
            migrationBuilder.RenameTable(
                name: "PreferenciaAmenidad",
                newName: "PreferenciasAmenidades");

            // Agregar la columna Id como IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PreferenciasAmenidades",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Agregar PK
            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades",
                column: "Id");

            // Agregar FK nuevamente
            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciasAmenidades_Preferencias_PreferenciasId",
                table: "PreferenciasAmenidades",
                column: "PreferenciasId",
                principalTable: "Preferencias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
