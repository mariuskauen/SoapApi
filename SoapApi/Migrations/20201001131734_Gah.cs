using Microsoft.EntityFrameworkCore.Migrations;

namespace SoapApi.Migrations
{
    public partial class Gah : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Users",
                table: "FriendRequest");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "FriendRequest",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "FriendRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "FriendRequest");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "FriendRequest");

            migrationBuilder.AddColumn<string>(
                name: "Users",
                table: "FriendRequest",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
