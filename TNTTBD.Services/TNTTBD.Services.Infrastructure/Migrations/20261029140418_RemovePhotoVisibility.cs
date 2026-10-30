using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TNTTBD.Services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhotoVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "EucharisticYouthPhotoEntity");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "CatechistPhotoEntity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "EucharisticYouthPhotoEntity",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "CatechistPhotoEntity",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
