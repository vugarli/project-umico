using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectUmico.Infrastructure.Persistance.Migrations
{
    public partial class _1e353b55064c4c52a70cc2a0d854a179 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c93d5e12-92ae-4579-a63e-e13da972f11a");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName", "UserPersistanceId", "UserType" },
                values: new object[] { "1a0b93ed-9bc1-4dd1-a77a-d34925e156f1", 0, "8689b9b0-8850-4146-9dce-4fd38d01ab36", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "s@s.com", false, false, null, null, null, "AQAAAAEAACcQAAAAECEMUH3nEuj7X3FMk8ITJfbd4cQFdoRRYXPDGwXoag4OUrVZ1tomwjqpfKXZSKxIGQ==", null, false, "bff9dfcc-c26e-4f44-95ee-1028e170f8c6", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "s@s.com", 0, "User" });

            migrationBuilder.UpdateData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "1a0b93ed-9bc1-4dd1-a77a-d34925e156f1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a0b93ed-9bc1-4dd1-a77a-d34925e156f1");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName", "UserPersistanceId", "UserType" },
                values: new object[] { "c93d5e12-92ae-4579-a63e-e13da972f11a", 0, "c60e2fda-d436-4993-add3-bfa77895c240", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "s@s.com", false, false, null, null, null, "AQAAAAEAACcQAAAAELfnCFO/Rsph5vOiMtxwWldoXn0JrROs5OD1cEQDG70TAA7xS96kjdC1cV55C5Lg3g==", null, false, "a1b9a77e-e421-480d-8fee-6f030554e18d", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "s@s.com", 0, "User" });

            migrationBuilder.UpdateData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "c93d5e12-92ae-4579-a63e-e13da972f11a");
        }
    }
}
