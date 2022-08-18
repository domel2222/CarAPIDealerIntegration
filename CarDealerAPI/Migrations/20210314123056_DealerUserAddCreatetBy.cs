using Microsoft.EntityFrameworkCore.Migrations;

namespace CarDealerAPI.Migrations
{
    public partial class DealerUserAddCreatetBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Dealers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dealers_CreatedById",
                table: "Dealers",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Dealers_Users_CreatedById",
                table: "Dealers",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dealers_Users_CreatedById",
                table: "Dealers");

            migrationBuilder.DropIndex(
                name: "IX_Dealers_CreatedById",
                table: "Dealers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Dealers");
        }
    }
}
