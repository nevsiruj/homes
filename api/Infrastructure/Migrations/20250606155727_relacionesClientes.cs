using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class relacionesClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificacionAgente_AspNetUsers_AgenteId",
                table: "CertificacionAgente");

            migrationBuilder.DropForeignKey(
                name: "FK_Interaccion_AspNetUsers_AgenteId",
                table: "Interaccion");

            migrationBuilder.DropForeignKey(
                name: "FK_Interaccion_Clientes_ClienteId",
                table: "Interaccion");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciaAmenidad_AmenidadesInmueble_AmenidadInmuebleId",
                table: "PreferenciaAmenidad");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciaAmenidad_Preferencias_PreferenciasId",
                table: "PreferenciaAmenidad");

            migrationBuilder.DropTable(
                name: "ClienteAtendido");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciaAmenidad",
                table: "PreferenciaAmenidad");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interaccion",
                table: "Interaccion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CertificacionAgente",
                table: "CertificacionAgente");

            migrationBuilder.RenameTable(
                name: "PreferenciaAmenidad",
                newName: "PreferenciasAmenidades");

            migrationBuilder.RenameTable(
                name: "Interaccion",
                newName: "Interacciones");

            migrationBuilder.RenameTable(
                name: "CertificacionAgente",
                newName: "CertificacionesAgentes");

            migrationBuilder.RenameIndex(
                name: "IX_PreferenciaAmenidad_AmenidadInmuebleId",
                table: "PreferenciasAmenidades",
                newName: "IX_PreferenciasAmenidades_AmenidadInmuebleId");

            migrationBuilder.RenameIndex(
                name: "IX_Interaccion_ClienteId",
                table: "Interacciones",
                newName: "IX_Interacciones_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Interaccion_AgenteId",
                table: "Interacciones",
                newName: "IX_Interacciones_AgenteId");

            migrationBuilder.RenameIndex(
                name: "IX_CertificacionAgente_AgenteId",
                table: "CertificacionesAgentes",
                newName: "IX_CertificacionesAgentes_AgenteId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Clientes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Clientes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "AgenteId",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades",
                columns: new[] { "PreferenciasId", "AmenidadInmuebleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interacciones",
                table: "Interacciones",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CertificacionesAgentes",
                table: "CertificacionesAgentes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_AgenteId",
                table: "Clientes",
                column: "AgenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificacionesAgentes_AspNetUsers_AgenteId",
                table: "CertificacionesAgentes",
                column: "AgenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_AgenteId",
                table: "Clientes",
                column: "AgenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Interacciones_AspNetUsers_AgenteId",
                table: "Interacciones",
                column: "AgenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interacciones_Clientes_ClienteId",
                table: "Interacciones",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciasAmenidades_AmenidadesInmueble_AmenidadInmuebleId",
                table: "PreferenciasAmenidades",
                column: "AmenidadInmuebleId",
                principalTable: "AmenidadesInmueble",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_CertificacionesAgentes_AspNetUsers_AgenteId",
                table: "CertificacionesAgentes");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_AgenteId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Interacciones_AspNetUsers_AgenteId",
                table: "Interacciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Interacciones_Clientes_ClienteId",
                table: "Interacciones");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciasAmenidades_AmenidadesInmueble_AmenidadInmuebleId",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciasAmenidades_Preferencias_PreferenciasId",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_AgenteId",
                table: "Clientes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreferenciasAmenidades",
                table: "PreferenciasAmenidades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Interacciones",
                table: "Interacciones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CertificacionesAgentes",
                table: "CertificacionesAgentes");

            migrationBuilder.DropColumn(
                name: "AgenteId",
                table: "Clientes");

            migrationBuilder.RenameTable(
                name: "PreferenciasAmenidades",
                newName: "PreferenciaAmenidad");

            migrationBuilder.RenameTable(
                name: "Interacciones",
                newName: "Interaccion");

            migrationBuilder.RenameTable(
                name: "CertificacionesAgentes",
                newName: "CertificacionAgente");

            migrationBuilder.RenameIndex(
                name: "IX_PreferenciasAmenidades_AmenidadInmuebleId",
                table: "PreferenciaAmenidad",
                newName: "IX_PreferenciaAmenidad_AmenidadInmuebleId");

            migrationBuilder.RenameIndex(
                name: "IX_Interacciones_ClienteId",
                table: "Interaccion",
                newName: "IX_Interaccion_ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Interacciones_AgenteId",
                table: "Interaccion",
                newName: "IX_Interaccion_AgenteId");

            migrationBuilder.RenameIndex(
                name: "IX_CertificacionesAgentes_AgenteId",
                table: "CertificacionAgente",
                newName: "IX_CertificacionAgente_AgenteId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Clientes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreferenciaAmenidad",
                table: "PreferenciaAmenidad",
                columns: new[] { "PreferenciasId", "AmenidadInmuebleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Interaccion",
                table: "Interaccion",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CertificacionAgente",
                table: "CertificacionAgente",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClienteAtendido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgenteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteAtendido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClienteAtendido_AspNetUsers_AgenteId",
                        column: x => x.AgenteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClienteAtendido_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteAtendido_AgenteId",
                table: "ClienteAtendido",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteAtendido_ClienteId",
                table: "ClienteAtendido",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificacionAgente_AspNetUsers_AgenteId",
                table: "CertificacionAgente",
                column: "AgenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interaccion_AspNetUsers_AgenteId",
                table: "Interaccion",
                column: "AgenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Interaccion_Clientes_ClienteId",
                table: "Interaccion",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciaAmenidad_AmenidadesInmueble_AmenidadInmuebleId",
                table: "PreferenciaAmenidad",
                column: "AmenidadInmuebleId",
                principalTable: "AmenidadesInmueble",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
