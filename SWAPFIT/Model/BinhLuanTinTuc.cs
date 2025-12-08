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

        // 💡 Thêm trường ParentId để hỗ trợ reply
        public int? ParentId { get; set; }  // null = bình luận cha, != null = reply

        [ForeignKey("ParentId")]
        public BinhLuanTinTuc? Parent { get; set; }

        public ICollection<BinhLuanTinTuc>? Replies { get; set; }  // danh sách reply

        [ForeignKey("MaNguoiDung")]
        public NguoiDung? NguoiDung { get; set; }
    }
}
