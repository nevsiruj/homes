using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefDireccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoordXY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FotoDniPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FotoClientePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FotoVendedorPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroHogar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CicloFacturacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimoDiaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodigoOrigen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoVenta = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ventas");
        }
    }
}
