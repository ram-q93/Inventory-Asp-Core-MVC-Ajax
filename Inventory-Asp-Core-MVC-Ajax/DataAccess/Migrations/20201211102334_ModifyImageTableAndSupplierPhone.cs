using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_Asp_Core_MVC_Ajax.Migrations
{
    public partial class ModifyImageTableAndSupplierPhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Products_productId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_productId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "productId",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Suppliers",
                type: "varchar(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(14)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "varchar(150)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "Images",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ImageId",
                table: "Products",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Images_ImageId",
                table: "Products",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Images_ImageId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ImageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Suppliers",
                type: "varchar(14)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Suppliers",
                type: "varchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(150)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_productId",
                table: "Images",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Products_productId",
                table: "Images",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
