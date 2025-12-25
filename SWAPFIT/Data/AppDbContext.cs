using Microsoft.EntityFrameworkCore;
using SWAPFIT.Model;
using SWAPFIT.Models;

namespace SWAPFIT.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<DoCu> DoCus { get; set; }
        public DbSet<HinhAnh> HinhAnhs { get; set; }
        public DbSet<YeuCau> YeuCaus { get; set; }
        public DbSet<ThuongHieu> ThuongHieus { get; set; }
        public DbSet<DiaChi> DiaChis { get; set; }
        public DbSet<GiaoDich> GiaoDichs { get; set; }
        public DbSet<TinNhan> TinNhans { get; set; }
        public DbSet<TinTuc> TinTucs { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<BaoCao> BaoCaos { get; set; }
        public DbSet<YeuThich> YeuThichs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<PhanHoiHoTro> PhanHoiHoTros { get; set; }
        public DbSet<HoTro> HoTros { get; set; }
        public DbSet<BinhLuanTinTuc> BinhLuanTinTucs { get; set; }
        public DbSet<LichSuDangNhap> LichSuDangNhaps { get; set; }
        public DbSet<LuotTimKiem> LuotTimKiems { get; set; }
        public DbSet<NhatKyHeThong> NhatKyHeThongs { get; set; }
        public DbSet<ThongKe> ThongKes { get; set; }
        public DbSet<TheLoaiTinTuc> TheLoaiTinTucs { get; set; }
        public DbSet<BaiViet> BaiViets { get; set; }
        public DbSet<AnhBaiViet> AnhBaiViets { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<BaoCaoTaiKhoan> BaoCaoTaiKhoans { get; set; }
        public DbSet<UuDai> UuDais { get; set; }
        public DbSet<UuDaiSanPham> UuDaiSanPhams { get; set; }
        public DbSet<UserVoucher> UserVouchers { get; set; }
        public DbSet<BaoCaoTaiKhoanAnh> BaoCaoTaiKhoanAnhs { get; set; }
        public object BaiVietDTO { get; internal set; }
        public DbSet<ThongKeDanhMucTongHop> ThongKeDanhMucTongHops { get; set; } = null!;
        public DbSet<TaiKhoanNganHang> TaiKhoanNganHang { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DoCu>()
                .Property(d => d.GiaTriUocTinh)
                .HasPrecision(18, 2);
            modelBuilder.Entity<YeuCau>()
                .HasOne(y => y.NguoiGui)
                .WithMany()
                .HasForeignKey(y => y.NguoiGuiId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<YeuCau>()
                .HasOne(y => y.NguoiNhan)
                .WithMany()
                .HasForeignKey(y => y.NguoiNhanId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DoCu>()
                .HasOne<DanhMuc>()
                .WithMany()
                .HasForeignKey(d => d.MaDanhMuc)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DoCu>()
                .HasOne<ThuongHieu>()
                .WithMany()
                .HasForeignKey(d => d.MaThuongHieu)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<DoCu>()
                .HasOne<NguoiDung>()
                .WithMany()
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DiaChi>()
                .HasOne<NguoiDung>()
                .WithMany()
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BaiViet>()
                .HasOne(b => b.NguoiDung)
                .WithMany(n => n.BaiViets)
                .HasForeignKey(b => b.MaNguoiDung)
                 .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<GioHang>()
                .HasMany(g => g.ChiTietGioHangs)
                .WithOne(ct => ct.GioHang)
                .HasForeignKey(ct => ct.MaGioHang)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChiTietGioHang>()
                .HasOne(ct => ct.BaiViet)
                .WithMany()
                .HasForeignKey(ct => ct.MaBaiViet)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TinNhan>()
                .HasOne<NguoiDung>()
                .WithMany() 
                .HasForeignKey(t => t.MaNguoiGui)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TinNhan>()
                .HasOne<NguoiDung>()
                .WithMany()
                .HasForeignKey(t => t.MaNguoiNhan)
                .OnDelete(DeleteBehavior.NoAction); 
            modelBuilder.Entity<DonHang>()
                .HasOne(d => d.NguoiMua)
                .WithMany(u => u.DonMua)
                .HasForeignKey(d => d.MaNguoiMua)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DonHang>()
                .HasOne(d => d.NguoiBan)
                .WithMany(u => u.DonBan)
                .HasForeignKey(d => d.MaNguoiBan)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BaoCaoTaiKhoan>()
                .HasOne(b => b.NguoiBaoCao)
                .WithMany()
                .HasForeignKey(b => b.MaNguoiBaoCao)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BaoCaoTaiKhoan>()
                .HasOne(b => b.NguoiBiBaoCao)
                .WithMany()
                .HasForeignKey(b => b.MaNguoiBiBaoCao)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<UuDaiSanPham>()
                .HasOne(udp => udp.UuDai)
                .WithMany()
                .HasForeignKey(udp => udp.MaUuDai)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<UuDaiSanPham>()
                .HasOne(udp => udp.DoCu)
                .WithMany()
                .HasForeignKey(udp => udp.MaDoCu)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<BaoCaoTaiKhoanAnh>()
               .HasOne(b => b.BaoCaoTaiKhoan)
               .WithMany(b => b.BaoCaoTaiKhoanAnhs)  
               .HasForeignKey(b => b.BaoCaoTaiKhoanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaiKhoanNganHang>()
                 .HasOne(tk => tk.NguoiDung)
                 .WithOne(u => u.TaiKhoanNganHang)
                 .HasForeignKey<TaiKhoanNganHang>(tk => tk.MaNguoiDung)
                 .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
