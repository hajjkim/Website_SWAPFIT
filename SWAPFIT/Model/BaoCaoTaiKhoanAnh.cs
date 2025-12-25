using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BaoCaoTaiKhoanAnh
    {
        public int Id { get; set; }

        // Foreign key to BaoCaoTaiKhoan

        public int BaoCaoTaiKhoanId { get; set; }

        // Path to the image file
        public string DuongDan { get; set; }

        // Navigation property to BaoCaoTaiKhoan
        public BaoCaoTaiKhoan BaoCaoTaiKhoan { get; set; }
    }
}
