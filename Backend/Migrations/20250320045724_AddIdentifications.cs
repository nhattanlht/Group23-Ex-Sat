using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdentificationId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Identifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentificationType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasChip = table.Column<bool>(type: "bit", nullable: true),
                    IssuingCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_IdentificationId",
                table: "Students",
                column: "IdentificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Identifications_IdentificationId",
                table: "Students",
                column: "IdentificationId",
                principalTable: "Identifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Identifications_IdentificationId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Identifications");

            migrationBuilder.DropIndex(
                name: "IX_Students_IdentificationId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IdentificationId",
                table: "Students");
        }
    }
}
