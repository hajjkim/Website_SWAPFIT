using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWAPFIT.Migrations
{
    /// <inheritdoc />
    public partial class baocaotaikhoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaoCaoTaiKhoanAnh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaoCaoTaiKhoanId = table.Column<int>(type: "int", nullable: false),
                    DuongDan = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoTaiKhoanAnh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaoCaoTaiKhoanAnh_BaoCaoTaiKhoans_BaoCaoTaiKhoanId",
                        column: x => x.BaoCaoTaiKhoanId,
                        principalTable: "BaoCaoTaiKhoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoTaiKhoanAnh_BaoCaoTaiKhoanId",
                table: "BaoCaoTaiKhoanAnh",
                column: "BaoCaoTaiKhoanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoTaiKhoanAnh");
        }
    }
}
