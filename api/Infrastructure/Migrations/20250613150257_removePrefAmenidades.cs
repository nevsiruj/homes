using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removePrefAmenidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Eliminar PK existente
            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades");

            // 2. Eliminar la columna Id actual
            migrationBuilder.DropColumn(
                name: "Id",
                table: "PreferenciasAmenidades");

            // 3. Volver a crear la columna Id como IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PreferenciasAmenidades",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // 4. Volver a definir la clave primaria sobre Id
            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades",
                column: "Id");

            // 5. (Opcional) Agregar índice si lo tenías antes
            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasAmenidades_PreferenciasId",
                table: "PreferenciasAmenidades",
                column: "PreferenciasId");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropIndex(
                name: "IX_PreferenciasAmenidades_PreferenciasId",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PreferenciasAmenidades");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PreferenciasAmenidades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades",
                column: "Id");
        }
    }
}
