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
                name: "RepoData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepoData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommitData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    repoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommitData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommitData_RepoData_repoId",
                        column: x => x.repoId,
                        principalTable: "RepoData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommitData_repoId",
                table: "CommitData",
                column: "repoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommitData");

            migrationBuilder.DropTable(
                name: "RepoData");
        }
    }
}
