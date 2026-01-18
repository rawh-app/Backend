using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAWH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DBSurvey2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.CreateTable(
                name: "PneumoniaSurveyRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChildName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    FeverDuration = table.Column<int>(type: "int", nullable: false),
                    FeverLevel = table.Column<int>(type: "int", nullable: false),
                    FeverResponse = table.Column<int>(type: "int", nullable: false),
                    CoughTime = table.Column<int>(type: "int", nullable: false),
                    CoughType = table.Column<int>(type: "int", nullable: false),
                    PhlegmStatus = table.Column<int>(type: "int", nullable: false),
                    CoughSeverity = table.Column<int>(type: "int", nullable: false),
                    HasAbnormalBreathingSound = table.Column<bool>(type: "bit", nullable: false),
                    BreathingEffort = table.Column<int>(type: "int", nullable: false),
                    FeedingAbility = table.Column<int>(type: "int", nullable: false),
                    HasChestIndrawing = table.Column<int>(type: "int", nullable: false),
                    HasNasalFlaring = table.Column<bool>(type: "bit", nullable: false),
                    HasCyanosis = table.Column<bool>(type: "bit", nullable: false),
                    FatigueStatus = table.Column<bool>(type: "bit", nullable: false),
                    AppetiteStatus = table.Column<int>(type: "int", nullable: false),
                    HasWeakCry = table.Column<bool>(type: "bit", nullable: false),
                    HasSevereRunnyNoseWithBreathingDifficulty = table.Column<bool>(type: "bit", nullable: false),
                    RecurrentChestIssues = table.Column<int>(type: "int", nullable: false),
                    HeartCondition = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PneumoniaSurveyRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PneumoniaSurveyRequest_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PneumoniaSurveyRequest_UserId",
                table: "PneumoniaSurveyRequest",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PneumoniaSurveyRequest");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });
        }
    }
}
