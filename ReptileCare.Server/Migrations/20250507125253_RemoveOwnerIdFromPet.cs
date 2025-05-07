using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReptileCare.Server.Migrations
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
                name: "FK_Reptiles_AspNetUsers_OwnerId",
                table: "Reptiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Reptiles_EnclosureProfiles_EnclosureProfileId",
                table: "Reptiles");

            migrationBuilder.DropIndex(
                name: "IX_Reptiles_OwnerId",
                table: "Reptiles");

            migrationBuilder.DropIndex(
                name: "IX_EnclosureProfiles_OwnerId",
                table: "EnclosureProfiles");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Reptiles");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Reptiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpeciesType",
                table: "Reptiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Reptiles_EnclosureProfiles_EnclosureProfileId",
                table: "Reptiles",
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
                name: "FK_Reptiles_AspNetUsers_AppUserId",
                table: "Reptiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Reptiles_EnclosureProfiles_EnclosureProfileId",
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
                name: "SpeciesType",
                table: "Reptiles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "EnclosureProfiles");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Reptiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reptiles_OwnerId",
                table: "Reptiles",
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
                name: "FK_Reptiles_AspNetUsers_OwnerId",
                table: "Reptiles",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reptiles_EnclosureProfiles_EnclosureProfileId",
                table: "Reptiles",
                column: "EnclosureProfileId",
                principalTable: "EnclosureProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
