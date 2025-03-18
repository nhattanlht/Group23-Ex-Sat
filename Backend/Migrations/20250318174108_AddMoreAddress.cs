using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiaChiNhanThuId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiaChiTamTruId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiaChiThuongTruId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuocTich",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HouseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_DiaChiNhanThuId",
                table: "Students",
                column: "DiaChiNhanThuId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DiaChiTamTruId",
                table: "Students",
                column: "DiaChiTamTruId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DiaChiThuongTruId",
                table: "Students",
                column: "DiaChiThuongTruId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Addresses_DiaChiNhanThuId",
                table: "Students",
                column: "DiaChiNhanThuId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Addresses_DiaChiTamTruId",
                table: "Students",
                column: "DiaChiTamTruId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Addresses_DiaChiThuongTruId",
                table: "Students",
                column: "DiaChiThuongTruId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Addresses_DiaChiNhanThuId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Addresses_DiaChiTamTruId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Addresses_DiaChiThuongTruId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Students_DiaChiNhanThuId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_DiaChiTamTruId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_DiaChiThuongTruId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DiaChiNhanThuId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DiaChiTamTruId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DiaChiThuongTruId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "QuocTich",
                table: "Students");
        }
    }
}
