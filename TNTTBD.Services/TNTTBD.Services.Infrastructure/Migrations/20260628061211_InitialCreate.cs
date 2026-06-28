using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TNTTBD.Services.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatechismYearEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Theme = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechismYearEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatechistEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChristName = table.Column<string>(type: "text", nullable: true),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    GivenName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    IsMale = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechistEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChristName = table.Column<string>(type: "text", nullable: true),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    GivenName = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EucharisticYouthEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    ChristName = table.Column<string>(type: "text", nullable: true),
                    FamilyName = table.Column<string>(type: "text", nullable: true),
                    MiddleName = table.Column<string>(type: "text", nullable: true),
                    GivenName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    IsMale = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EucharisticYouthEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EucharisticYouthRunningNumberEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    CurrentNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EucharisticYouthRunningNumberEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatechismClassEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClassName = table.Column<string>(type: "text", nullable: false),
                    ClassCode = table.Column<string>(type: "text", nullable: false),
                    CatechismYearId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechismClassEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatechismClassEntity_CatechismYearEntity_CatechismYearId",
                        column: x => x.CatechismYearId,
                        principalTable: "CatechismYearEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatechistPatronSaints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaintName = table.Column<string>(type: "text", nullable: false),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    CatechistId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechistPatronSaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatechistPatronSaints_CatechistEntity_CatechistId",
                        column: x => x.CatechistId,
                        principalTable: "CatechistEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatechistPhotoEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatechistId = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Purpose = table.Column<int>(type: "integer", nullable: false),
                    UploadedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UploadedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Visibility = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechistPhotoEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatechistPhotoEntity_CatechistEntity_CatechistId",
                        column: x => x.CatechistId,
                        principalTable: "CatechistEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EucharisticYouthContactEntity",
                columns: table => new
                {
                    EucharisticYouthId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactRelationType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EucharisticYouthContactEntity", x => new { x.ContactId, x.EucharisticYouthId });
                    table.ForeignKey(
                        name: "FK_EucharisticYouthContactEntity_ContactEntity_ContactId",
                        column: x => x.ContactId,
                        principalTable: "ContactEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EucharisticYouthContactEntity_EucharisticYouthEntity_Euchar~",
                        column: x => x.EucharisticYouthId,
                        principalTable: "EucharisticYouthEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EucharisticYouthPatronSaints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaintName = table.Column<string>(type: "text", nullable: false),
                    Day = table.Column<int>(type: "integer", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    EucharisticYouthId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EucharisticYouthPatronSaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EucharisticYouthPatronSaints_EucharisticYouthEntity_Euchari~",
                        column: x => x.EucharisticYouthId,
                        principalTable: "EucharisticYouthEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EucharisticYouthPhotoEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EucharisticYouthId = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageKey = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Purpose = table.Column<int>(type: "integer", nullable: false),
                    UploadedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UploadedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    Visibility = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EucharisticYouthPhotoEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EucharisticYouthPhotoEntity_EucharisticYouthEntity_Eucharis~",
                        column: x => x.EucharisticYouthId,
                        principalTable: "EucharisticYouthEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpiritualMilestoneEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EucharisticYouthId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    DocumentCode = table.Column<string>(type: "text", nullable: true),
                    Place = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpiritualMilestoneEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpiritualMilestoneEntity_EucharisticYouthEntity_Eucharistic~",
                        column: x => x.EucharisticYouthId,
                        principalTable: "EucharisticYouthEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CatechistId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: true),
                    EucharisticYouthId = table.Column<Guid>(type: "uuid", nullable: true),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEntity_CatechistEntity_CatechistId",
                        column: x => x.CatechistId,
                        principalTable: "CatechistEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEntity_ContactEntity_ContactId",
                        column: x => x.ContactId,
                        principalTable: "ContactEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEntity_EucharisticYouthEntity_EucharisticYouthId",
                        column: x => x.EucharisticYouthId,
                        principalTable: "EucharisticYouthEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CatechismClassCatechistEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatechismClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    CatechistId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    EnrolledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LeftAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechismClassCatechistEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatechismClassCatechistEntity_CatechismClassEntity_Catechis~",
                        column: x => x.CatechismClassId,
                        principalTable: "CatechismClassEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatechismClassCatechistEntity_CatechistEntity_CatechistId",
                        column: x => x.CatechistId,
                        principalTable: "CatechistEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatechismClassEucharisticYouthEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatechismClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    EucharisticYouthId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrolledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LeftAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechismClassEucharisticYouthEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatechismClassEucharisticYouthEntity_CatechismClassEntity_C~",
                        column: x => x.CatechismClassId,
                        principalTable: "CatechismClassEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CatechismClassEucharisticYouthEntity_EucharisticYouthEntity~",
                        column: x => x.EucharisticYouthId,
                        principalTable: "EucharisticYouthEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatechismSessionEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CatechismClassId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechismSessionEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatechismSessionEntity_CatechismClassEntity_CatechismClassId",
                        column: x => x.CatechismClassId,
                        principalTable: "CatechismClassEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CatechismClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamEntity_CatechismClassEntity_CatechismClassId",
                        column: x => x.CatechismClassId,
                        principalTable: "CatechismClassEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserExternalLoginEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    LinkedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExternalLoginEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExternalLoginEntity_UserEntity_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CatechismSessionAttendanceEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatechismSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttendanceStatus = table.Column<int>(type: "integer", nullable: false),
                    CatechismClassEucharisticYouthId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarkedByCatechistId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarkedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatechismSessionAttendanceEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatechismSessionAttendanceEntity_CatechismClassEucharisticY~",
                        column: x => x.CatechismClassEucharisticYouthId,
                        principalTable: "CatechismClassEucharisticYouthEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CatechismSessionAttendanceEntity_CatechismSessionEntity_Cat~",
                        column: x => x.CatechismSessionId,
                        principalTable: "CatechismSessionEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatechismSessionAttendanceEntity_CatechistEntity_MarkedByCa~",
                        column: x => x.MarkedByCatechistId,
                        principalTable: "CatechistEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamResultEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    CatechismClassEucharisticYouthId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    IsDone = table.Column<bool>(type: "boolean", nullable: false),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    MarkedByCatechistId = table.Column<Guid>(type: "uuid", nullable: false),
                    MarkedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamResultEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamResultEntity_CatechismClassEucharisticYouthEntity_Catec~",
                        column: x => x.CatechismClassEucharisticYouthId,
                        principalTable: "CatechismClassEucharisticYouthEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamResultEntity_CatechistEntity_MarkedByCatechistId",
                        column: x => x.MarkedByCatechistId,
                        principalTable: "CatechistEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamResultEntity_ExamEntity_ExamId",
                        column: x => x.ExamId,
                        principalTable: "ExamEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatechismClassCatechistEntity_CatechismClassId_CatechistId",
                table: "CatechismClassCatechistEntity",
                columns: new[] { "CatechismClassId", "CatechistId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatechismClassCatechistEntity_CatechistId",
                table: "CatechismClassCatechistEntity",
                column: "CatechistId");

            migrationBuilder.CreateIndex(
                name: "IX_CatechismClassEntity_CatechismYearId",
                table: "CatechismClassEntity",
                column: "CatechismYearId");

            migrationBuilder.CreateIndex(
                name: "IX_CatechismClassEucharisticYouthEntity_CatechismClassId_Eucha~",
                table: "CatechismClassEucharisticYouthEntity",
                columns: new[] { "CatechismClassId", "EucharisticYouthId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatechismClassEucharisticYouthEntity_EucharisticYouthId",
                table: "CatechismClassEucharisticYouthEntity",
                column: "EucharisticYouthId");

            migrationBuilder.CreateIndex(
                name: "IX_CatechismSessionAttendanceEntity_CatechismClassEucharisticY~",
                table: "CatechismSessionAttendanceEntity",
                column: "CatechismClassEucharisticYouthId");

            migrationBuilder.CreateIndex(
                name: "IX_CatechismSessionAttendanceEntity_CatechismSessionId_Catechi~",
                table: "CatechismSessionAttendanceEntity",
                columns: new[] { "CatechismSessionId", "CatechismClassEucharisticYouthId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatechismSessionAttendanceEntity_MarkedByCatechistId",
                table: "CatechismSessionAttendanceEntity",
                column: "MarkedByCatechistId");

            migrationBuilder.CreateIndex(
                name: "IX_CatechismSessionEntity_CatechismClassId",
                table: "CatechismSessionEntity",
                column: "CatechismClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CatechistPatronSaints_CatechistId",
                table: "CatechistPatronSaints",
                column: "CatechistId");

            migrationBuilder.CreateIndex(
                name: "IX_CatechistPhotoEntity_CatechistId",
                table: "CatechistPhotoEntity",
                column: "CatechistId");

            migrationBuilder.CreateIndex(
                name: "IX_EucharisticYouthContactEntity_ContactId_EucharisticYouthId",
                table: "EucharisticYouthContactEntity",
                columns: new[] { "ContactId", "EucharisticYouthId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EucharisticYouthContactEntity_EucharisticYouthId",
                table: "EucharisticYouthContactEntity",
                column: "EucharisticYouthId");

            migrationBuilder.CreateIndex(
                name: "IX_EucharisticYouthEntity_Code",
                table: "EucharisticYouthEntity",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EucharisticYouthPatronSaints_EucharisticYouthId",
                table: "EucharisticYouthPatronSaints",
                column: "EucharisticYouthId");

            migrationBuilder.CreateIndex(
                name: "IX_EucharisticYouthPhotoEntity_EucharisticYouthId",
                table: "EucharisticYouthPhotoEntity",
                column: "EucharisticYouthId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamEntity_CatechismClassId",
                table: "ExamEntity",
                column: "CatechismClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResultEntity_CatechismClassEucharisticYouthId",
                table: "ExamResultEntity",
                column: "CatechismClassEucharisticYouthId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResultEntity_ExamId_CatechismClassEucharisticYouthId",
                table: "ExamResultEntity",
                columns: new[] { "ExamId", "CatechismClassEucharisticYouthId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamResultEntity_MarkedByCatechistId",
                table: "ExamResultEntity",
                column: "MarkedByCatechistId");

            migrationBuilder.CreateIndex(
                name: "IX_SpiritualMilestoneEntity_EucharisticYouthId",
                table: "SpiritualMilestoneEntity",
                column: "EucharisticYouthId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_CatechistId",
                table: "UserEntity",
                column: "CatechistId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_ContactId",
                table: "UserEntity",
                column: "ContactId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_Email",
                table: "UserEntity",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_EucharisticYouthId",
                table: "UserEntity",
                column: "EucharisticYouthId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserEntity_UserName",
                table: "UserEntity",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserExternalLoginEntity_Provider_SubjectId",
                table: "UserExternalLoginEntity",
                columns: new[] { "Provider", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserExternalLoginEntity_UserId",
                table: "UserExternalLoginEntity",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatechismClassCatechistEntity");

            migrationBuilder.DropTable(
                name: "CatechismSessionAttendanceEntity");

            migrationBuilder.DropTable(
                name: "CatechistPatronSaints");

            migrationBuilder.DropTable(
                name: "CatechistPhotoEntity");

            migrationBuilder.DropTable(
                name: "EucharisticYouthContactEntity");

            migrationBuilder.DropTable(
                name: "EucharisticYouthPatronSaints");

            migrationBuilder.DropTable(
                name: "EucharisticYouthPhotoEntity");

            migrationBuilder.DropTable(
                name: "EucharisticYouthRunningNumberEntity");

            migrationBuilder.DropTable(
                name: "ExamResultEntity");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SpiritualMilestoneEntity");

            migrationBuilder.DropTable(
                name: "UserExternalLoginEntity");

            migrationBuilder.DropTable(
                name: "CatechismSessionEntity");

            migrationBuilder.DropTable(
                name: "CatechismClassEucharisticYouthEntity");

            migrationBuilder.DropTable(
                name: "ExamEntity");

            migrationBuilder.DropTable(
                name: "UserEntity");

            migrationBuilder.DropTable(
                name: "CatechismClassEntity");

            migrationBuilder.DropTable(
                name: "CatechistEntity");

            migrationBuilder.DropTable(
                name: "ContactEntity");

            migrationBuilder.DropTable(
                name: "EucharisticYouthEntity");

            migrationBuilder.DropTable(
                name: "CatechismYearEntity");
        }
    }
}
