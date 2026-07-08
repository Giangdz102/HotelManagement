using HotelManagement.ViewModels;
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
    /// Interaction logic for CustomerManagement.xaml
    /// </summary>
    public partial class CustomerManagement : Window
    {
        CustomersManagement CM = new CustomersManagement();
        FuminiHotelManagementContext FM = new FuminiHotelManagementContext();
        private void LoadCustomers() => LvCustomers.ItemsSource = CM.GetCustomer();
        //private void LoadCustomers() => LvCustomers.ItemsSource = FM.Customers.ToList();
        private void LoadData(object sender, RoutedEventArgs e) => LoadCustomers();


        public CustomerManagement()
        {
            InitializeComponent();
            LoadDataFromDatabase();
        }
        private void LoadDataFromDatabase()
        {
            try
            {
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi kết nối CSDL: {ex.Message}", "Thông báo", MessageBoxButton.OK);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string keyword = TxtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadCustomers();
                return;
            }
            var filteredList = FM.Customers.ToList().Where(c =>
                (c.CustomerFullName != null && c.CustomerFullName.ToLower().Contains(keyword)) ||
                (c.EmailAddress != null && c.EmailAddress.ToLower().Contains(keyword))
            ).ToList();
            LvCustomers.ItemsSource = filteredList;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TxtSearch.Text = "";
            LoadCustomers();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(txtCustomerName.Text) ||
            string.IsNullOrWhiteSpace(txtTelephone.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text) ||
            string.IsNullOrWhiteSpace(txtBirthday.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ tất cả các trường thông tin!",
                            "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string birthdayInput = txtBirthday.Text.Trim();
                DateOnly parsedDate;
                string[] formats = new string[]
{
    "dd/MM/yyyy",   // Thích hợp cho: 27/10/2005
    "d/M/yyyy",     // Thích hợp cho: 2/5/2005
    "dd/M/yyyy",    // Thích hợp cho: 27/5/2005
    "d/MM/yyyy",    // Thích hợp cho: 2/10/2005
    "yyyy-MM-dd"    // Đề phòng trường hợp nhận chuỗi chuẩn ISO từ nguồn khác
};

                // Thực hiện parse với cấu hình Culture chung (InvariantCulture)
                bool isValidDate = DateOnly.TryParseExact(
                    birthdayInput,
                    formats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.AllowWhiteSpaces, // Cho phép có khoảng trắng an toàn
                    out parsedDate
                );
                if (!isValidDate)
                {
                    MessageBox.Show("Ngày sinh không đúng định dạng! Vui lòng nhập theo dạng Ngày/Tháng/Năm (Ví dụ: 25/12/2000).",
                                    "Lỗi định dạng", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var Customer = new Customer { CustomerFullName = txtCustomerName.Text, Telephone = txtTelephone.Text, EmailAddress = txtEmail.Text, CustomerBirthday = parsedDate, Password = "123@" };
                CM.InsertCustomer(Customer);
                LoadCustomers();
                MessageBox.Show("Thêm mới khách hàng thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi hệ thống: {ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LvCustomers.SelectedItem is Customer selectedCustomer)
                {
                    selectedCustomer.CustomerFullName = txtCustomerName.Text;
                    selectedCustomer.Telephone = txtTelephone.Text;
                    selectedCustomer.EmailAddress = txtEmail.Text;
                    if (DateOnly.TryParseExact(txtBirthday.Text.Trim(), new string[] { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "yyyy-MM-dd" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out DateOnly parsedDate))
                    {
                        selectedCustomer.CustomerBirthday = parsedDate;
                    }
                    else
                    {
                        MessageBox.Show("Ngày sinh không đúng định dạng!");
                        return;
                    }
                    CM.UpdateCustomer(selectedCustomer);
                    LoadCustomers();
                    MessageBox.Show("Cập nhật thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LvCustomers.SelectedItem is Customer selectedCustomer)
                {
                    var result = MessageBox.Show("Bạn có chắc muốn xoá?", "Xác nhận", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        CM.DeleteCustomer(selectedCustomer);
                        LoadCustomers();
                        MessageBox.Show("Xoá thành công!");
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
