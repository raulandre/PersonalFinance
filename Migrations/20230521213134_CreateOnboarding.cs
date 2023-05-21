using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinance.Migrations
{
    /// <inheritdoc />
    public partial class CreateOnboarding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Onboarding",
                table: "Users",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Onboarding",
                table: "Users");
        }
    }
}
