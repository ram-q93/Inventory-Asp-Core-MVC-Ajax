using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_Asp_Core_MVC_Ajax.Migrations
{
    public partial class AddCityPropToStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Storages",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Storages");
        }
    }
}
