using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class HomesG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inmuebles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoPropiedad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contenido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagenPrincipal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ubicaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Luxury = table.Column<bool>(type: "bit", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Parqueos = table.Column<int>(type: "int", nullable: false),
                    Habitaciones = table.Column<int>(type: "int", nullable: false),
                    Banos = table.Column<int>(type: "int", nullable: false),
                    Niveles = table.Column<int>(type: "int", nullable: false),
                    MetrosCuadrados = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Video = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inmuebles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmenidadesInmueble",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InmuebleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenidadesInmueble", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmenidadesInmueble_Inmuebles_InmuebleId",
                        column: x => x.InmuebleId,
                        principalTable: "Inmuebles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagenesInmueble",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InmuebleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagenesInmueble", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagenesInmueble_Inmuebles_InmuebleId",
                        column: x => x.InmuebleId,
                        principalTable: "Inmuebles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmenidadesInmueble_InmuebleId",
                table: "AmenidadesInmueble",
                column: "InmuebleId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagenesInmueble_InmuebleId",
                table: "ImagenesInmueble",
                column: "InmuebleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmenidadesInmueble");

            migrationBuilder.DropTable(
                name: "ImagenesInmueble");

            migrationBuilder.DropTable(
                name: "Inmuebles");
        }
    }
}
