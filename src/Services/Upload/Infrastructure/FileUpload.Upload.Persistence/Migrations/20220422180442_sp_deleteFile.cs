using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileUpload.Upload.Persistence.Migrations
{
    public partial class sp_deleteFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"CREATE OR REPLACE PROCEDURE public.delete_file(IN fileid integer, IN userid integer) LANGUAGE plpgsql AS $procedure$ DECLARE usedspace int; BEGIN SELECT size INTO usedspace  FROM files WHERE files.id = fileid; DELETE FROM files WHERE files.id = fileid AND files.user_id = userid; UPDATE userinfo SET used_space=used_space-usedspace  WHERE id = userid; COMMIT; END; $procedure$;";

            migrationBuilder.Sql(sql);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "359f19ec-695b-4edc-9880-efab5750ed8a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3fe14b37-7334-4b0d-b959-2b3afef01d9c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "80cf4cd5-66a1-451b-8e8e-2a1e6d72354e", "AQAAAAEAACcQAAAAEGSpkc+6yI+j1pZKO7rKaPD3gfmkLUM3LCpJOHJZZLG1heW0Hs+WyLlnAEmnZ1AXlQ==", "0101f899-d32c-4bea-9342-9930f2ea3eda" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "69036ac5-8340-4779-9df3-b4f9608c3de3", "AQAAAAEAACcQAAAAEGq8DgD9tvX4rUwZkpwOqptnqjLzD6tTU1thyAeFFLI+6N2C2Jdo/z6S4KfEMk0Jyw==", "b32f691c-5f2a-4e01-8ce8-d80deab43a2e" });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 4, 41, 603, DateTimeKind.Utc).AddTicks(2685));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 4, 41, 603, DateTimeKind.Utc).AddTicks(2687));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 4, 41, 603, DateTimeKind.Utc).AddTicks(2688));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 4, 41, 603, DateTimeKind.Utc).AddTicks(2688));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 4, 41, 603, DateTimeKind.Utc).AddTicks(2689));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 4, 41, 603, DateTimeKind.Utc).AddTicks(2689));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 4, 41, 603, DateTimeKind.Utc).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 4, 41, 603, DateTimeKind.Utc).AddTicks(2652));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d36f773e-9457-4610-9153-66866704e09b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "18199f66-e26f-4878-93d7-dc4a6f7ccd3a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8a49aace-ab9e-4d24-89ee-7bb940325706", "AQAAAAEAACcQAAAAEPigycqL9PK6wH9w1MYT821yASkcEhRolAd2gpqKq2XMzuzChp5YXfSVzNz6uMIuDg==", "0d7aeb5d-9e33-46d7-95ef-b3e63cae2e9c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "af0554c2-3f5e-4c0a-b16b-7b9d0515f794", "AQAAAAEAACcQAAAAEBepzh5SObAu83Jn2UKtUWU8woddOiCrxjshfMP0KWSGIRucQusDP0+D8L0hJqbmxA==", "bf96bb53-d2fc-4391-9590-d24bea637c9c" });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 47, 685, DateTimeKind.Utc).AddTicks(1295));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 47, 685, DateTimeKind.Utc).AddTicks(1297));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 47, 685, DateTimeKind.Utc).AddTicks(1299));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 47, 685, DateTimeKind.Utc).AddTicks(1299));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 47, 685, DateTimeKind.Utc).AddTicks(1300));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 47, 685, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 47, 685, DateTimeKind.Utc).AddTicks(1223));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 47, 685, DateTimeKind.Utc).AddTicks(1231));
        }
    }
}
