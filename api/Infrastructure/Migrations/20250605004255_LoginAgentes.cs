using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LoginAgentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_ApplicationUserId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_ApplicationUserId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "AgenteNombre",
                table: "Interaccion");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "DireccionDomicilio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FechaIngreso",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Identificador",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TiendaId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Whatsapp",
                table: "AspNetUsers",
                newName: "Rol");

            migrationBuilder.RenameColumn(
                name: "ProfileImagePath",
                table: "AspNetUsers",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "EstadoCivil",
                table: "AspNetUsers",
                newName: "Notas");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PreferenciaAmenidad",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "AgenteId",
                table: "Interaccion",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Experiencia",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropiedadesVendidas",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorVendidoTotal",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CertificacionAgente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgenteId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificacionAgente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificacionAgente_AspNetUsers_AgenteId",
                        column: x => x.AgenteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClienteAtendido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgenteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
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
                name: "IX_Interaccion_AgenteId",
                table: "Interaccion",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificacionAgente_AgenteId",
                table: "CertificacionAgente",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteAtendido_AgenteId",
                table: "ClienteAtendido",
                column: "AgenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteAtendido_ClienteId",
                table: "ClienteAtendido",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interaccion_AspNetUsers_AgenteId",
                table: "Interaccion",
                column: "AgenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interaccion_AspNetUsers_AgenteId",
                table: "Interaccion");

            migrationBuilder.DropTable(
                name: "CertificacionAgente");

            migrationBuilder.DropTable(
                name: "ClienteAtendido");

            migrationBuilder.DropIndex(
                name: "IX_Interaccion_AgenteId",
                table: "Interaccion");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PreferenciaAmenidad");

            migrationBuilder.DropColumn(
                name: "Especialidad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Experiencia",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PropiedadesVendidas",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ValorVendidoTotal",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "AspNetUsers",
                newName: "ProfileImagePath");

            migrationBuilder.RenameColumn(
                name: "Rol",
                table: "AspNetUsers",
                newName: "Whatsapp");

            migrationBuilder.RenameColumn(
                name: "Notas",
                table: "AspNetUsers",
                newName: "EstadoCivil");

            migrationBuilder.AlterColumn<string>(
                name: "AgenteId",
                table: "Interaccion",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AgenteNombre",
                table: "Interaccion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Dni",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Apellido",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DireccionDomicilio",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaIngreso",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Identificador",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TiendaId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_ApplicationUserId",
                table: "Clientes",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_ApplicationUserId",
                table: "Clientes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
