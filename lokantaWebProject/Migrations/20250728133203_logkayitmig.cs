using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lokantaWebProject.Migrations
{
    /// <inheritdoc />
    public partial class logkayitmig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "AdminLoginLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "AdminLoginLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "AdminLoginLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "AdminLoginLogs");

            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "AdminLoginLogs");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "AdminLoginLogs");
        }
    }
}
