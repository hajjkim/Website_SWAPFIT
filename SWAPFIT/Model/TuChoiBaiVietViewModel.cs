using System.ComponentModel.DataAnnotations;

namespace SWAPFIT.Models
{
    public class TuChoiBaiVietViewModel
    {
        public int MaBaiViet { get; set; }
        public string? TieuDe { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lý do từ chối.")]
        [StringLength(500)]
        public string LyDoTuChoi { get; set; } = string.Empty;
    }
}
