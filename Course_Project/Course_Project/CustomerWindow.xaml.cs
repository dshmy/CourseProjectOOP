using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string strlogin;
        private int currentPage = 1;
        private MyOrders myOrders;
        Info info;
        List<MyOrders> Orders= new List<MyOrders>();
        private int id;
        public CustomerWindow()
        {
            InitializeComponent();
        }
        public CustomerWindow(string str)
        {
            InitializeComponent();
            strlogin = str;
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Name.Content = CustomerName(strlogin);
            Notifications();
            CheckNotification();
            MyOrders();
            if (Orders.Count > 0)
            {
                r1.Visibility = Visibility.Hidden;
                r2.Visibility = Visibility.Hidden;
                pages.Text = $"{currentPage}/{Orders.Count}";
                MasterSurname.Text = Orders[0].MasterSurname;
                typeOfService.Text = Orders[0].TypeOfService;
                price.Text = $"{Orders[0].Price} бел. руб.";
                MemoryStream strmImg = new MemoryStream(Orders[0].Photo);
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = strmImg;
                myBitmapImage.DecodePixelWidth = 265;
                myBitmapImage.DecodePixelHeight = 220;
                myBitmapImage.EndInit();
                photo.Source = myBitmapImage;
            }
            else
            {
                r1.Visibility = Visibility.Visible;
                r2.Visibility = Visibility.Visible;
            }
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void newOrder_Click(object sender, RoutedEventArgs e)
        {
            NewOrderWindow newOrderWindow = new NewOrderWindow(strlogin);
            newOrderWindow.ShowDialog();
        }

        private void passwordEdit_Click(object sender, RoutedEventArgs e)
        {
            EditPasswordWindow editPasswordWindow = new EditPasswordWindow(strlogin);
            editPasswordWindow.ShowDialog();
        }

        private void leaveAccount_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }
        private string CustomerName(string log)
        {
            string sqlExpression = "CustomerName";
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
                        string Login = reader.GetString(1);
                        string Name = reader.GetString(2);
                        if (Login == log)
                        {
                            this.id = reader.GetInt32(0);
                            return Name;
                        }
                    }
                }
                reader.Close();
                return "";
            }
        }
        private void MyOrders()
        {
            string sqlExpression = $"select Masters.Surname, TypesOfServices.Service, TypesOfServices.Price, Orders.Photo " +
                                   $"from Orders inner join Masters on Orders.Master_id = Masters.Master_id inner join TypesOfServices " +
                                   $"on TypesOfServices.Service_id = Orders.Services_id where Orders.Customer_id = {this.id} and Orders.State=0";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string surname = reader.GetString(0);
                        string service = reader.GetString(1);
                        int price = reader.GetInt32(2);
                        byte[] photo = (byte[])reader.GetValue(3);
                        myOrders= new MyOrders(surname, service, price, photo);
                        Orders.Add(myOrders);
                    }
                }

                reader.Close();
            }
        }
        private void Notifications()
        {
            string sqlExpression = $"select Masters.Surname, TypesOfServices.Service from Masters inner join Orders " +
                                   $"on Masters.Master_id=Orders.Master_id inner join TypesOfServices " +
                                   $"on Orders.Services_id=TypesOfServices.Service_id where Orders.IsCheck=0 and Orders.Customer_id={this.id} and State=1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MessageBox.Show($"Мастер {reader.GetString(0)} выполнил ваш заказ: {reader.GetString(1)}", "Ваш заказ готов",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                reader.Close();
            }
        }

        private void CheckNotification()
        {
            string sqlExpression = $"update Orders set IsCheck=1 where Customer_id={this.id} and State=1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var result = command.ExecuteNonQuery();
            }
        }
        private void left_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1 && currentPage <= Orders.Count)
            {
                currentPage--;
                pages.Text = $"{currentPage}/{Orders.Count}";
                MasterSurname.Text = Orders[currentPage-1].MasterSurname;
                typeOfService.Text = Orders[currentPage-1].TypeOfService;
                price.Text = $"{Orders[currentPage-1].Price} бел. руб.";
                MemoryStream strmImg = new MemoryStream(Orders[currentPage-1].Photo);
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = strmImg;
                myBitmapImage.DecodePixelWidth = 265;
                myBitmapImage.DecodePixelHeight = 220;
                myBitmapImage.EndInit();
                photo.Source = myBitmapImage;
            }
        }
        private void right_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage >= 1 && currentPage < Orders.Count)
            {
                currentPage++;
                pages.Text = $"{currentPage}/{Orders.Count}";
                MasterSurname.Text = Orders[currentPage - 1].MasterSurname;
                typeOfService.Text = Orders[currentPage - 1].TypeOfService;
                price.Text = $"{Orders[currentPage - 1].Price} бел. руб.";
                MemoryStream strmImg = new MemoryStream(Orders[currentPage - 1].Photo);
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = strmImg;
                    myBitmapImage.DecodePixelWidth = 265;
                    myBitmapImage.DecodePixelHeight = 220;
                    myBitmapImage.EndInit();
                    photo.Source = myBitmapImage;
                
            }
        }
        
    }
}
