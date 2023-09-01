using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_trade_api.Persistence.Migrations
{
    public partial class mig_6ProductRatingsAndDataAnnotations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRating_AspNetUsers_UserId",
                table: "ProductRating");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRating_Products_ProductId",
                table: "ProductRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRating",
                table: "ProductRating");

            migrationBuilder.RenameTable(
                name: "ProductRating",
                newName: "ProductRatings");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRating_UserId",
                table: "ProductRatings",
                newName: "IX_ProductRatings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRating_ProductId",
                table: "ProductRatings",
                newName: "IX_ProductRatings_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRatings",
                table: "ProductRatings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_AspNetUsers_UserId",
                table: "ProductRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRatings_Products_ProductId",
                table: "ProductRatings",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_AspNetUsers_UserId",
                table: "ProductRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRatings_Products_ProductId",
                table: "ProductRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRatings",
                table: "ProductRatings");

            migrationBuilder.RenameTable(
                name: "ProductRatings",
                newName: "ProductRating");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRatings_UserId",
                table: "ProductRating",
                newName: "IX_ProductRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRatings_ProductId",
                table: "ProductRating",
                newName: "IX_ProductRating_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRating",
                table: "ProductRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRating_AspNetUsers_UserId",
                table: "ProductRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRating_Products_ProductId",
                table: "ProductRating",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
