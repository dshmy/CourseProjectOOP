using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
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
    /// Логика взаимодействия для NewOrderWindow.xaml
    /// </summary>
    public partial class NewOrderWindow : Window
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string strlogin;
        string path="";
        int id;
        List<string> PriceList = new List<string>();
        BindingList<string> ModelList = new BindingList<string>();
        public NewOrderWindow()
        {
            InitializeComponent();
        }

        public NewOrderWindow(string str)
        {
            InitializeComponent();
            FillingFields();
            FillingFieldsProd();
            strlogin = str;
        }
        private void sendRequest_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            if (services.SelectedItem == null)
            {
                MessageBox.Show("Выберите услугу");
                flag = false;
            }
            else if (producer.SelectedItem == null)
            {
                MessageBox.Show("Выберите фирму вашего ноутбука");
                flag = false;
            }
            else if (this.path == "")
            {
                MessageBox.Show("Прикрепите фотографию проблемы");
                flag = false;
            }
            else if (services.SelectedIndex == services.Items.Count - 1 && description.Text == "")
            {
                MessageBox.Show("Опишите вашу проблему");
                flag = false;
            }
            else if (flag)
            {
                if (model.Text == "")
                {
                    id = CheckCustomerId(strlogin);
                    AddOrder(id, services.SelectedIndex + 1, producer.Text, "не указано", description.Text);
                    MessageBox.Show("Ваша заявка отправлена.\nОжидайте подтвержения мастера");
                    this.Close();
                }
                else
                {
                    id = CheckCustomerId(strlogin);
                    AddOrder(id, services.SelectedIndex + 1, producer.Text, model.Text, description.Text);
                    MessageBox.Show("Ваша заявка отправлена.\nОжидайте подтвержения мастера");
                    this.Close();
                }
            }
        }
        private void AddOrder(int cust_id, int services_id, string prod, string model, string descript)
        {
            string sqlExpression = $"insert into Orders(Customer_id, Services_id,  Producer, Model, Description_of_problem,State, Photo," +
                                   $" Format_of_photo)" +
                                   $"select {cust_id},{services_id},  '{prod}', '{model}', '{descript}', 0, BulkColumn, 'image/jpg'" +
                                   $"from OpenRowSet" +
                                   $"(" +
                                   $"bulk N'{path}'," +
                                   $"SINGLE_BLOB)" +
                                   $"as Файл";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
            }
        }
        private void services_LostFocus(object sender, RoutedEventArgs e)
        {
            if (services.SelectedIndex > -1)
                price.Text = PriceList[services.SelectedIndex];
            if (services.SelectedIndex == PriceList.Count - 1)
                price.Text = "";
        }
        private void producer_LostFocus(object sender, RoutedEventArgs e)
        {
            FillingFieldsModels(producer.Text);
            model.ItemsSource = ModelList;
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
                        services.Items.Add(Service);
                        PriceList.Add(Price);
                    }
                }
                reader.Close();
            }
        }
        private void FillingFieldsProd()
        {
            string sqlExpression = "fillProd";
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
                        string producers = reader.GetString(0);
                        producer.Items.Add(producers);
                    }
                }
                reader.Close();
            }
        }
        private void FillingFieldsModels(string prod)
        {
            ModelList.Clear();
            string sqlExpression = $"select Models.Producer, Models.Model from Models inner join Laptops " +
                                   $"on Laptops.Producer = Models.Producer where Models.Producer = '{prod}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ModelList.Add(reader.GetString(1));
                    }
                }
                reader.Close();
            }
        }
        private int CheckCustomerId(string log)
        {
            int id;
            string sqlExpression = "CheckCustomerId";
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
                        string Login = reader.GetString(0);
                        if (Login == log)
                        {
                            id = reader.GetInt32(1);
                            return id;
                        }
                    }
                }
                reader.Close();
                return 0;
            }
        }
        

        private void addPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JPG Format (*.jpg)|*.jpg";
            if (openFile.ShowDialog() == true)
            {
                path = openFile.FileName;
                photo.Source = BitmapFrame.Create(new Uri(openFile.FileName));
            }
        }
    }
}
