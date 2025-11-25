using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WPFApp_QuanLyLinhKien.Models;

namespace WPFApp_QuanLyLinhKien.ViewModels.Admin
{
    public class QLKhachHangViewModel : BaseViewModel
    {
        private KhachHang? _selectedKhachHang;
        private string _tenKH = string.Empty;
        private string _diaChi = string.Empty;
        private string _sdt = string.Empty;
        private string _email = string.Empty;
        private bool _isEditing;

        public ObservableCollection<KhachHang> KhachHangs { get; } = new ObservableCollection<KhachHang>();

        public KhachHang? SelectedKhachHang
        {
            get => _selectedKhachHang;
            set
            {
                if (SetProperty(ref _selectedKhachHang, value))
                {
                    if (value != null && IsEditing)
                    {
                        TenKH = value.TenKH;
                        DiaChi = value.DiaChi;
                        SDT = value.SDT;
                        Email = value.Email;
                    }
                    OnPropertyChanged(nameof(CanEditOrDelete));
                }
            }
        }

        public string TenKH { get => _tenKH; set => SetProperty(ref _tenKH, value); }
        public string DiaChi { get => _diaChi; set => SetProperty(ref _diaChi, value); }
        public string SDT { get => _sdt; set => SetProperty(ref _sdt, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public bool IsEditing { get => _isEditing; set => SetProperty(ref _isEditing, value); }

        public bool CanEditOrDelete => SelectedKhachHang != null;
        public bool HasValidInput => !string.IsNullOrWhiteSpace(TenKH);

        public ICommand AddCommand { get; }
        public ICommand StartEditCommand { get; }
        public ICommand SaveEditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearFormCommand { get; }

        private int _nextId = 1;

        public QLKhachHangViewModel()
        {
            // Dummy data
            KhachHangs.Add(new KhachHang { MaKH = _nextId++, TenKH = "Nguyễn Văn A", DiaChi = "HN", SDT = "0901234567", Email = "a@example.com" });
            KhachHangs.Add(new KhachHang { MaKH = _nextId++, TenKH = "Trần Thị B", DiaChi = "HCM", SDT = "0912345678", Email = "b@example.com" });

            AddCommand = new RelayCommand(_ => Add(), _ => HasValidInput && !IsEditing);
            StartEditCommand = new RelayCommand(_ => BeginEdit(), _ => SelectedKhachHang != null && !IsEditing);
            SaveEditCommand = new RelayCommand(_ => SaveEdit(), _ => IsEditing && HasValidInput);
            DeleteCommand = new RelayCommand(_ => Delete(), _ => SelectedKhachHang != null);
            ClearFormCommand = new RelayCommand(_ => ClearForm(), _ => true);
        }

        private void Add()
        {
            var kh = new KhachHang
            {
                MaKH = _nextId++,
                TenKH = TenKH,
                DiaChi = DiaChi,
                SDT = SDT,
                Email = Email
            };
            KhachHangs.Add(kh);
            ClearForm();
        }

        private void BeginEdit()
        {
            if (SelectedKhachHang == null) return;
            IsEditing = true;
            TenKH = SelectedKhachHang.TenKH;
            DiaChi = SelectedKhachHang.DiaChi;
            SDT = SelectedKhachHang.SDT;
            Email = SelectedKhachHang.Email;
        }

        private void SaveEdit()
        {
            if (SelectedKhachHang == null) return;
            SelectedKhachHang.TenKH = TenKH;
            SelectedKhachHang.DiaChi = DiaChi;
            SelectedKhachHang.SDT = SDT;
            SelectedKhachHang.Email = Email;
            // Force refresh
            var index = KhachHangs.IndexOf(SelectedKhachHang);
            if (index >= 0)
            {
                KhachHangs[index] = SelectedKhachHang;
            }
            IsEditing = false;
            ClearForm();
        }

        private void Delete()
        {
            if (SelectedKhachHang == null) return;
            KhachHangs.Remove(SelectedKhachHang);
            SelectedKhachHang = null;
            ClearForm();
        }

        private void ClearForm()
        {
            TenKH = string.Empty;
            DiaChi = string.Empty;
            SDT = string.Empty;
            Email = string.Empty;
            IsEditing = false;
            OnPropertyChanged(nameof(HasValidInput));
        }
    }
}
