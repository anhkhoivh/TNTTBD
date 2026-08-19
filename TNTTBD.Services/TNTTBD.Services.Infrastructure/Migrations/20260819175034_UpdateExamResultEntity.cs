using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TNTTBD.Services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExamResultEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "ExamResultEntity");

            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "ExamResultEntity",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Score",
                table: "ExamResultEntity",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "ExamResultEntity",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
