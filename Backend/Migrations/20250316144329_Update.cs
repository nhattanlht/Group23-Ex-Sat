using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_CacKhoa_KhoaId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "CacKhoa");

            migrationBuilder.CreateTable(
                name: "SchoolYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolYears", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SchoolYears",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "18" },
                    { 2, "19" },
                    { 3, "20" },
                    { 4, "21" },
                    { 5, "22" },
                    { 6, "23" },
                    { 7, "24" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_SchoolYears_KhoaId",
                table: "Students",
                column: "KhoaId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_SchoolYears_KhoaId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "SchoolYears");

            migrationBuilder.CreateTable(
                name: "CacKhoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacKhoa", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CacKhoa",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "18" },
                    { 2, "19" },
                    { 3, "20" },
                    { 4, "21" },
                    { 5, "22" },
                    { 6, "23" },
                    { 7, "24" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_CacKhoa_KhoaId",
                table: "Students",
                column: "KhoaId",
                principalTable: "CacKhoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
