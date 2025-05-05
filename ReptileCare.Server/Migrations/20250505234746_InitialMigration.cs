using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReptileCare.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnclosureProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<double>(type: "REAL", nullable: false),
                    Height = table.Column<double>(type: "REAL", nullable: false),
                    SubstrateType = table.Column<string>(type: "TEXT", nullable: true),
                    HasUVBLighting = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasHeatingElement = table.Column<bool>(type: "INTEGER", nullable: false),
                    IdealTemperature = table.Column<double>(type: "REAL", nullable: false),
                    IdealHumidity = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnclosureProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reptiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Species = table.Column<string>(type: "TEXT", nullable: false),
                    DateAcquired = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Sex = table.Column<string>(type: "TEXT", nullable: true),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    EnclosureProfileId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reptiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reptiles_EnclosureProfiles_EnclosureProfileId",
                        column: x => x.EnclosureProfileId,
                        principalTable: "EnclosureProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BehaviorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReptileId = table.Column<int>(type: "INTEGER", nullable: false),
                    LogDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BehaviorType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsAbnormal = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BehaviorLogs_Reptiles_ReptileId",
                        column: x => x.ReptileId,
                        principalTable: "Reptiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaregiverAccesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    ReptileId = table.Column<int>(type: "INTEGER", nullable: false),
                    CanEdit = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanDelete = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessGranted = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaregiverAccesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaregiverAccesses_Reptiles_ReptileId",
                        column: x => x.ReptileId,
                        principalTable: "Reptiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentalReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReptileId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReadingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Temperature = table.Column<double>(type: "REAL", nullable: false),
                    Humidity = table.Column<double>(type: "REAL", nullable: false),
                    UVBIndex = table.Column<double>(type: "REAL", nullable: true),
                    IsManualReading = table.Column<bool>(type: "INTEGER", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentalReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnvironmentalReadings_Reptiles_ReptileId",
                        column: x => x.ReptileId,
                        principalTable: "Reptiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedingRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReptileId = table.Column<int>(type: "INTEGER", nullable: false),
                    FeedingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FoodItem = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<double>(type: "REAL", nullable: false),
                    ItemType = table.Column<int>(type: "INTEGER", nullable: false),
                    WasEaten = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedingRecords_Reptiles_ReptileId",
                        column: x => x.ReptileId,
                        principalTable: "Reptiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReptileId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssessmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    AssessedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthScores_Reptiles_ReptileId",
                        column: x => x.ReptileId,
                        principalTable: "Reptiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MeasurementRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReptileId = table.Column<int>(type: "INTEGER", nullable: false),
                    MeasurementDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Length = table.Column<double>(type: "REAL", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeasurementRecords_Reptiles_ReptileId",
                        column: x => x.ReptileId,
                        principalTable: "Reptiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReptileId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledTasks_Reptiles_ReptileId",
                        column: x => x.ReptileId,
                        principalTable: "Reptiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SheddingRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReptileId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsComplete = table.Column<bool>(type: "INTEGER", nullable: false),
                    WasAssisted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheddingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SheddingRecords_Reptiles_ReptileId",
                        column: x => x.ReptileId,
                        principalTable: "Reptiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EnclosureProfiles",
                columns: new[] { "Id", "HasHeatingElement", "HasUVBLighting", "Height", "IdealHumidity", "IdealTemperature", "Length", "Name", "SubstrateType", "Width" },
                values: new object[,]
                {
                    { 1, true, true, 60.0, 30.0, 32.0, 120.0, "Desert Terrarium", "Sand/Clay mix", 60.0 },
                    { 2, true, true, 90.0, 70.0, 28.0, 90.0, "Tropical Vivarium", "Coconut Fiber", 45.0 }
                });

            migrationBuilder.InsertData(
                table: "Reptiles",
                columns: new[] { "Id", "DateAcquired", "DateOfBirth", "Description", "EnclosureProfileId", "Length", "Name", "Sex", "Species", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Friendly beardie with orange coloration", 1, 45.0, "Spike", "Male", "Bearded Dragon", 450.0 },
                    { 2, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Normal morph ball python, very docile", 2, 120.0, "Monty", "Male", "Ball Python", 1500.0 }
                });

            migrationBuilder.InsertData(
                table: "EnvironmentalReadings",
                columns: new[] { "Id", "Humidity", "IsManualReading", "ReadingDate", "ReptileId", "Source", "Temperature", "UVBIndex" },
                values: new object[,]
                {
                    { 1, 35.0, true, new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manual", 33.5, 6.7000000000000002 },
                    { 2, 65.0, true, new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Manual", 28.0, null }
                });

            migrationBuilder.InsertData(
                table: "FeedingRecords",
                columns: new[] { "Id", "FeedingDate", "FoodItem", "ItemType", "Notes", "Quantity", "ReptileId", "WasEaten" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Crickets", 0, null, 12.0, 1, true },
                    { 2, new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mealworms", 0, null, 15.0, 1, true },
                    { 3, new DateTime(2025, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Small Rat", 1, null, 1.0, 2, true }
                });

            migrationBuilder.InsertData(
                table: "HealthScores",
                columns: new[] { "Id", "AssessedBy", "AssessmentDate", "Notes", "ReptileId", "Score" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Healthy and active.", 1, 9 },
                    { 2, null, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Slight respiratory noise observed.", 2, 8 }
                });

            migrationBuilder.InsertData(
                table: "MeasurementRecords",
                columns: new[] { "Id", "Length", "MeasurementDate", "Notes", "ReptileId", "Weight" },
                values: new object[,]
                {
                    { 1, 46.0, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gained some weight.", 1, 460.0 },
                    { 2, 121.0, new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Normal growth.", 2, 1520.0 }
                });

            migrationBuilder.InsertData(
                table: "ScheduledTasks",
                columns: new[] { "Id", "CompletedDate", "Description", "DueDate", "IsCompleted", "Priority", "ReptileId", "Title" },
                values: new object[,]
                {
                    { 1, null, "Full substrate change and decoration cleaning", new DateTime(2025, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, 1, "Clean terrarium" },
                    { 2, null, "Replace the UVB bulb which is nearing end of its effective lifespan", new DateTime(2025, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, 2, "UVB Bulb Replacement" }
                });

            migrationBuilder.InsertData(
                table: "SheddingRecords",
                columns: new[] { "Id", "CompletionDate", "IsComplete", "Notes", "ReptileId", "StartDate", "WasAssisted" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Normal shed.", 1, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false },
                    { 2, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Helped with tail shedding.", 2, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BehaviorLogs_ReptileId",
                table: "BehaviorLogs",
                column: "ReptileId");

            migrationBuilder.CreateIndex(
                name: "IX_CaregiverAccesses_ReptileId",
                table: "CaregiverAccesses",
                column: "ReptileId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvironmentalReadings_ReptileId",
                table: "EnvironmentalReadings",
                column: "ReptileId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedingRecords_ReptileId",
                table: "FeedingRecords",
                column: "ReptileId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthScores_ReptileId",
                table: "HealthScores",
                column: "ReptileId");

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementRecords_ReptileId",
                table: "MeasurementRecords",
                column: "ReptileId");

            migrationBuilder.CreateIndex(
                name: "IX_Reptiles_EnclosureProfileId",
                table: "Reptiles",
                column: "EnclosureProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTasks_ReptileId",
                table: "ScheduledTasks",
                column: "ReptileId");

            migrationBuilder.CreateIndex(
                name: "IX_SheddingRecords_ReptileId",
                table: "SheddingRecords",
                column: "ReptileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BehaviorLogs");

            migrationBuilder.DropTable(
                name: "CaregiverAccesses");

            migrationBuilder.DropTable(
                name: "EnvironmentalReadings");

            migrationBuilder.DropTable(
                name: "FeedingRecords");

            migrationBuilder.DropTable(
                name: "HealthScores");

            migrationBuilder.DropTable(
                name: "MeasurementRecords");

            migrationBuilder.DropTable(
                name: "ScheduledTasks");

            migrationBuilder.DropTable(
                name: "SheddingRecords");

            migrationBuilder.DropTable(
                name: "Reptiles");

            migrationBuilder.DropTable(
                name: "EnclosureProfiles");
        }
    }
}
