using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAWH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SurveyDB4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "PneumoniaSurveyRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "PneumoniaSurveyRequest");
        }
    }
}
