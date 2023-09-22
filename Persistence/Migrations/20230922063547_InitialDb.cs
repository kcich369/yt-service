using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YtChannels",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    YtId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedById = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YtChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YtVideos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    YtId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    Process = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCulture = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ChannelId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YtVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YtVideos_YtChannels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "YtChannels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YtVideoFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Path_MainPath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Path_FileName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Path_FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PathData_FullValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedById = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Retries = table.Column<int>(type: "int", nullable: false),
                    Bytes = table.Column<long>(type: "bigint", nullable: false),
                    Process = table.Column<bool>(type: "bit", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YtVideoFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YtVideoFiles_YtVideos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "YtVideos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YtVideoFileWavs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Path_MainPath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Path_FileName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Path_FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PathData_FullValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Process = table.Column<bool>(type: "bit", nullable: false),
                    YtVideoFileId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YtVideoFileWavs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YtVideoFileWavs_YtVideoFiles_YtVideoFileId",
                        column: x => x.YtVideoFileId,
                        principalTable: "YtVideoFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YtVideoTranscriptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Path_MainPath = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Path_FileName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Path_FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PathData_FullValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Process = table.Column<bool>(type: "bit", nullable: false),
                    WavFileId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YtVideoTranscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YtVideoTranscriptions_YtVideoFileWavs_WavFileId",
                        column: x => x.WavFileId,
                        principalTable: "YtVideoFileWavs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YtVideoDescriptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Process = table.Column<bool>(type: "bit", nullable: false),
                    YtVideoTranscriptionId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    YtVideoId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YtVideoDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YtVideoDescriptions_YtVideoTranscriptions_YtVideoTranscriptionId",
                        column: x => x.YtVideoTranscriptionId,
                        principalTable: "YtVideoTranscriptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_YtVideoDescriptions_YtVideos_YtVideoId",
                        column: x => x.YtVideoId,
                        principalTable: "YtVideos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YtVideoTags",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(26)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Process = table.Column<bool>(type: "bit", nullable: false),
                    YtVideoTranscriptionId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    YtVideoId = table.Column<string>(type: "nvarchar(26)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YtVideoTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YtVideoTags_YtVideoTranscriptions_YtVideoTranscriptionId",
                        column: x => x.YtVideoTranscriptionId,
                        principalTable: "YtVideoTranscriptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_YtVideoTags_YtVideos_YtVideoId",
                        column: x => x.YtVideoId,
                        principalTable: "YtVideos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_YtChannels_Name",
                table: "YtChannels",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideoDescriptions_YtVideoId",
                table: "YtVideoDescriptions",
                column: "YtVideoId",
                unique: true,
                filter: "[YtVideoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideoDescriptions_YtVideoTranscriptionId",
                table: "YtVideoDescriptions",
                column: "YtVideoTranscriptionId",
                unique: true,
                filter: "[YtVideoTranscriptionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideoFiles_VideoId",
                table: "YtVideoFiles",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideoFileWavs_YtVideoFileId",
                table: "YtVideoFileWavs",
                column: "YtVideoFileId",
                unique: true,
                filter: "[YtVideoFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideos_ChannelId",
                table: "YtVideos",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideos_Name",
                table: "YtVideos",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideoTags_YtVideoId",
                table: "YtVideoTags",
                column: "YtVideoId",
                unique: true,
                filter: "[YtVideoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideoTags_YtVideoTranscriptionId",
                table: "YtVideoTags",
                column: "YtVideoTranscriptionId",
                unique: true,
                filter: "[YtVideoTranscriptionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_YtVideoTranscriptions_WavFileId",
                table: "YtVideoTranscriptions",
                column: "WavFileId",
                unique: true,
                filter: "[WavFileId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YtVideoDescriptions");

            migrationBuilder.DropTable(
                name: "YtVideoTags");

            migrationBuilder.DropTable(
                name: "YtVideoTranscriptions");

            migrationBuilder.DropTable(
                name: "YtVideoFileWavs");

            migrationBuilder.DropTable(
                name: "YtVideoFiles");

            migrationBuilder.DropTable(
                name: "YtVideos");

            migrationBuilder.DropTable(
                name: "YtChannels");
        }
    }
}
