using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessEF.Migrations
{
    /// <inheritdoc />
    public partial class mig30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaxRequests",
                table: "Subscriptions",
                newName: "MaxTextRequests");

            migrationBuilder.AddColumn<int>(
                name: "MaxImageRequests",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxImageRequests",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "MaxTextRequests",
                table: "Subscriptions",
                newName: "MaxRequests");
        }
    }
}
