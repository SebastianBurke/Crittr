using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReptileCare.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCaregiverAccessToEnclosure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaregiverAccesses_Reptiles_ReptileId",
                table: "CaregiverAccesses");

            migrationBuilder.RenameColumn(
                name: "ReptileId",
                table: "CaregiverAccesses",
                newName: "EnclosureId");

            migrationBuilder.RenameIndex(
                name: "IX_CaregiverAccesses_ReptileId",
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
                newName: "ReptileId");

            migrationBuilder.RenameIndex(
                name: "IX_CaregiverAccesses_EnclosureId",
                table: "CaregiverAccesses",
                newName: "IX_CaregiverAccesses_ReptileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaregiverAccesses_Reptiles_ReptileId",
                table: "CaregiverAccesses",
                column: "ReptileId",
                principalTable: "Reptiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
