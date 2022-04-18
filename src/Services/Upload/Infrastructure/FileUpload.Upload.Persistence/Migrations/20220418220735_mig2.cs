using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileUpload.Upload.Persistence.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userinfo_AspNetUsers_user_id",
                table: "userinfo");

            migrationBuilder.DropIndex(
                name: "IX_userinfo_user_id",
                table: "userinfo");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "userinfo");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "829ea532-61f5-4228-ac74-6f0fc7c38e9d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "693f68bb-920b-4d56-afba-a66effceec7c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9c80cec6-8c94-478c-84df-f3e69dec961b", "AQAAAAEAACcQAAAAEAtsoGNyzgHE/Q+//B2ocnZBOw3K3P/AdaECxg9N2SOAOiYEXs+gbilEC8dyW0K8gQ==", "ff6b1d46-33e4-468b-878b-36d6bce0c1f4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78ba930d-c9ed-4bb5-aa64-e1a060bb7dc9", "AQAAAAEAACcQAAAAEM6GVAXFPvCvP3EHa0NmPAu48YAXxdfrXIlxTYkDdmpVUSXdtYnSpOFGfTYzOYT94w==", "e049d158-17b9-48a1-8507-8a44b44b72c1" });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 22, 7, 35, 149, DateTimeKind.Utc).AddTicks(5997));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 22, 7, 35, 149, DateTimeKind.Utc).AddTicks(5998));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 22, 7, 35, 149, DateTimeKind.Utc).AddTicks(5999));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 22, 7, 35, 149, DateTimeKind.Utc).AddTicks(6000));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 22, 7, 35, 149, DateTimeKind.Utc).AddTicks(6001));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 22, 7, 35, 149, DateTimeKind.Utc).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 22, 7, 35, 149, DateTimeKind.Utc).AddTicks(5956));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 22, 7, 35, 149, DateTimeKind.Utc).AddTicks(5959));

            migrationBuilder.AddForeignKey(
                name: "FK_userinfo_AspNetUsers_id",
                table: "userinfo",
                column: "id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userinfo_AspNetUsers_id",
                table: "userinfo");

            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "userinfo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "533a3048-fdf2-41e9-bedf-9911f1c3bd5a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "2e7b559f-b2ad-4a05-bd61-acd19d3380f3");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6e73ce00-d9d8-4586-94c8-17de34d65fad", "AQAAAAEAACcQAAAAEGbao2uALrbyAJH4aiDRVTnXVez4750W/6dAgM8kQ6/CMD1j0HYNTI1yhOqEgSujhw==", "fc846c57-a2fe-4a04-be76-cc2f8a094bec" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8ef2822-9f6b-4d33-a19f-94dd5de5c727", "AQAAAAEAACcQAAAAEAwtoeUgNhqfCcd3+C6yzQFHAAq0WvRZYFB7b3DvZLOVD/ypEeBSkOguYYJ4cA+ZNg==", "7c8339f6-ad2d-49fd-bb14-f51f17558dd3" });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 21, 55, 42, 46, DateTimeKind.Utc).AddTicks(7825));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 21, 55, 42, 46, DateTimeKind.Utc).AddTicks(7826));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 21, 55, 42, 46, DateTimeKind.Utc).AddTicks(7827));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 21, 55, 42, 46, DateTimeKind.Utc).AddTicks(7827));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 21, 55, 42, 46, DateTimeKind.Utc).AddTicks(7828));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6,
                column: "created_date",
                value: new DateTime(2022, 4, 18, 21, 55, 42, 46, DateTimeKind.Utc).AddTicks(7828));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_date", "user_id" },
                values: new object[] { new DateTime(2022, 4, 18, 21, 55, 42, 46, DateTimeKind.Utc).AddTicks(7784), 1 });

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_date", "user_id" },
                values: new object[] { new DateTime(2022, 4, 18, 21, 55, 42, 46, DateTimeKind.Utc).AddTicks(7787), 2 });

            migrationBuilder.CreateIndex(
                name: "IX_userinfo_user_id",
                table: "userinfo",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_userinfo_AspNetUsers_user_id",
                table: "userinfo",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
