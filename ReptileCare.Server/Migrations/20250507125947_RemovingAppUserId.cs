using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReptileCare.Server.Migrations
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
                name: "FK_Reptiles_AspNetUsers_AppUserId",
                table: "Reptiles");

            migrationBuilder.DropIndex(
                name: "IX_Reptiles_AppUserId",
                table: "Reptiles");

            migrationBuilder.DropIndex(
                name: "IX_EnclosureProfiles_AppUserId",
                table: "EnclosureProfiles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Reptiles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "EnclosureProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Reptiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "EnclosureProfiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reptiles_AppUserId",
                table: "Reptiles",
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
                name: "FK_Reptiles_AspNetUsers_AppUserId",
                table: "Reptiles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
