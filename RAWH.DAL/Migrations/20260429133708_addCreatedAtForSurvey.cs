using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAWH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addCreatedAtForSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PneumoniaSurveyRequest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PneumoniaSurveyRequest");
        }
    }
}
