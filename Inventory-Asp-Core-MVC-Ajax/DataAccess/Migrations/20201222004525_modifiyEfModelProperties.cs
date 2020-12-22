using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory_Asp_Core_MVC_Ajax.Migrations
{
    public partial class modifiyEfModelProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Suppliers",
                newName: "CompanyName");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_Name",
                table: "Suppliers",
                newName: "IX_Suppliers_CompanyName");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Suppliers",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Suppliers",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactTitle",
                table: "Suppliers",
                type: "varchar(5)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Suppliers",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Suppliers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fax",
                table: "Suppliers",
                type: "varchar(14)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomePage",
                table: "Suppliers",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Suppliers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Suppliers",
                type: "varchar(14)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Suppliers",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Storages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Storages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Storages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Storages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Products",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "ContactTitle",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Fax",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "HomePage",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Storages");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Suppliers",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_CompanyName",
                table: "Suppliers",
                newName: "IX_Suppliers_Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Suppliers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Suppliers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Storages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Storages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
