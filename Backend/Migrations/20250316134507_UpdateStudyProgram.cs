using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStudyProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Programs_ProgramId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.RenameColumn(
                name: "ProgramId",
                table: "Students",
                newName: "StudyProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_ProgramId",
                table: "Students",
                newName: "IX_Students_StudyProgramId");

            migrationBuilder.CreateTable(
                name: "StudyPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPrograms", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "StudyPrograms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chính quy" },
                    { 2, "Chất lượng cao" },
                    { 3, "Việt Pháp" },
                    { 4, "Tiên tiến" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudyPrograms_StudyProgramId",
                table: "Students",
                column: "StudyProgramId",
                principalTable: "StudyPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudyPrograms_StudyProgramId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "StudyPrograms");

            migrationBuilder.RenameColumn(
                name: "StudyProgramId",
                table: "Students",
                newName: "ProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_StudyProgramId",
                table: "Students",
                newName: "IX_Students_ProgramId");

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
                table: "Programs",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Chính quy" },
                    { 2, "Chất lượng cao" },
                    { 3, "Việt Pháp" },
                    { 4, "Tiên tiến" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Programs_ProgramId",
                table: "Students",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
