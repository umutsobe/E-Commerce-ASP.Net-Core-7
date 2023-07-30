using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_trade_api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameSurname",
                table: "AspNetUsers",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "NameSurname");
        }
    }
}
