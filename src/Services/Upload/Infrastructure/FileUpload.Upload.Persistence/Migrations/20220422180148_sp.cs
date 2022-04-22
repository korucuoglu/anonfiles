using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileUpload.Upload.Persistence.Migrations
{
    public partial class sp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"CREATE OR REPLACE PROCEDURE public.add_file(IN filename text, IN filesize bigint, IN filekey text, IN fileuser_id integer, IN categoryids integer[] DEFAULT '{}'::integer[], INOUT file_id integer DEFAULT NULL::integer) LANGUAGE plpgsql AS $procedure$ DECLARE _categoryid int; BEGIN INSERT INTO files (file_name, size, file_key, user_id) values(filename, filesize, filekey, fileuser_id) RETURNING id INTO file_id; UPDATE userinfo SET used_space=used_space+filesize WHERE id = fileuser_id; FOR _categoryid IN SELECT unnest(categoryids) LOOP IF EXISTS (SELECT 1 FROM categories c WHERE c.id = _categoryid AND user_id = fileuser_id) THEN INSERT INTO filecategory (category_id, file_id) VALUES (_categoryid, file_id); ELSE CONTINUE; END IF; END LOOP; COMMIT; END; $procedure$ ;";

            migrationBuilder.Sql(sql);


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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "32bc2039-56dc-418a-959c-877f8aaf65f5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b0a6e693-f3e5-4666-ae5c-e3306bfd30c2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4fbb71bf-8f79-421f-83f7-5cc1ac21ad24", "AQAAAAEAACcQAAAAEMUbnvDYl5IgxXItfcTW+LxFBhTyyLYkkOP/UqBtyAubmdPFzxUkGwnVkxa4R0ST4A==", "3095adb9-6eff-4462-a724-271cf15268d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff04aa2e-b9d6-4c73-b4e1-2dcd9faa8963", "AQAAAAEAACcQAAAAEJGam2RiPWSbU7FtXx0wkDaY9PuoKAIubIQuo9NFYNv1nNC5XfhoyKSh58ihgGZvyQ==", "fdb540d3-5e19-414c-a9b4-cd86b479cec3" });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 22, 451, DateTimeKind.Utc).AddTicks(5729));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 22, 451, DateTimeKind.Utc).AddTicks(5734));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 22, 451, DateTimeKind.Utc).AddTicks(5735));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 22, 451, DateTimeKind.Utc).AddTicks(5735));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 22, 451, DateTimeKind.Utc).AddTicks(5738));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 22, 451, DateTimeKind.Utc).AddTicks(5738));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 1,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 22, 451, DateTimeKind.Utc).AddTicks(5690));

            migrationBuilder.UpdateData(
                table: "userinfo",
                keyColumn: "id",
                keyValue: 2,
                column: "created_date",
                value: new DateTime(2022, 4, 22, 18, 1, 22, 451, DateTimeKind.Utc).AddTicks(5693));
        }
    }
}
