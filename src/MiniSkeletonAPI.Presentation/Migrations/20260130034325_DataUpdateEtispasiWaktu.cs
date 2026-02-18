using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSkeletonAPI.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class DataUpdateEtispasiWaktu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "HangerSpeed",
                table: "DataAndons",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StarProsess",
                table: "DataAndons",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "stopLine",
                table: "DataAndons",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HangerSpeed",
                table: "DataAndons");

            migrationBuilder.DropColumn(
                name: "StarProsess",
                table: "DataAndons");

            migrationBuilder.DropColumn(
                name: "stopLine",
                table: "DataAndons");
        }
    }
}
