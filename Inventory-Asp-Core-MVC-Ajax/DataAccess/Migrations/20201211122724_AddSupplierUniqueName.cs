using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_Asp_Core_MVC_Ajax.Migrations
{
    public partial class AddSupplierUniqueName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                table: "Suppliers",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Suppliers_Name",
                table: "Suppliers");
        }
    }
}
