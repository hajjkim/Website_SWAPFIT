using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWAPFIT.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaoCaos",
                columns: table => new
                {
                    MaBaoCao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiBaoCao = table.Column<int>(type: "int", nullable: false),
                    MaDoCu = table.Column<int>(type: "int", nullable: true),
                    LyDo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ChiTiet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaos", x => x.MaBaoCao);
                });

            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    MaDanhGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaDoCu = table.Column<int>(type: "int", nullable: false),
                    Diem = table.Column<int>(type: "int", nullable: true),
                    BinhLuan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.MaDanhGia);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucs",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucs", x => x.MaDanhMuc);
                });

            migrationBuilder.CreateTable(
                name: "GiaoDichs",
                columns: table => new
                {
                    MaGiaoDich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaYeuCau = table.Column<int>(type: "int", nullable: false),
                    NgayGiaoDich = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GiaTri = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaoDichs", x => x.MaGiaoDich);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhs",
                columns: table => new
                {
                    MaHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDoCu = table.Column<int>(type: "int", nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhs", x => x.MaHinhAnh);
                });

            migrationBuilder.CreateTable(
                name: "HoTros",
                columns: table => new
                {
                    MaHoTro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    ChuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoTros", x => x.MaHoTro);
                });

            migrationBuilder.CreateTable(
                name: "LichSuDangNhaps",
                columns: table => new
                {
                    MaLichSu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    ThoiGianDangNhap = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiaChiIP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuDangNhaps", x => x.MaLichSu);
                });

            migrationBuilder.CreateTable(
                name: "LuotTimKiems",
                columns: table => new
                {
                    MaTimKiem = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TuKhoa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoLanTim = table.Column<int>(type: "int", nullable: true),
                    LanCuoiTim = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuotTimKiems", x => x.MaTimKiem);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "NhatKyHeThongs",
                columns: table => new
                {
                    MaNhatKy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HanhDong = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThucHienBoi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhatKyHeThongs", x => x.MaNhatKy);
                });

            migrationBuilder.CreateTable(
                name: "PhanHoiHoTros",
                columns: table => new
                {
                    MaPhanHoi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoTro = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayPhanHoi = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanHoiHoTros", x => x.MaPhanHoi);
                });

            migrationBuilder.CreateTable(
                name: "TheLoaiTinTucs",
                columns: table => new
                {
                    MaTheLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTheLoai = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheLoaiTinTucs", x => x.MaTheLoai);
                });

            migrationBuilder.CreateTable(
                name: "ThongBaos",
                columns: table => new
                {
                    MaThongBao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LienKet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaXem = table.Column<bool>(type: "bit", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaos", x => x.MaThongBao);
                });

            migrationBuilder.CreateTable(
                name: "ThongKes",
                columns: table => new
                {
                    MaThongKe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TongNguoiDung = table.Column<int>(type: "int", nullable: true),
                    TongBaiViet = table.Column<int>(type: "int", nullable: true),
                    TongGiaoDich = table.Column<int>(type: "int", nullable: true),
                    NgayThongKe = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongKes", x => x.MaThongKe);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieus",
                columns: table => new
                {
                    MaThuongHieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieus", x => x.MaThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "TinTucs",
                columns: table => new
                {
                    MaTinTuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaTheLoai = table.Column<int>(type: "int", nullable: true),
                    NgayDang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KichThuoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoaiSanPham = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinTucs", x => x.MaTinTuc);
                });

            migrationBuilder.CreateTable(
                name: "YeuThichs",
                columns: table => new
                {
                    MaYeuThich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaDoCu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuThichs", x => x.MaYeuThich);
                });

            migrationBuilder.CreateTable(
                name: "BinhLuanTinTucs",
                columns: table => new
                {
                    MaBinhLuan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTinTuc = table.Column<int>(type: "int", nullable: false),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBinhLuan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinhLuanTinTucs", x => x.MaBinhLuan);
                    table.ForeignKey(
                        name: "FK_BinhLuanTinTucs_BinhLuanTinTucs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BinhLuanTinTucs",
                        principalColumn: "MaBinhLuan");
                    table.ForeignKey(
                        name: "FK_BinhLuanTinTucs_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiaChis",
                columns: table => new
                {
                    MaDiaChi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    Tinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Huyen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Xa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChiTiet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LaMacDinh = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaChis", x => x.MaDiaChi);
                    table.ForeignKey(
                        name: "FK_DiaChis_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    MaDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    TaiKhoan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaNguoiMua = table.Column<int>(type: "int", nullable: true),
                    MaNguoiBan = table.Column<int>(type: "int", nullable: true),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhuongThucGiaoHang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NguoiDungMaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NguoiDungMaNguoiDung1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.MaDonHang);
                    table.ForeignKey(
                        name: "FK_DonHangs_NguoiDungs_MaNguoiBan",
                        column: x => x.MaNguoiBan,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHangs_NguoiDungs_MaNguoiMua",
                        column: x => x.MaNguoiMua,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung",
                        column: x => x.NguoiDungMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_DonHangs_NguoiDungs_NguoiDungMaNguoiDung1",
                        column: x => x.NguoiDungMaNguoiDung1,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "GioHangs",
                columns: table => new
                {
                    MaGioHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangs", x => x.MaGioHang);
                    table.ForeignKey(
                        name: "FK_GioHangs_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TinNhans",
                columns: table => new
                {
                    MaTinNhan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiGui = table.Column<int>(type: "int", nullable: false),
                    MaNguoiNhan = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThoiGianGui = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DaDoc = table.Column<bool>(type: "bit", nullable: true),
                    NguoiGuiMaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    NguoiNhanMaNguoiDung = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhans", x => x.MaTinNhan);
                    table.ForeignKey(
                        name: "FK_TinNhans_NguoiDungs_MaNguoiGui",
                        column: x => x.MaNguoiGui,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_TinNhans_NguoiDungs_MaNguoiNhan",
                        column: x => x.MaNguoiNhan,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_TinNhans_NguoiDungs_NguoiGuiMaNguoiDung",
                        column: x => x.NguoiGuiMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_TinNhans_NguoiDungs_NguoiNhanMaNguoiDung",
                        column: x => x.NguoiNhanMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "YeuCaus",
                columns: table => new
                {
                    MaYeuCau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiGuiId = table.Column<int>(type: "int", nullable: false),
                    NguoiNhanId = table.Column<int>(type: "int", nullable: false),
                    MaDoCu = table.Column<int>(type: "int", nullable: true),
                    NoiDung = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuCaus", x => x.MaYeuCau);
                    table.ForeignKey(
                        name: "FK_YeuCaus_NguoiDungs_NguoiGuiId",
                        column: x => x.NguoiGuiId,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YeuCaus_NguoiDungs_NguoiNhanId",
                        column: x => x.NguoiNhanId,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoCus",
                columns: table => new
                {
                    MaDoCu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: true),
                    MaThuongHieu = table.Column<int>(type: "int", nullable: true),
                    TenSanPham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhTrang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhuongThucTraoDoi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LoaiBaiDang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiaChiId = table.Column<int>(type: "int", nullable: true),
                    GiaTriUocTinh = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    NgayDang = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AnhSanPham = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoCus", x => x.MaDoCu);
                    table.ForeignKey(
                        name: "FK_DoCus_DanhMucs_MaDanhMuc",
                        column: x => x.MaDanhMuc,
                        principalTable: "DanhMucs",
                        principalColumn: "MaDanhMuc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DoCus_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoCus_ThuongHieus_MaThuongHieu",
                        column: x => x.MaThuongHieu,
                        principalTable: "ThuongHieus",
                        principalColumn: "MaThuongHieu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaiViets",
                columns: table => new
                {
                    MaBaiViet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    NguoiDungMaNguoiDung = table.Column<int>(type: "int", nullable: true),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: true),
                    MaThuongHieu = table.Column<int>(type: "int", nullable: true),
                    MaDiaChi = table.Column<int>(type: "int", nullable: true),
                    TieuDe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiBaiDang = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GiaSanPham = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaiViets", x => x.MaBaiViet);
                    table.ForeignKey(
                        name: "FK_BaiViets_DanhMucs_MaDanhMuc",
                        column: x => x.MaDanhMuc,
                        principalTable: "DanhMucs",
                        principalColumn: "MaDanhMuc");
                    table.ForeignKey(
                        name: "FK_BaiViets_DiaChis_MaDiaChi",
                        column: x => x.MaDiaChi,
                        principalTable: "DiaChis",
                        principalColumn: "MaDiaChi");
                    table.ForeignKey(
                        name: "FK_BaiViets_NguoiDungs_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaiViets_NguoiDungs_NguoiDungMaNguoiDung",
                        column: x => x.NguoiDungMaNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "MaNguoiDung");
                    table.ForeignKey(
                        name: "FK_BaiViets_ThuongHieus_MaThuongHieu",
                        column: x => x.MaThuongHieu,
                        principalTable: "ThuongHieus",
                        principalColumn: "MaThuongHieu");
                });

            migrationBuilder.CreateTable(
                name: "AnhBaiViets",
                columns: table => new
                {
                    MaAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaBaiViet = table.Column<int>(type: "int", nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnhBaiViets", x => x.MaAnh);
                    table.ForeignKey(
                        name: "FK_AnhBaiViets_BaiViets_MaBaiViet",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViets",
                        principalColumn: "MaBaiViet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    MaChiTietDonHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<int>(type: "int", nullable: false),
                    MaBaiViet = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => x.MaChiTietDonHang);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_BaiViets_MaBaiViet",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViets",
                        principalColumn: "MaBaiViet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DonHangs",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietGioHangs",
                columns: table => new
                {
                    MaChiTiet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaGioHang = table.Column<int>(type: "int", nullable: false),
                    MaBaiViet = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietGioHangs", x => x.MaChiTiet);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_BaiViets_MaBaiViet",
                        column: x => x.MaBaiViet,
                        principalTable: "BaiViets",
                        principalColumn: "MaBaiViet",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChiTietGioHangs_GioHangs_MaGioHang",
                        column: x => x.MaGioHang,
                        principalTable: "GioHangs",
                        principalColumn: "MaGioHang",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnhBaiViets_MaBaiViet",
                table: "AnhBaiViets",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_BaiViets_MaDanhMuc",
                table: "BaiViets",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_BaiViets_MaDiaChi",
                table: "BaiViets",
                column: "MaDiaChi");

            migrationBuilder.CreateIndex(
                name: "IX_BaiViets_MaNguoiDung",
                table: "BaiViets",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_BaiViets_MaThuongHieu",
                table: "BaiViets",
                column: "MaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_BaiViets_NguoiDungMaNguoiDung",
                table: "BaiViets",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuanTinTucs_MaNguoiDung",
                table: "BinhLuanTinTucs",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuanTinTucs_ParentId",
                table: "BinhLuanTinTucs",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaBaiViet",
                table: "ChiTietDonHangs",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_MaDonHang",
                table: "ChiTietDonHangs",
                column: "MaDonHang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHangs_MaBaiViet",
                table: "ChiTietGioHangs",
                column: "MaBaiViet");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietGioHangs_MaGioHang",
                table: "ChiTietGioHangs",
                column: "MaGioHang");

            migrationBuilder.CreateIndex(
                name: "IX_DiaChis_MaNguoiDung",
                table: "DiaChis",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DoCus_MaDanhMuc",
                table: "DoCus",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_DoCus_MaNguoiDung",
                table: "DoCus",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DoCus_MaThuongHieu",
                table: "DoCus",
                column: "MaThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaNguoiBan",
                table: "DonHangs",
                column: "MaNguoiBan");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_MaNguoiMua",
                table: "DonHangs",
                column: "MaNguoiMua");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_NguoiDungMaNguoiDung1",
                table: "DonHangs",
                column: "NguoiDungMaNguoiDung1");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangs_MaNguoiDung",
                table: "GioHangs",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_MaNguoiGui",
                table: "TinNhans",
                column: "MaNguoiGui");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_MaNguoiNhan",
                table: "TinNhans",
                column: "MaNguoiNhan");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_NguoiGuiMaNguoiDung",
                table: "TinNhans",
                column: "NguoiGuiMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_NguoiNhanMaNguoiDung",
                table: "TinNhans",
                column: "NguoiNhanMaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCaus_NguoiGuiId",
                table: "YeuCaus",
                column: "NguoiGuiId");

            migrationBuilder.CreateIndex(
                name: "IX_YeuCaus_NguoiNhanId",
                table: "YeuCaus",
                column: "NguoiNhanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnhBaiViets");

            migrationBuilder.DropTable(
                name: "BaoCaos");

            migrationBuilder.DropTable(
                name: "BinhLuanTinTucs");

            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "ChiTietGioHangs");

            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropTable(
                name: "DoCus");

            migrationBuilder.DropTable(
                name: "GiaoDichs");

            migrationBuilder.DropTable(
                name: "HinhAnhs");

            migrationBuilder.DropTable(
                name: "HoTros");

            migrationBuilder.DropTable(
                name: "LichSuDangNhaps");

            migrationBuilder.DropTable(
                name: "LuotTimKiems");

            migrationBuilder.DropTable(
                name: "NhatKyHeThongs");

            migrationBuilder.DropTable(
                name: "PhanHoiHoTros");

            migrationBuilder.DropTable(
                name: "TheLoaiTinTucs");

            migrationBuilder.DropTable(
                name: "ThongBaos");

            migrationBuilder.DropTable(
                name: "ThongKes");

            migrationBuilder.DropTable(
                name: "TinNhans");

            migrationBuilder.DropTable(
                name: "TinTucs");

            migrationBuilder.DropTable(
                name: "YeuCaus");

            migrationBuilder.DropTable(
                name: "YeuThichs");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "BaiViets");

            migrationBuilder.DropTable(
                name: "GioHangs");

            migrationBuilder.DropTable(
                name: "DanhMucs");

            migrationBuilder.DropTable(
                name: "DiaChis");

            migrationBuilder.DropTable(
                name: "ThuongHieus");

            migrationBuilder.DropTable(
                name: "NguoiDungs");
        }
    }
}
