using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWAPFIT.Migrations
{
    /// <inheritdoc />
    public partial class AddLyDoTuChoiToBaiViet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDo",
                table: "BaiViets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LyDoTuChoi",
                table: "BaiViets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDo",
                table: "BaiViets");

            migrationBuilder.DropColumn(
                name: "LyDoTuChoi",
                table: "BaiViets");
        }
    }
}
