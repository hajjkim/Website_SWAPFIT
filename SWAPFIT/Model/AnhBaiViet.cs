using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class AnhBaiViet
    {
        [Key]
        public int MaAnh { get; set; }

        public int MaBaiViet { get; set; }

        [StringLength(255)]
        public string DuongDan { get; set; } = string.Empty;

        [ForeignKey("MaBaiViet")]
        public BaiViet BaiViet { get; set; }
    }
}
