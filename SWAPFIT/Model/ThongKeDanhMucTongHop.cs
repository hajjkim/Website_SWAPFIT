<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class ThongKeDanhMucTongHop
    {
        [Key]
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
        public int TongSoBai { get; set; }
        public int TongSoLuotMua { get; set; }
=======
﻿namespace SWAPFIT.Models
{
    public class ThongKeDanhMucTongHop
    {
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }

        public int TongSoBai { get; set; }      // tổng số bài đăng thuộc danh mục
        public int TongSoLuotMua { get; set; }  // số đơn mua (lượt mua) thuộc danh mục
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
    }
}
