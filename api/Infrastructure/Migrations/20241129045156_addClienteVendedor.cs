using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addClienteVendedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendedorId",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_VendedorId",
                table: "Clientes",
                column: "VendedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_VendedorId",
                table: "Clientes",
                column: "VendedorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_VendedorId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_VendedorId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "VendedorId",
                table: "Clientes");
        }
    }
}
