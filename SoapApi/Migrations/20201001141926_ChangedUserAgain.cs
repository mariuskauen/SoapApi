using Microsoft.EntityFrameworkCore.Migrations;

namespace SoapApi.Migrations
{
    public partial class ChangedUserAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttendingEventStore",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MyEventStore",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendingEventStore",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MyEventStore",
                table: "Users");
        }
    }
}
