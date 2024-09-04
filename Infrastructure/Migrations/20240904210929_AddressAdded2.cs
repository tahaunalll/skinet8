using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddressAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_AdressID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AdressID",
                table: "AspNetUsers",
                newName: "AddressID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_AdressID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_AddressID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressID",
                table: "AspNetUsers",
                column: "AddressID",
                principalTable: "Addresses",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_AddressID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AddressID",
                table: "AspNetUsers",
                newName: "AdressID");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_AddressID",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_AdressID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_AdressID",
                table: "AspNetUsers",
                column: "AdressID",
                principalTable: "Addresses",
                principalColumn: "ID");
        }
    }
}
