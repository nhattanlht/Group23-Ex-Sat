using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class fixMSSV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Grades",
                newName: "MSSV");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                newName: "IX_Grades_MSSV");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_MSSV",
                table: "Grades",
                column: "MSSV",
                principalTable: "Students",
                principalColumn: "MSSV",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_MSSV",
                table: "Grades");

            migrationBuilder.RenameColumn(
                name: "MSSV",
                table: "Grades",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_MSSV",
                table: "Grades",
                newName: "IX_Grades_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "MSSV",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
