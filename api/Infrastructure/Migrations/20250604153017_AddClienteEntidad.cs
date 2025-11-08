using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteEntidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_LiderId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_VendedorId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_LiderId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "LiderId",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "VendedorId",
                table: "Clientes",
                newName: "ApplicationUserId");

            migrationBuilder.RenameColumn(
                name: "Nota",
                table: "Clientes",
                newName: "Notas");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_VendedorId",
                table: "Clientes",
                newName: "IX_Clientes_ApplicationUserId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaRegistro",
                table: "Clientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Interaccion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgenteId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgenteNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interaccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interaccion_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preferencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Operacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrecioMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Habitaciones = table.Column<int>(type: "int", nullable: false),
                    Banos = table.Column<int>(type: "int", nullable: false),
                    MetrosCuadrados = table.Column<int>(type: "int", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferencias_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreferenciaAmenidad",
                columns: table => new
                {
                    PreferenciasId = table.Column<int>(type: "int", nullable: false),
                    AmenidadInmuebleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferenciaAmenidad", x => new { x.PreferenciasId, x.AmenidadInmuebleId });
                    table.ForeignKey(
                        name: "FK_PreferenciaAmenidad_AmenidadesInmueble_AmenidadInmuebleId",
                        column: x => x.AmenidadInmuebleId,
                        principalTable: "AmenidadesInmueble",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferenciaAmenidad_Preferencias_PreferenciasId",
                        column: x => x.PreferenciasId,
                        principalTable: "Preferencias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interaccion_ClienteId",
                table: "Interaccion",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciaAmenidad_AmenidadInmuebleId",
                table: "PreferenciaAmenidad",
                column: "AmenidadInmuebleId");

            migrationBuilder.CreateIndex(
                name: "IX_Preferencias_ClienteId",
                table: "Preferencias",
                column: "ClienteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_ApplicationUserId",
                table: "Clientes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_ApplicationUserId",
                table: "Clientes");

            migrationBuilder.DropTable(
                name: "Interaccion");

            migrationBuilder.DropTable(
                name: "PreferenciaAmenidad");

            migrationBuilder.DropTable(
                name: "Preferencias");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "Notas",
                table: "Clientes",
                newName: "Nota");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Clientes",
                newName: "VendedorId");

            migrationBuilder.RenameIndex(
                name: "IX_Clientes_ApplicationUserId",
                table: "Clientes",
                newName: "IX_Clientes_VendedorId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaRegistro",
                table: "Clientes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "LiderId",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_LiderId",
                table: "Clientes",
                column: "LiderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_LiderId",
                table: "Clientes",
                column: "LiderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_VendedorId",
                table: "Clientes",
                column: "VendedorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
