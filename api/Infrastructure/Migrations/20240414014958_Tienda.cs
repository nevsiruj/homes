using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Tienda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Insumos",
                table: "Insumos");

            migrationBuilder.RenameTable(
                name: "Insumos",
                newName: "Tiendas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tiendas",
                table: "Tiendas",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tiendas",
                table: "Tiendas");

            migrationBuilder.RenameTable(
                name: "Tiendas",
                newName: "Insumos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Insumos",
                table: "Insumos",
                column: "Id");
        }
    }
}
