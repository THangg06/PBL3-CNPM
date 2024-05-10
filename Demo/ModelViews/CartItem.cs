namespace Demo.ModelViews
{
	public class CartItem
	{
		public string MaHh { get; set; }
		public string Hinh { get; set; }
		public string TenHH { get; set; }
		//public double? Dongia { get; set; }
		public decimal Dongia { get; set; }
		public int SoLuong { get; set; }
		public decimal ThanhTien => SoLuong * Dongia ;
	}
}
