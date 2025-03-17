using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class NewUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_SchoolYears_KhoaId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentStatuses_StatusId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StatusId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "KhoaId",
                table: "Students",
                newName: "StudentStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_KhoaId",
                table: "Students",
                newName: "IX_Students_StudentStatusId");

            migrationBuilder.AddColumn<int>(
                name: "SchoolYearId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolYearId",
                table: "Students",
                column: "SchoolYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_SchoolYears_SchoolYearId",
                table: "Students",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentStatuses_StudentStatusId",
                table: "Students",
                column: "StudentStatusId",
                principalTable: "StudentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_SchoolYears_SchoolYearId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentStatuses_StudentStatusId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_SchoolYearId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "SchoolYearId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "StudentStatusId",
                table: "Students",
                newName: "KhoaId");

            migrationBuilder.RenameIndex(
                name: "IX_Students_StudentStatusId",
                table: "Students",
                newName: "IX_Students_KhoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StatusId",
                table: "Students",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_SchoolYears_KhoaId",
                table: "Students",
                column: "KhoaId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentStatuses_StatusId",
                table: "Students",
                column: "StatusId",
                principalTable: "StudentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
