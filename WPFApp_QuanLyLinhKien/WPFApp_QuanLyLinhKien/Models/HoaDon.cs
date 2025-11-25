using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPFApp_QuanLyLinhKien.Models
{
    public class HoaDon : INotifyPropertyChanged
    {
        [Key]
        public int MaHD { get; set; }

        // QUAN HỆ
        [ForeignKey(nameof(KhachHang))]
        public int? MaKH { get; set; }
        public KhachHang KhachHang { get; set; }

        [ForeignKey(nameof(NhanVien))]
        public int? MaNV { get; set; }
        public NhanVien NhanVien { get; set; }

        private DateTime _ngayLap = DateTime.Now;
        public DateTime NgayLap
        {
            get => _ngayLap;
            set
            {
                if (_ngayLap != value)
                {
                    _ngayLap = value;
                    OnPropertyChanged(nameof(NgayLap));
                }
            }
        }

        private decimal? _tongTien;
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TongTien
        {
            get => _tongTien;
            set
            {
                if (_tongTien != value)
                {
                    _tongTien = value;
                    OnPropertyChanged(nameof(TongTien));
                }
            }
        }

        public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();


      


        // NOTIFY PROPERTY CHANGED
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
