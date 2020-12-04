using Microsoft.EntityFrameworkCore.Migrations;

namespace PlaystationWishlist.DataAccess.Migrations
{
    public partial class AddedImageUrlForGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountPercentage",
                table: "PlaystationGames",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameImageUrl",
                table: "PlaystationGames",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "PlaystationGames");

            migrationBuilder.DropColumn(
                name: "GameImageUrl",
                table: "PlaystationGames");
        }
    }
}
