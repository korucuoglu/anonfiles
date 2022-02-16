using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FileUpload.Data.Migrations
{
    public partial class Files_Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryFile");

            migrationBuilder.CreateTable(
                name: "Files_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileId = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Categories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Files_Categories_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_Categories_CategoryId",
                table: "Files_Categories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Categories_FileId",
                table: "Files_Categories",
                column: "FileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files_Categories");

            migrationBuilder.CreateTable(
                name: "CategoryFile",
                columns: table => new
                {
                    CategoriesId = table.Column<string>(type: "text", nullable: false),
                    FilesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryFile", x => new { x.CategoriesId, x.FilesId });
                    table.ForeignKey(
                        name: "FK_CategoryFile_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryFile_Files_FilesId",
                        column: x => x.FilesId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryFile_FilesId",
                table: "CategoryFile",
                column: "FilesId");
        }
    }
}
