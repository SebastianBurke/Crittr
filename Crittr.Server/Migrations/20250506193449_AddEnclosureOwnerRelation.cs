using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crittr.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEnclosureOwnerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "EnclosureProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EnclosureProfiles_AspNetUsers_OwnerId",
                table: "EnclosureProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EnclosureProfiles_OwnerId",
                table: "EnclosureProfiles");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "EnclosureProfiles");
        }
    }
}
