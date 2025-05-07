using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crittr.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovingAppUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnclosureProfiles_AspNetUsers_AppUserId",
                table: "EnclosureProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Critters_AspNetUsers_AppUserId",
                table: "Critters");

            migrationBuilder.DropIndex(
                name: "IX_Critters_AppUserId",
                table: "Critters");

            migrationBuilder.DropIndex(
                name: "IX_EnclosureProfiles_AppUserId",
                table: "EnclosureProfiles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Critters");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "EnclosureProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Critters",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "EnclosureProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Critters_AppUserId",
                table: "Critters",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EnclosureProfiles_AppUserId",
                table: "EnclosureProfiles",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnclosureProfiles_AspNetUsers_AppUserId",
                table: "EnclosureProfiles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Critters_AspNetUsers_AppUserId",
                table: "Critters",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
