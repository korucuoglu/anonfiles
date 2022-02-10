using Microsoft.EntityFrameworkCore.Migrations;

namespace FileUpload.Data.Migrations
{
    public partial class ValidationFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Files",
                newName: "Extension");

            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ApplicationUserId",
                table: "Categories",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_ApplicationUserId",
                table: "Categories",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_ApplicationUserId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ApplicationUserId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Extension",
                table: "Files",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Categories",
                newName: "CategoryName");
        }
    }
}
