using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatePreferenciaInmueble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciasAmenidades_AmenidadesInmueble_AmenidadInmuebleId",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropIndex(
                name: "IX_PreferenciasAmenidades_AmenidadInmuebleId",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropColumn(
                name: "AmenidadInmuebleId",
                table: "PreferenciasAmenidades");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades",
                column: "PreferenciasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades");

            migrationBuilder.AddColumn<int>(
                name: "AmenidadInmuebleId",
                table: "PreferenciasAmenidades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades",
                columns: new[] { "PreferenciasId", "AmenidadInmuebleId" });

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciasAmenidades_AmenidadInmuebleId",
                table: "PreferenciasAmenidades",
                column: "AmenidadInmuebleId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciasAmenidades_AmenidadesInmueble_AmenidadInmuebleId",
                table: "PreferenciasAmenidades",
                column: "AmenidadInmuebleId",
                principalTable: "AmenidadesInmueble",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
