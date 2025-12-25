using SWAPFIT.Models;
using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Model
{
    public class TaiKhoanNganHang
    {
        [Key]
        public int MaTaiKhoanNganHang { get; set; }

        public int MaNguoiDung { get; set; }
        public NguoiDung? NguoiDung { get; set; }

        public string? TenNganHang { get; set; }
        public string? SoTaiKhoan { get; set; }
        public string? ChuTaiKhoan { get; set; }

        public string? QrCodeImage { get; set; }   
    }

}
