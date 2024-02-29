using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wikirace.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_IsOwner",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_IsWinner",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
