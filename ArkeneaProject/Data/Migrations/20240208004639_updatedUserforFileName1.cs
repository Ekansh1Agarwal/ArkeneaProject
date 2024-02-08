using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArkeneaProject.Data.Migrations
{
    public partial class updatedUserforFileName1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserData");
        }
    }
}
