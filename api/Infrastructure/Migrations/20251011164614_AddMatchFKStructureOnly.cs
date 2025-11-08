using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchFKStructureOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. PASO DE LIMPIEZA DE DATOS (CRUCIAL):
            // Elimina todos los registros de 'Matches' donde el 'InmuebleId'
            // no corresponde a un 'Id' existente en la tabla 'Inmuebles'.
            migrationBuilder.Sql(@"
                DELETE FROM Matches
                WHERE NOT EXISTS (
                    SELECT 1 FROM Inmuebles i WHERE i.Id = Matches.InmuebleId
                );
            ");

            // 2. CREAR ÍNDICE:
            // Crea el índice para optimizar la búsqueda y la integridad.
            migrationBuilder.CreateIndex(
                name: "IX_Matches_InmuebleId",
                table: "Matches",
                column: "InmuebleId");

            // 3. AÑADIR CLAVE FORÁNEA:
            // Ahora, la adición de la FK no entrará en conflicto con los datos existentes.
            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Inmuebles_InmuebleId",
                table: "Matches",
                column: "InmuebleId",
                principalTable: "Inmuebles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // El método Down revierte las acciones anteriores
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Inmuebles_InmuebleId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_InmuebleId",
                table: "Matches");

            // NOTA: No se revierte la limpieza de datos en el Down(), ya que se considera pérdida de datos.
        }
    }
}