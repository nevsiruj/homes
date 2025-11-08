using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SolucionDaronParte1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciaAmenidad_Preferencias_PreferenciasId",
                table: "PreferenciaAmenidad");

            migrationBuilder.AlterColumn<int>(
                name: "PreferenciasId",
                table: "PreferenciaAmenidad",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AmenidadId",
                table: "PreferenciaAmenidad",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Amenidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenidades", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreferenciaAmenidad_AmenidadId",
                table: "PreferenciaAmenidad",
                column: "AmenidadId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciaAmenidad_Amenidades_AmenidadId",
                table: "PreferenciaAmenidad",
                column: "AmenidadId",
                principalTable: "Amenidades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreferenciaAmenidad_Preferencias_PreferenciasId",
                table: "PreferenciaAmenidad",
                column: "PreferenciasId",
                principalTable: "Preferencias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciaAmenidad_Amenidades_AmenidadId",
                table: "PreferenciaAmenidad");

            migrationBuilder.DropForeignKey(
                name: "FK_PreferenciaAmenidad_Preferencias_PreferenciasId",
                table: "PreferenciaAmenidad");

            migrationBuilder.DropTable(
                name: "Amenidades");

            migrationBuilder.DropIndex(
                name: "IX_PreferenciaAmenidad_AmenidadId",
                table: "PreferenciaAmenidad");

            migrationBuilder.DropColumn(
                name: "AmenidadId",
                table: "PreferenciaAmenidad");

            migrationBuilder.AlterColumn<int>(
                name: "PreferenciasId",
                table: "PreferenciaAmenidad",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
