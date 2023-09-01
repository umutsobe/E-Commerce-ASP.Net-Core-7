using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_trade_api.Persistence.Migrations
{
    public partial class mig_6TotalBasketAddedTotalOrderNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalBasketAdded",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalOrderNumber",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBasketAdded",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TotalOrderNumber",
                table: "Products");
        }
    }
}
