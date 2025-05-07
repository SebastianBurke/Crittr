using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crittr.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCaregiverAccessToEnclosure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverAccesses_Critters_CritterId",
                table: "CaregiverAccesses");

            migrationBuilder.RenameColumn(
                name: "CritterId",
                table: "CaregiverAccesses",
                newName: "EnclosureId");

            migrationBuilder.RenameIndex(
                name: "IX_CaregiverAccesses_CritterId",
                table: "CaregiverAccesses",
                newName: "IX_CaregiverAccesses_EnclosureId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverAccesses_EnclosureProfiles_EnclosureId",
                table: "CaregiverAccesses",
                column: "EnclosureId",
                principalTable: "EnclosureProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverAccesses_EnclosureProfiles_EnclosureId",
                table: "CaregiverAccesses");

            migrationBuilder.RenameColumn(
                name: "EnclosureId",
                table: "CaregiverAccesses",
                newName: "CritterId");

            migrationBuilder.RenameIndex(
                name: "IX_CaregiverAccesses_EnclosureId",
                table: "CaregiverAccesses",
                newName: "IX_CaregiverAccesses_CritterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverAccesses_Critters_CritterId",
                table: "CaregiverAccesses",
                column: "CritterId",
                principalTable: "Critters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
