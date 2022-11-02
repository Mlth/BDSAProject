using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitInsight.Entities.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReposityCommitData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    author = table.Column<string>(type: "TEXT", nullable: true),
                    dateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReposityCommitData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FrequencyDTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RepositoryCommitDataId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequencyDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrequencyDTO_ReposityCommitData_RepositoryCommitDataId",
                        column: x => x.RepositoryCommitDataId,
                        principalTable: "ReposityCommitData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FrequencyDTO_RepositoryCommitDataId",
                table: "FrequencyDTO",
                column: "RepositoryCommitDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FrequencyDTO");

            migrationBuilder.DropTable(
                name: "ReposityCommitData");
        }
    }
}
