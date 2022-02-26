using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FileUpload.Api.Persistence.Migrations
{
    public partial class FileCategoryJoinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Files_Categories",
                table: "Files_Categories");

            migrationBuilder.DropIndex(
                name: "IX_Files_Categories_CategoryId",
                table: "Files_Categories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Files_Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files_Categories",
                table: "Files_Categories",
                columns: new[] { "CategoryId", "FileId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Files_Categories",
                table: "Files_Categories");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Files_Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files_Categories",
                table: "Files_Categories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Categories_CategoryId",
                table: "Files_Categories",
                column: "CategoryId");
        }
    }
}
