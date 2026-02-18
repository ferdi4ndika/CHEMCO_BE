using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSkeletonAPI.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class Data_new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Coler",
                table: "Settings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coler",
                table: "Settings");
        }
    }
}
