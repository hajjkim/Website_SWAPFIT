using SWAPFIT.Models;

namespace SWAPFIT.Model
{
    public class UserVoucher
    {
        public int UserVoucherId { get; set; }

        public int UserId { get; set; }
        public virtual NguoiDung User { get; set; }

        public int VoucherId { get; set; }
        public virtual UuDai Voucher { get; set; }

        public DateTime DateClaimed { get; set; }
    }

}
