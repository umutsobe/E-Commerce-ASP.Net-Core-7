using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_trade_api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Baskets_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_UserId",
                table: "Baskets");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_UserId",
                table: "Baskets",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Baskets_UserId",
                table: "Baskets");

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_UserId",
                table: "Baskets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Baskets_Id",
                table: "Orders",
                column: "Id",
                principalTable: "Baskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
