using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AmenidadId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificacionesAgentes_AspNetUsers_AgenteId",
                table: "CertificacionesAgentes");

            migrationBuilder.DropIndex(
                name: "IX_CertificacionesAgentes_AgenteId",
                table: "CertificacionesAgentes");

            migrationBuilder.AlterColumn<int>(
                name: "AgenteId",
                table: "CertificacionesAgentes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AgenteId1",
                table: "CertificacionesAgentes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CertificacionesAgentes_AgenteId1",
                table: "CertificacionesAgentes",
                column: "AgenteId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificacionesAgentes_AspNetUsers_AgenteId1",
                table: "CertificacionesAgentes",
                column: "AgenteId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CertificacionesAgentes_AspNetUsers_AgenteId1",
                table: "CertificacionesAgentes");

            migrationBuilder.DropIndex(
                name: "IX_CertificacionesAgentes_AgenteId1",
                table: "CertificacionesAgentes");

            migrationBuilder.DropColumn(
                name: "AgenteId1",
                table: "CertificacionesAgentes");

            migrationBuilder.AlterColumn<string>(
                name: "AgenteId",
                table: "CertificacionesAgentes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CertificacionesAgentes_AgenteId",
                table: "CertificacionesAgentes",
                column: "AgenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_CertificacionesAgentes_AspNetUsers_AgenteId",
                table: "CertificacionesAgentes",
                column: "AgenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
