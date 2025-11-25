using System.Windows;
using WPFApp_QuanLyLinhKien.ViewModels;
using WPFApp_QuanLyLinhKien.ViewModels.Admin;

namespace WPFApp_QuanLyLinhKien
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new QLDonHangViewModel();
        }
    }
}