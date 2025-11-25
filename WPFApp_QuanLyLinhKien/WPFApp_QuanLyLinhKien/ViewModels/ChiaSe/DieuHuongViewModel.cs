using System.Windows.Input;
using WPFApp_QuanLyLinhKien.ViewModels.Admin;

namespace WPFApp_QuanLyLinhKien.ViewModels
{
    public class DieuHuongViewModel : BaseViewModel
    {
        private BaseViewModel _currentView;
        public BaseViewModel CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand ShowKhachHangCommand { get; }
        public ICommand ShowDonHangCommand { get; }
        public ICommand ShowBaoCaoCommand { get; }

        public QLKhachHangViewModel KhachHangVM { get; }
        public QLDonHangViewModel DonHangVM { get; }
        public BaoCaoViewModel BaoCaoVM { get; }

        public DieuHuongViewModel()
        {
            KhachHangVM = new QLKhachHangViewModel();
            DonHangVM = new QLDonHangViewModel();
            BaoCaoVM = new BaoCaoViewModel(KhachHangVM, DonHangVM);

            ShowKhachHangCommand = new RelayCommand(_ => CurrentView = KhachHangVM);
            ShowDonHangCommand = new RelayCommand(_ => CurrentView = DonHangVM);
            ShowBaoCaoCommand = new RelayCommand(_ => CurrentView = BaoCaoVM);

            CurrentView = KhachHangVM; // mặc định
        }
    }
}
