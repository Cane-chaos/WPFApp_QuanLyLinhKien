using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using WPFApp_QuanLyLinhKien.Models;

namespace WPFApp_QuanLyLinhKien.ViewModels.Admin
{
    public class BaoCaoViewModel : BaseViewModel
    {
        private readonly QLKhachHangViewModel _khachHangVM;
        private readonly QLDonHangViewModel _donHangVM;

        private int _tongSoKhachHang;
        private int _tongSoDonHang;
        private decimal _tongDoanhThu;

        public int TongSoKhachHang { get => _tongSoKhachHang; private set => SetProperty(ref _tongSoKhachHang, value); }
        public int TongSoDonHang { get => _tongSoDonHang; private set => SetProperty(ref _tongSoDonHang, value); }
        public decimal TongDoanhThu { get => _tongDoanhThu; private set => SetProperty(ref _tongDoanhThu, value); }

        public BaoCaoViewModel(QLKhachHangViewModel khVM, QLDonHangViewModel dhVM)
        {
            _khachHangVM = khVM;
            _donHangVM = dhVM;
            _khachHangVM.KhachHangs.CollectionChanged += OnDataChanged;
            _donHangVM.HoaDons.CollectionChanged += OnDataChanged;
            Recalculate();
        }

        private void OnDataChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Recalculate();
        }

        private void Recalculate()
        {
            TongSoKhachHang = _khachHangVM.KhachHangs.Count;
            TongSoDonHang = _donHangVM.HoaDons.Count;
            TongDoanhThu = _donHangVM.HoaDons.Sum(h => h.TongTien ?? 0m);
        }
    }
}
