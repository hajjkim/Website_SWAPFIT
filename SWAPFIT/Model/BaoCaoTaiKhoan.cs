<<<<<<< HEAD
﻿using SWAPFIT.Model;
using System;
using System.Collections.Generic;
=======
﻿using System;
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BaoCaoTaiKhoan
    {
        public int Id { get; set; }

<<<<<<< HEAD
        // Foreign key to NguoiDung (the user who reported)
        public int MaNguoiBaoCao { get; set; }

        // Foreign key to NguoiDung (the user being reported)
=======
        // FK tới NguoiDung (người báo cáo)
        public int MaNguoiBaoCao { get; set; }

        // FK tới NguoiDung (người bị báo cáo)
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        public int MaNguoiBiBaoCao { get; set; }

        public string LyDo { get; set; } = string.Empty;
        public string? MoTaChiTiet { get; set; }

        public DateTime NgayTao { get; set; } = DateTime.Now;

        // "Moi", "DangXuLy", "DaXuLy"
        public string TrangThai { get; set; } = "Moi";

<<<<<<< HEAD
        // Navigation properties
        [ForeignKey(nameof(MaNguoiBaoCao))]
        public NguoiDung? NguoiBaoCao { get; set; }

        [ForeignKey(nameof(MaNguoiBiBaoCao))]
        public NguoiDung? NguoiBiBaoCao { get; set; }

        // List of images associated with the report
        public List<BaoCaoTaiKhoanAnh>? BaoCaoTaiKhoanAnhs { get; set; }
=======
        // 👉 Navigation: Người báo cáo
        [ForeignKey(nameof(MaNguoiBaoCao))]
        public NguoiDung? NguoiBaoCao { get; set; }

        // 👉 Navigation: Người bị báo cáo
        [ForeignKey(nameof(MaNguoiBiBaoCao))]
        public NguoiDung? NguoiBiBaoCao { get; set; }
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
    }
}
