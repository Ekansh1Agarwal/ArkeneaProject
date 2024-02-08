using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArkeneaProject.Data.Migrations
{
    public partial class updatedUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "UserData",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "UserData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "UserData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "UserData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "UserData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "file",
                table: "UserData",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "UserData");

            migrationBuilder.DropColumn(
                name: "file",
                table: "UserData");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "UserData",
                newName: "Name");
        }
    }
}
