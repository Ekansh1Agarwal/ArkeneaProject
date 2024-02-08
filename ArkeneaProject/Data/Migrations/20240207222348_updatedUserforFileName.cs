using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArkeneaProject.Data.Migrations
{
    public partial class updatedUserforFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fileName",
                table: "UserData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileName",
                table: "UserData");
        }
    }
}
