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
    /// Interaction logic for RoomManagement.xaml
    /// </summary>
    public partial class RoomManagement : Window
    {
        public RoomManagement()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using (var db = new FuminiHotelManagementContext())
            {
                LvRooms.ItemsSource = db.RoomInformations.ToList();
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = TxtSearch.Text.Trim().ToLower();
            using (var db = new FuminiHotelManagementContext())
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    LvRooms.ItemsSource = db.RoomInformations.ToList();
                }
                else
                {
                    LvRooms.ItemsSource = db.RoomInformations.Where(r =>
                        (r.RoomNumber != null && r.RoomNumber.ToLower().Contains(keyword)) ||
                        (r.RoomDetailDescription != null && r.RoomDetailDescription.ToLower().Contains(keyword))
                    ).ToList();
                }
            }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new FuminiHotelManagementContext())
                {
                    var newRoom = new RoomInformation
                    {
                        RoomNumber = txtRoomNumber.Text,
                        RoomDetailDescription = txtDescription.Text,
                        RoomMaxCapacity = int.TryParse(txtCapacity.Text, out int cap) ? cap : null,
                        RoomTypeId = int.TryParse(txtTypeId.Text, out int typeId) ? typeId : 1,
                        RoomPricePerDay = decimal.TryParse(txtPrice.Text, out decimal price) ? price : null,
                        RoomStatus = 1
                    };
                    db.RoomInformations.Add(newRoom);
                    db.SaveChanges();
                    LoadData();
                    MessageBox.Show("Thêm mới thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (LvRooms.SelectedItem is RoomInformation selectedRoom)
            {
                try
                {
                    using (var db = new FuminiHotelManagementContext())
                    {
                        var room = db.RoomInformations.FirstOrDefault(r => r.RoomId == selectedRoom.RoomId);
                        if (room != null)
                        {
                            room.RoomNumber = txtRoomNumber.Text;
                            room.RoomDetailDescription = txtDescription.Text;
                            room.RoomMaxCapacity = int.TryParse(txtCapacity.Text, out int cap) ? cap : null;
                            room.RoomTypeId = int.TryParse(txtTypeId.Text, out int typeId) ? typeId : 1;
                            room.RoomPricePerDay = decimal.TryParse(txtPrice.Text, out decimal price) ? price : null;

                            db.SaveChanges();
                            LoadData();
                            MessageBox.Show("Cập nhật thành công!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}");
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (LvRooms.SelectedItem is RoomInformation selectedRoom)
            {
                try
                {
                    using (var db = new FuminiHotelManagementContext())
                    {
                        var hasBooking = db.BookingDetails.Any(b => b.RoomId == selectedRoom.RoomId);

                        var room = db.RoomInformations.FirstOrDefault(r => r.RoomId == selectedRoom.RoomId);
                        if (room != null)
                        {
                            if (hasBooking)
                            {
                                // Soft delete
                                room.RoomStatus = 0; // 0 = inactive/hidden
                                db.SaveChanges();
                                MessageBox.Show("Phòng đã từng được đặt. Trạng thái đã được chuyển sang ngừng hoạt động!");
                            }
                            else
                            {
                                // Hard delete
                                db.RoomInformations.Remove(room);
                                db.SaveChanges();
                                MessageBox.Show("Xoá phòng thành công khỏi cơ sở dữ liệu!");
                            }
                            LoadData();
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
}
