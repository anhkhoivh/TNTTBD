using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TNTTBD.Services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEntity_CatechistEntity_CatechistId",
                table: "UserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEntity_ContactEntity_ContactId",
                table: "UserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEntity_EucharisticYouthEntity_EucharisticYouthId",
                table: "UserEntity");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntity_CatechistEntity_CatechistId",
                table: "UserEntity",
                column: "CatechistId",
                principalTable: "CatechistEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntity_ContactEntity_ContactId",
                table: "UserEntity",
                column: "ContactId",
                principalTable: "ContactEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntity_EucharisticYouthEntity_EucharisticYouthId",
                table: "UserEntity",
                column: "EucharisticYouthId",
                principalTable: "EucharisticYouthEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEntity_CatechistEntity_CatechistId",
                table: "UserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEntity_ContactEntity_ContactId",
                table: "UserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEntity_EucharisticYouthEntity_EucharisticYouthId",
                table: "UserEntity");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntity_CatechistEntity_CatechistId",
                table: "UserEntity",
                column: "CatechistId",
                principalTable: "CatechistEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntity_ContactEntity_ContactId",
                table: "UserEntity",
                column: "ContactId",
                principalTable: "ContactEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEntity_EucharisticYouthEntity_EucharisticYouthId",
                table: "UserEntity",
                column: "EucharisticYouthId",
                principalTable: "EucharisticYouthEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
