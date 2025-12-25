using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWAPFIT.Migrations
{
    /// <inheritdoc />
    public partial class AddTaiKhoanNganHang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung",
                table: "DonHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung1",
                table: "DonHangs");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung",
                table: "DonHangs");

            migrationBuilder.DropIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung1",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "DonHangs");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung1",
                table: "DonHangs");

            migrationBuilder.CreateTable(
                name: "TaiKhoanNganHang",
                columns: table => new
                {
                    MaTaiKhoanNganHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    TenNganHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChuTaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeImage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoanNganHang", x => x.MaTaiKhoanNganHang);
                    table.ForeignKey(
                        name: "FK_TaiKhoanNganHang_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaiKhoanNganHang_MaNguoiDung",
                table: "TaiKhoanNganHang",
                column: "MaNguoiDung",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaiKhoanNganHang");

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "DonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung1",
                table: "DonHangs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung1",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung1");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung1",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung1",
                principalTable: "NguoiDungs",
                principalColumn: "MaNguoiDung");
        }
    }
}
