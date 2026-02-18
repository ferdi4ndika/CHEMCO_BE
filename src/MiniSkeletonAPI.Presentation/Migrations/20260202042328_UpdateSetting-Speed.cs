using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSkeletonAPI.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSettingSpeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Speed",
                table: "DataCounts",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speed",
                table: "DataCounts");
        }
    }
}
