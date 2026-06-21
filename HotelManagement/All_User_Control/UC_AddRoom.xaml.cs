using System;
using System.Collections.Generic;
using System.Data;
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

namespace HotelManagement.All_User_Control
{
    /// <summary>
    /// Interaction logic for UC_AddRoom.xaml
    /// </summary>
    public partial class UC_AddRoom : UserControl
    {
        function fn = new function();
        String query;
        public UC_AddRoom()
        {
            InitializeComponent();
        }
        private void UC_AddRoom_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataGrid(); 
        }
        private void LoadDataGrid()
        {
            query = "select * from rooms";
            DataSet ds = fn.getData(query);
            dataGridView.ItemsSource = ds.Tables[0].DefaultView;
        }

        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            if(txtRoomNo.Text != "" && txtRoomType.Text != "" && txtBed.Text != "" && txtPrice.Text != "")
            {
                String roomno = txtRoomNo.Text;
                String type = txtRoomType.Text;
                String bed = txtBed.Text;
                Int64 price = Int64.Parse(txtPrice.Text);


                query = "insert into rooms(roomNo,roomType, bed, price) values('"+ roomno +"','"+ type +"','"+ bed +"','"+ price + "')";
                fn.setData(query, "Đã thêm phòng");


                LoadDataGrid();
                clearAll();
            }
        }

        private void clearAll()
        {
            txtRoomNo.Clear();
            txtRoomType.SelectedIndex = -1;
            txtBed.SelectedIndex = -1;
            txtPrice.Clear();
        }
    }
}
