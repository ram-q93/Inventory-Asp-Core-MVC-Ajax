using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_Asp_Core_MVC_Ajax.DataAccess.Migrations
{
    public partial class changeProductColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enable",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "Enable",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
