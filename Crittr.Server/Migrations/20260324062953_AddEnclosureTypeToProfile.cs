using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crittr.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddEnclosureTypeToProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Critters_EnclosureProfiles_EnclosureProfileId",
                table: "Critters");

            migrationBuilder.AddColumn<int>(
                name: "EnclosureType",
                table: "EnclosureProfiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Critters_EnclosureProfiles_EnclosureProfileId",
                table: "Critters",
                column: "EnclosureProfileId",
                principalTable: "EnclosureProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Critters_EnclosureProfiles_EnclosureProfileId",
                table: "Critters");

            migrationBuilder.DropColumn(
                name: "EnclosureType",
                table: "EnclosureProfiles");

            migrationBuilder.AddForeignKey(
                name: "FK_Critters_EnclosureProfiles_EnclosureProfileId",
                table: "Critters",
                column: "EnclosureProfileId",
                principalTable: "EnclosureProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
