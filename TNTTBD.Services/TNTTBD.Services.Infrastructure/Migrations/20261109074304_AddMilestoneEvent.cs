using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TNTTBD.Services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMilestoneEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpiritualMilestoneEntity_EucharisticYouthId",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.AddColumn<string>(
                name: "Celebrant",
                table: "SpiritualMilestoneEntity",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MilestoneEventId",
                table: "SpiritualMilestoneEntity",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SponsorContactId",
                table: "SpiritualMilestoneEntity",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MilestoneEventEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Place = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Celebrant = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Note = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CodeTemplate = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StartIndex = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilestoneEventEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpiritualMilestoneEntity_EucharisticYouthId_Type",
                table: "SpiritualMilestoneEntity",
                columns: new[] { "EucharisticYouthId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_SpiritualMilestoneEntity_MilestoneEventId",
                table: "SpiritualMilestoneEntity",
                column: "MilestoneEventId");

            migrationBuilder.CreateIndex(
                name: "IX_SpiritualMilestoneEntity_SponsorContactId",
                table: "SpiritualMilestoneEntity",
                column: "SponsorContactId");

            migrationBuilder.CreateIndex(
                name: "IX_MilestoneEventEntity_Type_Date",
                table: "MilestoneEventEntity",
                columns: new[] { "Type", "Date" });

            migrationBuilder.AddForeignKey(
                name: "FK_SpiritualMilestoneEntity_ContactEntity_SponsorContactId",
                table: "SpiritualMilestoneEntity",
                column: "SponsorContactId",
                principalTable: "ContactEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SpiritualMilestoneEntity_MilestoneEventEntity_MilestoneEven~",
                table: "SpiritualMilestoneEntity",
                column: "MilestoneEventId",
                principalTable: "MilestoneEventEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpiritualMilestoneEntity_ContactEntity_SponsorContactId",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_SpiritualMilestoneEntity_MilestoneEventEntity_MilestoneEven~",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.DropTable(
                name: "MilestoneEventEntity");

            migrationBuilder.DropIndex(
                name: "IX_SpiritualMilestoneEntity_EucharisticYouthId_Type",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.DropIndex(
                name: "IX_SpiritualMilestoneEntity_MilestoneEventId",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.DropIndex(
                name: "IX_SpiritualMilestoneEntity_SponsorContactId",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.DropColumn(
                name: "Celebrant",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.DropColumn(
                name: "MilestoneEventId",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.DropColumn(
                name: "SponsorContactId",
                table: "SpiritualMilestoneEntity");

            migrationBuilder.CreateIndex(
                name: "IX_SpiritualMilestoneEntity_EucharisticYouthId",
                table: "SpiritualMilestoneEntity",
                column: "EucharisticYouthId");
        }
    }
}
