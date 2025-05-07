using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crittr.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOwnerIdFromPet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnclosureProfiles_AspNetUsers_OwnerId",
                table: "EnclosureProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Critters_AspNetUsers_OwnerId",
                table: "Critters");

            migrationBuilder.DropForeignKey(
                name: "FK_Critters_EnclosureProfiles_EnclosureProfileId",
                table: "Critters");

            migrationBuilder.DropIndex(
                name: "IX_Critters_OwnerId",
                table: "Critters");

            migrationBuilder.DropIndex(
                name: "IX_EnclosureProfiles_OwnerId",
                table: "EnclosureProfiles");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Critters");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Critters",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpeciesType",
                table: "Critters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Critters_EnclosureProfiles_EnclosureProfileId",
                table: "Critters",
                column: "EnclosureProfileId",
                principalTable: "EnclosureProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnclosureProfiles_AspNetUsers_AppUserId",
                table: "EnclosureProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Critters_AspNetUsers_AppUserId",
                table: "Critters");

            migrationBuilder.DropForeignKey(
                name: "FK_Critters_EnclosureProfiles_EnclosureProfileId",
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
                name: "SpeciesType",
                table: "Critters");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "EnclosureProfiles");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Critters",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Critters_OwnerId",
                table: "Critters",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_EnclosureProfiles_OwnerId",
                table: "EnclosureProfiles",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EnclosureProfiles_AspNetUsers_OwnerId",
                table: "EnclosureProfiles",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Critters_AspNetUsers_OwnerId",
                table: "Critters",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Critters_EnclosureProfiles_EnclosureProfileId",
                table: "Critters",
                column: "EnclosureProfileId",
                principalTable: "EnclosureProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
