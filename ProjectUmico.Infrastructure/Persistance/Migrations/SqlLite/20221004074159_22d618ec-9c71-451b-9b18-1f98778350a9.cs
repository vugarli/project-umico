using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectUmico.Infrastructure.Persistance.Migrations.SqlLite
{
    public partial class _22d618ec9c71451b9b181f98778350a9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductAttribute",
                columns: new[] { "Id", "AttributeType", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "ParentAttributeId", "Value" },
                values: new object[] { 1, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, "Color" });

            migrationBuilder.InsertData(
                table: "ProductAttribute",
                columns: new[] { "Id", "AttributeType", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "ParentAttributeId", "Value" },
                values: new object[] { 2, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, 1, "Red" });

            migrationBuilder.InsertData(
                table: "ProductAttribute",
                columns: new[] { "Id", "AttributeType", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "ParentAttributeId", "Value" },
                values: new object[] { 3, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, 1, "Green" });

            migrationBuilder.InsertData(
                table: "ProductAttribute",
                columns: new[] { "Id", "AttributeType", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "ParentAttributeId", "Value" },
                values: new object[] { 4, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, 1, "Blue" });

            migrationBuilder.InsertData(
                table: "ProductAttribute",
                columns: new[] { "Id", "AttributeType", "CreatedAt", "CreatedBy", "LastModified", "LastModifiedBy", "ParentAttributeId", "Value" },
                values: new object[] { 5, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, 1, "Pink" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductAttribute",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductAttribute",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductAttribute",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductAttribute",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductAttribute",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
