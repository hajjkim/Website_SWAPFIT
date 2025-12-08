using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BaoCaoTaiKhoan
    {
        public int Id { get; set; }

        // FK tới NguoiDung (người báo cáo)
        public int MaNguoiBaoCao { get; set; }

        // FK tới NguoiDung (người bị báo cáo)
        public int MaNguoiBiBaoCao { get; set; }

        public string LyDo { get; set; } = string.Empty;
        public string? MoTaChiTiet { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // "Moi", "DangXuLy", "DaXuLy"
        public string TrangThai { get; set; } = "Moi";

        // 👉 Navigation: Người báo cáo
        [ForeignKey(nameof(MaNguoiBaoCao))]
        public NguoiDung? NguoiBaoCao { get; set; }

        // 👉 Navigation: Người bị báo cáo
        [ForeignKey(nameof(MaNguoiBiBaoCao))]
        public NguoiDung? NguoiBiBaoCao { get; set; }
    }
}
