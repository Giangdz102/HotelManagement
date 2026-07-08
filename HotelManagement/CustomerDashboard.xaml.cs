using HotelManagement.Models;
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

namespace HotelManagement
{
    /// <summary>
    /// Interaction logic for CustomerDashboard.xaml
    /// </summary>
    public partial class CustomerDashboard : Window
    {
        private Customer _currentCustomer;

        public CustomerDashboard(Customer customer)
        {
            InitializeComponent();
            _currentCustomer = customer;
            LoadProfile();
            LoadHistory();
        }

        private void LoadProfile()
        {
            if (_currentCustomer != null)
            {
                txtFullName.Text = _currentCustomer.CustomerFullName;
                txtTelephone.Text = _currentCustomer.Telephone;
                txtEmail.Text = _currentCustomer.EmailAddress;
                txtBirthday.Text = _currentCustomer.CustomerBirthday?.ToString("yyyy-MM-dd");
            }
        }

        private void LoadHistory()
        {
            using (var db = new FuminiHotelManagementContext())
            {
                LvBookingHistory.ItemsSource = db.BookingReservations
                    .Where(b => b.CustomerId == _currentCustomer.CustomerId)
                    .OrderByDescending(b => b.BookingDate)
                    .ToList();
            }
        }

        private void BtnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FuminiHotelManagementContext())
                {
                    var customerInDb = db.Customers.FirstOrDefault(c => c.CustomerId == _currentCustomer.CustomerId);
                    if (customerInDb != null)
                    {
                        customerInDb.CustomerFullName = txtFullName.Text;
                        customerInDb.Telephone = txtTelephone.Text;

                        if (DateOnly.TryParseExact(txtBirthday.Text.Trim(), new string[] { "yyyy-MM-dd", "dd/MM/yyyy", "d/M/yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out DateOnly parsedDate))
                        {
                            customerInDb.CustomerBirthday = parsedDate;
                        }
                        else
                        {
                            MessageBox.Show("Ngày sinh không đúng định dạng (khuyên dùng yyyy-MM-dd)!");
                            return;
                        }

                        db.SaveChanges();
                        _currentCustomer = customerInDb; // Update local ref
                        MessageBox.Show("Cập nhật Profile thành công!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }
    }
}
