using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wikirace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddConditionsToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Games");

            migrationBuilder.CreateIndex(
                name: "IX_Players_IsOwner",
                table: "Players",
                column: "IsOwner",
                unique: true,
                filter: "[IsOwner] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Players_IsWinner",
                table: "Players",
                column: "IsWinner",
                unique: true,
                filter: "[IsWinner] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_IsOwner",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_IsWinner",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "WinnerId",
                table: "Games",
                type: "TEXT",
                nullable: true);
        }
    }
}
