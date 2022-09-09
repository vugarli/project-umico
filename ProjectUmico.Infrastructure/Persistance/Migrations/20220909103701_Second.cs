using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectUmico.Infrastructure.Persistance.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserPersistance_UserPersistanceId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserPersistance");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserPersistanceId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPersistance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPersistance", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserPersistanceId",
                table: "AspNetUsers",
                column: "UserPersistanceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserPersistance_UserPersistanceId",
                table: "AspNetUsers",
                column: "UserPersistanceId",
                principalTable: "UserPersistance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
