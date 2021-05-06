using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
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

namespace Course_Project
{
    /// <summary>
    /// Логика взаимодействия для PickOrder.xaml
    /// </summary>
    public partial class PickOrder : Window
    {
        private int order_id;
        private int master_id;
        List<string> PriceList = new List<string>();
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public PickOrder()
        {
            InitializeComponent();
        }
        public PickOrder(Info info, int id)
        {
            InitializeComponent();
            order_id = info.Order_id;
            master_id = id;
            producer.Content = $"Производитель: {info.Producer}";
            model.Content = $"Модель ноутбука: {info.Model}";
            description.Text = $"{info.Description}";
            
                MemoryStream strmImg = new MemoryStream(info.Photo);
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = strmImg;
                myBitmapImage.DecodePixelWidth = 280;
                myBitmapImage.DecodePixelHeight = 265;
                myBitmapImage.EndInit();
                photo.Source = myBitmapImage;
            
            FillingFields();
        }
        private void FillingFields()
        {
            string sqlExpression = "fillingFields";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string Service = reader.GetString(0);
                        string Price = Convert.ToString(reader.GetInt32(1));
                        if (reader.GetInt32(1) != 0)
                        {
                            service.Items.Add(Service);
                            PriceList.Add(Price);
                        }
                    }
                }
                reader.Close();
            }
        }

        private void service_LostFocus(object sender, RoutedEventArgs e)
        {
            if(service.SelectedIndex>-1)
                price.Content = $"Стоимость услуги: {PriceList[service.SelectedIndex]} руб.";
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            if (service.SelectedIndex < 0)
                MessageBox.Show("Выберите тип предоставляемой услуги.");
            else
            {
                Pick(order_id, service.SelectedIndex+1, master_id);
                MessageBox.Show("Заказ успешно добавлен.");
                this.Close();
            }
        }
        private static void Pick(int order, int service, int master)
        {
            string sqlExpression = "Pick";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter orderParam = new SqlParameter
                {
                    ParameterName = "@order_id",
                    Value = order
                };
                command.Parameters.Add(orderParam);
                SqlParameter serviceParam = new SqlParameter
                {
                    ParameterName = "@service_id",
                    Value = service
                };
                command.Parameters.Add(serviceParam);

                SqlParameter masterParam = new SqlParameter
                {
                    ParameterName = "@master_id",
                    Value = master
                };
                command.Parameters.Add(masterParam);

                var result = command.ExecuteScalar();
            }
        }
    }
}
