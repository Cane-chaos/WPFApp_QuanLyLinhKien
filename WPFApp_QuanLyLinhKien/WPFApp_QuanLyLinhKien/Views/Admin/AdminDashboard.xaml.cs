using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFApp_QuanLyLinhKien.Views.Admin;
using WPFApp_QuanLyLinhKien.ViewModels.Admin;


namespace WPFApp_QuanLyLinhKien.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();

            // 🔥 Load QLDonHangView khi đăng nhập thành công
            var view = new QLDonHangView();
            view.DataContext = new QLDonHangViewModel();

            MainContent.Content = view;
        }
    }

}
