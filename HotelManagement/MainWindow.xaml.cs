using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(txtUsername.Text == "admin" && txtPassword.Password == "123")
            {
                labelError.Visibility = Visibility.Hidden;
                Dashboard dashboard = new Dashboard();
                this.Hide();
                dashboard.Show();

            }
            else
            {
                labelError.Visibility = Visibility.Visible;
                txtPassword.Clear();
            }
        }
    }
}