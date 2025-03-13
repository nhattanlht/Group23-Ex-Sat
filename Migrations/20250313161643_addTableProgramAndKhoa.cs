using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class addTableProgramAndKhoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChuongTrinh",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "KhoaHoc",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "KhoaId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProgramId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
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

            migrationBuilder.InsertData(
                table: "Programs",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chính quy" },
                    { 2, "Chất lượng cao" },
                    { 3, "Việt Pháp" },
                    { 4, "Tiên tiến" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_KhoaId",
                table: "Students",
                column: "KhoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProgramId",
                table: "Students",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_CacKhoa_KhoaId",
                table: "Students",
                column: "KhoaId",
                principalTable: "CacKhoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Programs_ProgramId",
                table: "Students",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_CacKhoa_KhoaId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Programs_ProgramId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "CacKhoa");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Students_KhoaId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ProgramId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "KhoaId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "ChuongTrinh",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KhoaHoc",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
