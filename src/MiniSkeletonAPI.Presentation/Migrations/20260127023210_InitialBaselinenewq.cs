using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSkeletonAPI.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaselinenewq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "DataAndonDetails",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qty",
                table: "DataAndonDetails");
        }
    }
}
