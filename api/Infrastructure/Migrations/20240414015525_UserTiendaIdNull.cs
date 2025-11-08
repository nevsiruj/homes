using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserTiendaIdNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TiendaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TiendaId",
                table: "AspNetUsers",
                column: "TiendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tiendas_TiendaId",
                table: "AspNetUsers",
                column: "TiendaId",
                principalTable: "Tiendas",
                principalColumn: "Id");

            migrationBuilder.Sql("UPDATE AspNetUsers SET TiendaId = 4");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tiendas_TiendaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TiendaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TiendaId",
                table: "AspNetUsers");
        }
    }
}
