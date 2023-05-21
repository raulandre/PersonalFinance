using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinance.Migrations
{
    /// <inheritdoc />
    public partial class CreateBalanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BalanceId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Salary = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_BalanceId",
                table: "Users",
                column: "BalanceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Balances_BalanceId",
                table: "Users",
                column: "BalanceId",
                principalTable: "Balances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Balances_BalanceId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Balances");

            migrationBuilder.DropIndex(
                name: "IX_Users_BalanceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BalanceId",
                table: "Users");
        }
    }
}
