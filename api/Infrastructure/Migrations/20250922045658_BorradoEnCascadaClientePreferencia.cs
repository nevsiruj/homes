using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BorradoEnCascadaClientePreferencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Preferencias_Clientes_ClienteId",
                table: "Preferencias");

            // Option 1: Delete all records with NULL ClienteId
            // This is the cleanest option if you don't need these records.
            migrationBuilder.Sql("DELETE FROM Preferencias WHERE ClienteId IS NULL;");

            // Option 2: Update NULL values to a valid ClienteId (e.g., a dummy client)
            // You would need to know a valid ID, for example, 1.
            // migrationBuilder.Sql("UPDATE Preferencias SET ClienteId = 1 WHERE ClienteId IS NULL;");

            // Now, alter the column to be non-nullable
            // Note: We remove the 'defaultValue: 0' since it's causing the conflict
            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Preferencias",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Add the new foreign key with a cascade delete rule
            migrationBuilder.AddForeignKey(
                name: "FK_Preferencias_Clientes_ClienteId",
                table: "Preferencias",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // The down method remains the same as it correctly reverses the Up migration.
            migrationBuilder.DropForeignKey(
                name: "FK_Preferencias_Clientes_ClienteId",
                table: "Preferencias");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Preferencias",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Preferencias_Clientes_ClienteId",
                table: "Preferencias",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id");
        }
    }
}