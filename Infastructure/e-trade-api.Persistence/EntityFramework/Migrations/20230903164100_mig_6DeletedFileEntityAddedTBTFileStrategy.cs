using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_trade_api.Persistence.Migrations
{
    public partial class mig_6DeletedFileEntityAddedTBTFileStrategy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductImageFile_Files_ProductImageFilesId",
                table: "ProductProductImageFile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "ProductImageFiles");

            migrationBuilder.AlterColumn<bool>(
                name: "Showcase",
                table: "ProductImageFiles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImageFiles",
                table: "ProductImageFiles",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "InvoiceFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Storage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceFiles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductImageFile_ProductImageFiles_ProductImageFilesId",
                table: "ProductProductImageFile",
                column: "ProductImageFilesId",
                principalTable: "ProductImageFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductImageFile_ProductImageFiles_ProductImageFilesId",
                table: "ProductProductImageFile");

            migrationBuilder.DropTable(
                name: "InvoiceFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImageFiles",
                table: "ProductImageFiles");

            migrationBuilder.RenameTable(
                name: "ProductImageFiles",
                newName: "Files");

            migrationBuilder.AlterColumn<bool>(
                name: "Showcase",
                table: "Files",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Files",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductImageFile_Files_ProductImageFilesId",
                table: "ProductProductImageFile",
                column: "ProductImageFilesId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
