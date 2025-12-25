using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class BinhLuanTinTuc
    {
        [Key]
        public int MaBinhLuan { get; set; }

        public int MaTinTuc { get; set; }

        public int MaNguoiDung { get; set; }

        public string? NoiDung { get; set; }

        public DateTime? NgayBinhLuan { get; set; }

      
        public int? ParentId { get; set; }  

        [ForeignKey("ParentId")]
        public BinhLuanTinTuc? Parent { get; set; }

        public ICollection<BinhLuanTinTuc>? Replies { get; set; }  

        [ForeignKey("MaNguoiDung")]
        public NguoiDung? NguoiDung { get; set; }
    }
}
