using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceApp.Migrations
{
    /// <inheritdoc />
    public partial class ClientModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Invoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostCode = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ClientId",
                table: "Items",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ClientId",
                table: "Customers",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Client_ClientId",
                table: "Customers",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Client_ClientId",
                table: "Invoices",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Client_ClientId",
                table: "Items",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Client_ClientId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Client_ClientId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Client_ClientId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Items_ClientId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Customers_ClientId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Customers");
        }
    }
}
