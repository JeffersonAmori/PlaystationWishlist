using Microsoft.EntityFrameworkCore.Migrations;

namespace PlaystationWishlist.DataAccess.Migrations
{
    public partial class Migration2020112403 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "FinalPrice",
                table: "PlaystationGames",
                type: "decimal (18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal (18,2)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
              name: "FinalPrice",
              table: "PlaystationGames",
              type: "decimal (18,2)",
              nullable: false,
              oldClrType: typeof(decimal),
              oldType: "decimal (18,2)");
        }
    }
}
