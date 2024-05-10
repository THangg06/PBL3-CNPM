namespace Demo.ModelViews
{
    public class HangHoaVM
    {
       public string MaHH { get; set; }
        public string TenHH { get; set; }
        public string? Hinh { get; set; }
        public decimal? Dongia { get; set; }
        public string? MoTaNgan { get; set; }
        public string TenLoai { get; set; }
        
    }
    public class ChiTietHangHoaVM
    {
        public string MaHH { get; set; }
        public string TenHH { get; set; }
        public string? Hinh { get; set; }
        public decimal? Dongia { get; set; }
        public string? MoTaNgan { get; set; }
        public string TenLoai { get; set; }

        public string ChiTiet { get; set; }
        public int DiemDanhGia {  get; set; }
        public int SoLuongTon {  get; set; }
    }
}
