using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWAPFIT.Migrations
{
    /// <inheritdoc />
    public partial class AddThongBaoNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaiViets_NguoiDungs_NguoiDungMaNguoiDung",
                table: "BaiViets");

            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoans_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoans_BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropIndex(
                name: "IX_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropIndex(
                name: "IX_BaiViets_NguoiDungMaNguoiDung",
                table: "BaiViets");

            migrationBuilder.DropColumn(
                name: "BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropColumn(
                name: "NguoiDungMaNguoiDung",
                table: "BaiViets");

            migrationBuilder.RenameColumn(
                name: "LoaiThongBao",
                table: "ThongBaos",
                newName: "Loai");

            migrationBuilder.RenameColumn(
                name: "MaThongBao",
                table: "ThongBaos",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "NoiDung",
                table: "ThongBaos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThamChieuId",
                table: "ThongBaos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ThongKeDanhMucTongHops",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TongSoBai = table.Column<int>(type: "int", nullable: false),
                    TongSoLuotMua = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongKeDanhMucTongHops", x => x.MaDanhMuc);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoans_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnhs",
                column: "BaoCaoTaiKhoanId",
                principalTable: "BaoCaoTaiKhoans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoans_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropTable(
                name: "ThongKeDanhMucTongHops");

            migrationBuilder.DropColumn(
                name: "ThamChieuId",
                table: "ThongBaos");

            migrationBuilder.RenameColumn(
                name: "Loai",
                table: "ThongBaos",
                newName: "LoaiThongBao");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ThongBaos",
                newName: "MaThongBao");

            migrationBuilder.AlterColumn<string>(
                name: "NoiDung",
                table: "ThongBaos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungMaNguoiDung",
                table: "BaiViets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs",
                column: "BaoCaoTaiKhoanId1");

            migrationBuilder.CreateIndex(
                name: "IX_BaiViets_NguoiDungMaNguoiDung",
                table: "BaiViets",
                column: "NguoiDungMaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_BaiViets_NguoiDungs_NguoiDungMaNguoiDung",
                table: "BaiViets",
                column: "NguoiDungMaNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "MaNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoans_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnhs",
                column: "BaoCaoTaiKhoanId",
                principalTable: "BaoCaoTaiKhoans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoans_BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs",
                column: "BaoCaoTaiKhoanId1",
                principalTable: "BaoCaoTaiKhoans",
                principalColumn: "Id");
        }
    }
}
