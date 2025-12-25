using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWAPFIT.Migrations
{
    /// <inheritdoc />
    public partial class voucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoaiThongBao",
                table: "ThongBaos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BaoCaoTaiKhoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiBaoCao = table.Column<int>(type: "int", nullable: false),
                    MaNguoiBiBaoCao = table.Column<int>(type: "int", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoTaChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoTaiKhoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaoCaoTaiKhoans_NguoiDungs_MaNguoiBaoCao",
                        column: x => x.MaNguoiBaoCao,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_BaoCaoTaiKhoans_NguoiDungs_MaNguoiBiBaoCao",
                        column: x => x.MaNguoiBiBaoCao,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "UuDais",
                columns: table => new
                {
                    MaUuDai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenUuDai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LoaiUuDai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    GiaTri = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AnhBia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GioiHanSoLuong = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UuDais", x => x.MaUuDai);
                });

            migrationBuilder.CreateTable(
                name: "UserVouchers",
                columns: table => new
                {
                    UserVoucherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VoucherId = table.Column<int>(type: "int", nullable: false),
                    DateClaimed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVouchers", x => x.UserVoucherId);
                    table.ForeignKey(
                        name: "FK_UserVouchers_NguoiDungs_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVouchers_UuDais_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "UuDais",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UuDaiSanPhams",
                columns: table => new
                {
                    MaUuDaiSanPham = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaUuDai = table.Column<int>(type: "int", nullable: false),
                    MaDoCu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UuDaiSanPhams", x => x.MaUuDaiSanPham);
                    table.ForeignKey(
                        name: "FK_UuDaiSanPhams_DoCus_MaDoCu",
                        column: x => x.MaDoCu,
                        principalTable: "DoCus",
                        principalColumn: "MaDoCu",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UuDaiSanPhams_UuDais_MaUuDai",
                        column: x => x.MaUuDai,
                        principalTable: "UuDais",
                        principalColumn: "MaUuDai",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTaiKhoans_MaNguoiBaoCao",
                table: "BaoCaoTaiKhoans",
                column: "MaNguoiBaoCao");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTaiKhoans_MaNguoiBiBaoCao",
                table: "BaoCaoTaiKhoans",
                column: "MaNguoiBiBaoCao");

            migrationBuilder.CreateIndex(
                name: "IX_UserVouchers_UserId",
                table: "UserVouchers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVouchers_VoucherId",
                table: "UserVouchers",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_UuDaiSanPhams_MaDoCu",
                table: "UuDaiSanPhams",
                column: "MaDoCu");

            migrationBuilder.CreateIndex(
                name: "IX_UuDaiSanPhams_MaUuDai",
                table: "UuDaiSanPhams",
                column: "MaUuDai");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoTaiKhoans");

            migrationBuilder.DropTable(
                name: "UserVouchers");

            migrationBuilder.DropTable(
                name: "UuDaiSanPhams");

            migrationBuilder.DropTable(
                name: "UuDais");

            migrationBuilder.DropColumn(
                name: "LoaiThongBao",
                table: "ThongBaos");
        }
    }
}
