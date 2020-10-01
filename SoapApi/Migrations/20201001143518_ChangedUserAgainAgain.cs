using Microsoft.EntityFrameworkCore.Migrations;

namespace SoapApi.Migrations
{
    public partial class ChangedUserAgainAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStore",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserStore",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
