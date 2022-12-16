using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerConnect.Infrastructure.Migrations
{
    public partial class RemoveJobStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Jobs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Jobs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
