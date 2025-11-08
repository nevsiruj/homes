using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addLiderCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_LiderId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_LiderId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "LiderId",
                table: "Clientes");
        }
    }
}
