using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWAPFIT.Models
{
    public class TinNhan
    {
        [Key]
        public int MaTinNhan { get; set; }

        public int MaNguoiGui { get; set; }
        public int MaNguoiNhan { get; set; }

        public string? NoiDung { get; set; }

        public DateTime? ThoiGianGui { get; set; }

        public bool? DaDoc { get; set; }
        [ForeignKey(nameof(MaNguoiGui))]
        public NguoiDung? NguoiGui { get; set; }

        [ForeignKey(nameof(MaNguoiNhan))]
        public NguoiDung? NguoiNhan { get; set; }

    }
}
