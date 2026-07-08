using HotelManagement.Models;
using HotelManagement.ViewModels;
using Microsoft.Extensions.Configuration;
using System.IO;
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
        CustomersManagement CM = new CustomersManagement();
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Email và Mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string adminEmail = "admin@FUMiniHotelSystem.com";
                string adminPassword = "@@abc123@@";

                if (email == adminEmail && password == adminPassword)
                {
                    labelError.Visibility = Visibility.Hidden;
                    AdminWindow adminWindow = new AdminWindow();
                    this.Close();
                    adminWindow.Show();
                    return;
                }

                Customer loggedInCustomer = CM.GetCustomerByEmailAndPassword(email, password);
                if (loggedInCustomer != null)
                {
                    CustomerDashboard customerDashboard = new CustomerDashboard(loggedInCustomer);
                    this.Close();
                    customerDashboard.Show();
                }
                else
                {
                    labelError.Visibility = Visibility.Visible;
                    txtPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống khi đăng nhập: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }
    }
}