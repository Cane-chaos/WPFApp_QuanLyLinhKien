using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WPFApp_QuanLyLinhKien.Database;
using WPFApp_QuanLyLinhKien.Models;

namespace WPFApp_QuanLyLinhKien.ViewModels.Admin
{
    public class QLDonHangViewModel : BaseViewModel
    {

        public ObservableCollection<KhachHang> KhachHangs { get; } = new();
        public ObservableCollection<NhanVien> NhanViens { get; } = new();

        public ObservableCollection<HoaDon> HoaDons { get; set; } = new();
        public ObservableCollection<SanPham> SanPhams { get; set; } = new();
        public ObservableCollection<ChiTietHoaDon> CurrentEditingItems { get; set; } = new();

        private HoaDon _selectedHoaDon;
        public HoaDon SelectedHoaDon
        {
            get => _selectedHoaDon;
            set
            {
                if (SetProperty(ref _selectedHoaDon, value))
                {
                    if (value != null)
                        LoadEditingOrder(value);
                }
            }
        }

        private HoaDon _currentEditing = new();
        public HoaDon CurrentEditing
        {
            get => _currentEditing;
            set => SetProperty(ref _currentEditing, value);
        }

        public ICommand AddHoaDonCommand { get; }
        public ICommand SaveHoaDonCommand { get; }
        public ICommand DeleteHoaDonCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand RecalcCommand { get; }

        public QLDonHangViewModel()
        {
           
            LoadData();


            AddHoaDonCommand = new RelayCommand(_ => AddHoaDon());
            SaveHoaDonCommand = new RelayCommand(_ => SaveHoaDon(), _ => SelectedHoaDon != null);
            DeleteHoaDonCommand = new RelayCommand(_ => DeleteHoaDon(), _ => SelectedHoaDon != null);
            AddItemCommand = new RelayCommand(o => AddItem(o as SanPham));
            RemoveItemCommand = new RelayCommand(o => RemoveItem(o as ChiTietHoaDon));
            RecalcCommand = new RelayCommand(_ => RecalcTotal());

            System.Diagnostics.Debug.WriteLine("SanPhams count = " + SanPhams.Count);
            System.Diagnostics.Debug.WriteLine("HoaDons count = " + HoaDons.Count);
        }

        // ---------------- LOAD DATA ----------------
        public void LoadData()
        {
            using var db = new AppDbContext();

            SanPhams.Clear();
            foreach (var sp in db.SanPhams.ToList())
                SanPhams.Add(sp);

            HoaDons.Clear();
            foreach (var hd in db.HoaDons
                .Include(h => h.ChiTietHoaDons)
                .ThenInclude(ct => ct.SanPham))
                HoaDons.Add(hd);
            KhachHangs.Clear();
            foreach (var kh in db.KhachHangs.ToList())
                KhachHangs.Add(kh);

            NhanViens.Clear();
            foreach (var nv in db.NhanViens.ToList())
                NhanViens.Add(nv);

        }

        // --------------- LOAD EDITING ORDER -----------
        void LoadEditingOrder(HoaDon hd)
        {
            CurrentEditing = new HoaDon
            {
                MaHD = hd.MaHD,
                MaKH = hd.MaKH,
                NgayLap = hd.NgayLap,
                TongTien = hd.TongTien
            };

            CurrentEditingItems = new ObservableCollection<ChiTietHoaDon>(
                hd.ChiTietHoaDons.Select(ct => new ChiTietHoaDon
                {
                    MaHD = ct.MaHD,
                    MaSP = ct.MaSP,
                    SanPham = ct.SanPham,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia
                })
            );

            OnPropertyChanged(nameof(CurrentEditingItems));
        }

        // ---------------- ADD ORDER ----------------
        void AddHoaDon()
        {
            using var db = new AppDbContext();


            var hd = new HoaDon
            {
                MaKH = CurrentEditing.MaKH,
                NgayLap = System.DateTime.Now
            };
            db.HoaDons.Add(hd);
            db.SaveChanges();

            foreach (var ct in CurrentEditingItems)
            {
                ct.MaHD = hd.MaHD;
                db.ChiTietHoaDons.Add(new ChiTietHoaDon
                {
                    MaHD = hd.MaHD,
                    MaSP = ct.MaSP,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia
                });
            }

            db.SaveChanges();
            LoadData();

            CurrentEditing = new HoaDon();
            CurrentEditingItems.Clear();
            SelectedHoaDon = null;
        }

        // ---------------- SAVE ORDER ----------------
        void SaveHoaDon()
        {
            if (SelectedHoaDon == null)
                return;

            try
            {
                using var db = new AppDbContext();

                // Lấy đúng hóa đơn từ DB (có chi tiết)
                var hd = db.HoaDons
                    .Include(h => h.ChiTietHoaDons)
                    .First(x => x.MaHD == SelectedHoaDon.MaHD);

                // Cập nhật thông tin chung
                hd.MaKH = CurrentEditing.MaKH;
                hd.MaNV = CurrentEditing.MaNV;
                hd.NgayLap = CurrentEditing.NgayLap;

                // Xóa toàn bộ chi tiết cũ
                db.ChiTietHoaDons.RemoveRange(hd.ChiTietHoaDons);

                // Tính tổng tiền
                decimal tong = 0;

                // Thêm lại chi tiết mới
                foreach (var ct in CurrentEditingItems)
                {
                    var newCT = new ChiTietHoaDon
                    {
                        MaHD = hd.MaHD,
                        MaSP = ct.MaSP,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia
                    };

                    tong += ct.SoLuong * ct.DonGia;

                    db.ChiTietHoaDons.Add(newCT);
                }

                // Cập nhật tổng tiền
                hd.TongTien = tong;

                // Lưu tất cả thay đổi xuống database
                db.SaveChanges();

                // Tải lại danh sách về ViewModel
                LoadData();

                // Reset panel
                SelectedHoaDon = null;
                CurrentEditing = new HoaDon();
                CurrentEditingItems.Clear();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SaveHoaDon ERROR: " + ex.Message);
            }
        }


        // ---------------- DELETE ORDER ----------------
        void DeleteHoaDon()
        {
            if (SelectedHoaDon == null) return;

            using var db = new AppDbContext();

            var hd = db.HoaDons
                .Include(h => h.ChiTietHoaDons)
                .First(x => x.MaHD == SelectedHoaDon.MaHD);

            db.ChiTietHoaDons.RemoveRange(hd.ChiTietHoaDons);
            db.HoaDons.Remove(hd);
            db.SaveChanges();

            LoadData();
            CurrentEditingItems.Clear();
            SelectedHoaDon = null;
        }

        // ---------------- ITEM EDITING ----------------
        void AddItem(SanPham sp)
        {
            if (sp == null) return;

            CurrentEditingItems.Add(new ChiTietHoaDon
            {
                MaSP = sp.MaSP,
                SanPham = sp,
                DonGia = sp.GiaBan,
                SoLuong = 1
            });

            RecalcTotal();
            OnPropertyChanged(nameof(CurrentEditingItems));
        }

        void RemoveItem(ChiTietHoaDon ct)
        {
            if (ct == null) return;

            CurrentEditingItems.Remove(ct);
            RecalcTotal();
            OnPropertyChanged(nameof(CurrentEditingItems));
        }

        // ---------------- TOTAL CALC ----------------
        void RecalcTotal()
        {
            CurrentEditing.TongTien = CurrentEditingItems.Sum(i => i.DonGia * i.SoLuong);
            OnPropertyChanged(nameof(CurrentEditing));
        }
      

    }

}

