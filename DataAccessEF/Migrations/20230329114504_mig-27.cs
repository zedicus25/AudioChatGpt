using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessEF.Migrations
{
    /// <inheritdoc />
    public partial class mig27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastRequest",
                table: "Users",
                newName: "LastTextRequest");

            migrationBuilder.RenameColumn(
                name: "CountRequests",
                table: "Users",
                newName: "CountTextRequests");

            migrationBuilder.AddColumn<int>(
                name: "CountImageRequests",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastImageRequest",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountImageRequests",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastImageRequest",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "LastTextRequest",
                table: "Users",
                newName: "LastRequest");

            migrationBuilder.RenameColumn(
                name: "CountTextRequests",
                table: "Users",
                newName: "CountRequests");
        }
    }
}
