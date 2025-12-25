using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BaoCaoTaiKhoanAnh
    {
        public int Id { get; set; }


        public int BaoCaoTaiKhoanId { get; set; }

        public string DuongDan { get; set; }

        public BaoCaoTaiKhoan BaoCaoTaiKhoan { get; set; }
    }
}
