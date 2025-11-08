using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTiposTarjetas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoTarjetaId",
                table: "Ventas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TiposTarjetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTarjetas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_TipoTarjetaId",
                table: "Ventas",
                column: "TipoTarjetaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_TiposTarjetas_TipoTarjetaId",
                table: "Ventas",
                column: "TipoTarjetaId",
                principalTable: "TiposTarjetas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_TiposTarjetas_TipoTarjetaId",
                table: "Ventas");

            migrationBuilder.DropTable(
                name: "TiposTarjetas");

            migrationBuilder.DropIndex(
                name: "IX_Ventas_TipoTarjetaId",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "TipoTarjetaId",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Clientes");
        }
    }
}
