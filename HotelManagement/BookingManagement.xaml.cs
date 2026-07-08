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
    /// Interaction logic for BookingManagement.xaml
    /// </summary>
    public partial class BookingManagement : Window
    {
        public BookingManagement()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new FuminiHotelManagementContext())
            {
                LvBookings.ItemsSource = db.BookingReservations.ToList();
            }
        }

        private void LvBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvBookings.SelectedItem is BookingReservation selectedBooking)
            {
                using (var db = new FuminiHotelManagementContext())
                {
                    LvBookingDetails.ItemsSource = db.BookingDetails
                        .Where(bd => bd.BookingReservationId == selectedBooking.BookingReservationId)
                        .ToList();
                }
            }
            else
            {
                LvBookingDetails.ItemsSource = null;
            }
        }
    }
}
