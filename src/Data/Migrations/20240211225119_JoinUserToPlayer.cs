using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wikirace.Data.Migrations
{
    /// <inheritdoc />
    public partial class JoinUserToPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserId",
                table: "Players",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_AspNetUsers_UserId",
                table: "Players",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_AspNetUsers_UserId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_UserId",
                table: "Players");

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
    }
}
