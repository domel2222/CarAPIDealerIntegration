using Microsoft.EntityFrameworkCore.Migrations;

namespace CarDealerAPI.Migrations
{
    public partial class userColorEye : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ColorEye",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColorEye",
                table: "Users");
        }
    }
}
