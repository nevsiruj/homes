using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditTienda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "Insumos");

            migrationBuilder.RenameColumn(
                name: "imagenPrincipal",
                table: "Insumos",
                newName: "Nombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Insumos",
                newName: "imagenPrincipal");

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "Insumos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
