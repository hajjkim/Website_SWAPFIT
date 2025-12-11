using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BaiViet
    {
<<<<<<< HEAD
        internal readonly string HoTen;
        internal readonly string TenDangNhap;
        internal readonly string TenDanhMuc;
        internal readonly string? DuongDan;

        [Key]
        public int MaBaiViet { get; set; }

        [Required]
        [ForeignKey("NguoiDung")] // Tham chiếu tới đối tượng NguoiDung thay vì trường MaNguoiDung
        public int MaNguoiDung { get; set; }

        public NguoiDung? NguoiDung { get; set; }

=======
        [Key]
        public int MaBaiViet { get; set; }

        public int MaNguoiDung { get; set; }

        [ForeignKey(nameof(MaNguoiDung))]
        public NguoiDung? NguoiDung { get; set; }


>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        public int? MaDanhMuc { get; set; }
        public int? MaThuongHieu { get; set; }
        public int? MaDiaChi { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
        public string? TieuDe { get; set; }

        public string? NoiDung { get; set; }

        [StringLength(50)]
        public string? LoaiBaiDang { get; set; } // “Tặng” hoặc “Bán”

        public decimal? GiaSanPham { get; set; } // Giá nếu là “Bán”

        [StringLength(100)]
        public string? TrangThai { get; set; }

<<<<<<< HEAD
        public DateTime? NgayTao { get; set; }
=======
        public DateTime? NgayTao { get; set; } = DateTime.Now;
>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff

        // 🆕 Thêm trường mới
        [StringLength(20)]
        public string? Size { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        public ICollection<AnhBaiViet>? AnhBaiViets { get; set; }
<<<<<<< HEAD
=======

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
        // ==================== Navigation properties ====================
        [ForeignKey(nameof(MaDanhMuc))]
        public DanhMuc? DanhMuc { get; set; }

        [ForeignKey(nameof(MaThuongHieu))]
        public ThuongHieu? ThuongHieu { get; set; }

        [ForeignKey(nameof(MaDiaChi))]
        public DiaChi? DiaChi { get; set; }
<<<<<<< HEAD
        public string? LyDo { get; set; } // thêm dấu ? để nullable → không bắt buộc nữa
        //public string HoTen { get; internal set; }
        //public string TenDangNhap { get; internal set; }
        //public string TenDanhMuc { get; internal set; }
        //public string HinhAnh { get; internal set; }
        //public string DuongDan { get; internal set; }
=======

        [StringLength(500)]
        public string? LyDoTuChoi { get; set; }
        public ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();

>>>>>>> cff493713bfe5280dbb98db99eb56a2baceef7ff
    }
}
