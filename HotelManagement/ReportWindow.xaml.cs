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
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn đầy đủ Từ ngày và Đến ngày!");
                return;
            }

            DateOnly startDate = DateOnly.FromDateTime(dpStartDate.SelectedDate.Value);
            DateOnly endDate = DateOnly.FromDateTime(dpEndDate.SelectedDate.Value);

            using (var db = new FuminiHotelManagementContext())
            {
                var query = db.BookingReservations
                    .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
                    .OrderByDescending(b => b.BookingDate)
                    .ToList();

                LvReport.ItemsSource = query;
            }
        }
    }
}
