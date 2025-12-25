using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWAPFIT.Migrations
{
    /// <inheritdoc />
    public partial class anhgiohang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoTaiKhoanAnh_BaoCaoTaiKhoans_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnh");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaoCaoTaiKhoanAnh",
                table: "BaoCaoTaiKhoanAnh");

            migrationBuilder.RenameTable(
                name: "BaoCaoTaiKhoanAnh",
                newName: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.RenameIndex(
                name: "IX_BaoCaoTaiKhoanAnh_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnhs",
                newName: "IX_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoanId");

            migrationBuilder.AddColumn<string>(
                name: "AnhSanPham",
                table: "ChiTietGioHangs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaoCaoTaiKhoanAnhs",
                table: "BaoCaoTaiKhoanAnhs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs",
                column: "BaoCaoTaiKhoanId1");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoans_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropForeignKey(
                name: "FK_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoans_BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BaoCaoTaiKhoanAnhs",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropIndex(
                name: "IX_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.DropColumn(
                name: "AnhSanPham",
                table: "ChiTietGioHangs");

            migrationBuilder.DropColumn(
                name: "BaoCaoTaiKhoanId1",
                table: "BaoCaoTaiKhoanAnhs");

            migrationBuilder.RenameTable(
                name: "BaoCaoTaiKhoanAnhs",
                newName: "BaoCaoTaiKhoanAnh");

            migrationBuilder.RenameIndex(
                name: "IX_BaoCaoTaiKhoanAnhs_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnh",
                newName: "IX_BaoCaoTaiKhoanAnh_BaoCaoTaiKhoanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BaoCaoTaiKhoanAnh",
                table: "BaoCaoTaiKhoanAnh",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BaoCaoTaiKhoanAnh_BaoCaoTaiKhoans_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnh",
                column: "BaoCaoTaiKhoanId",
                principalTable: "BaoCaoTaiKhoans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
