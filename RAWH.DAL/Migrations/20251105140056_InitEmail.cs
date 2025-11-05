using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAWH.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "resetPasswordEmail",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resetPasswordEmail",
                table: "AspNetUsers");
        }
    }
}
