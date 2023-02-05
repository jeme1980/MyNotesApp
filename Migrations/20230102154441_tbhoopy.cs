using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyNotesApp.Migrations
{
    public partial class tbhoopy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hoppies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hoppies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HoppyUsers",
                columns: table => new
                {
                    HoppyId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoppyUsers", x => new { x.AppUserId, x.HoppyId });
                    table.ForeignKey(
                        name: "FK_HoppyUsers_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoppyUsers_Hoppies_HoppyId",
                        column: x => x.HoppyId,
                        principalTable: "Hoppies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoppyUsers_HoppyId",
                table: "HoppyUsers",
                column: "HoppyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoppyUsers");

            migrationBuilder.DropTable(
                name: "Hoppies");
        }
    }
}
