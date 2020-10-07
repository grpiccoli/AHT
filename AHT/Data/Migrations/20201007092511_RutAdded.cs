using Microsoft.EntityFrameworkCore.Migrations;

namespace AHT.Data.Migrations
{
    public partial class RutAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RUT",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RUT",
                table: "AspNetUsers");
        }
    }
}
