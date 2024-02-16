using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wikirace.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Players",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_AppUserId",
                table: "Players",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_AspNetUsers_AppUserId",
                table: "Players",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_AspNetUsers_AppUserId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_AppUserId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Players");
        }
    }
}
