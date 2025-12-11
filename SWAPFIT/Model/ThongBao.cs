<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;
=======
﻿using System;
using System.ComponentModel.DataAnnotations;
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff

namespace SWAPFIT.Models
{
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }
<<<<<<< HEAD
        public int MaNguoiDung { get; set; }
        public string? NoiDung { get; set; }
        public string? LienKet { get; set; }

        // SỬA: Bỏ dấu ? → không nullable nữa
        public bool DaXem { get; set; } = false;           // ← Thêm default false
        public DateTime NgayTao { get; set; } = DateTime.Now; // ← Thêm default

        public string? LoaiThongBao { get; set; }
    }
}
=======

        public int MaNguoiDung { get; set; }

        public string? NoiDung { get; set; }

        public string? LienKet { get; set; }

        public bool? DaXem { get; set; }
        public string? LoaiThongBao { get; set; }
        public DateTime? NgayTao { get; set; }
    }
}
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
