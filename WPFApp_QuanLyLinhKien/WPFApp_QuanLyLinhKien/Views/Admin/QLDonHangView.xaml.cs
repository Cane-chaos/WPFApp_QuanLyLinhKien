using System.Windows.Controls;

namespace WPFApp_QuanLyLinhKien.Views.Admin
{
    public partial class QLDonHangView : UserControl
    {
        public QLDonHangView()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("QLDonHangView DataContext = " + DataContext);

            };
        }


    }
}
