using SWAPFIT.Models;

namespace SWAPFIT.Model
{
    public class UserVoucher
    {
        public int UserVoucherId { get; set; }

        // Khóa ngoại đến bảng NguoiDung (User)
        public int UserId { get; set; }
        public virtual NguoiDung User { get; set; }

        // Khóa ngoại đến bảng UuDai (Voucher)
        public int VoucherId { get; set; }
        public virtual UuDai Voucher { get; set; }

        // Lưu ngày người dùng nhận voucher
        public DateTime DateClaimed { get; set; }
    }

}
