using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Course_Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public BindingList<InfoAboutOrders> Accepted = new BindingList<InfoAboutOrders>();
        public BindingList<InfoAboutOrders> NotAccepted = new BindingList<InfoAboutOrders>();
        public BindingList<InfoAboutOrders> Ready = new BindingList<InfoAboutOrders>();
        public BindingList<InfoAboutMasters> Masters = new BindingList<InfoAboutMasters>();
        private InfoAboutMasters infoAboutMasters;
        private InfoAboutOrders infoAboutOrders;
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public AdminWindow()
        {
            InitializeComponent();
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void leaveAccount_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }

        private void addMaster_Click(object sender, RoutedEventArgs e)
        {
            NewMasterWindow newMasterWindow= new NewMasterWindow();
            newMasterWindow.ShowDialog();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void AcceptedOrders()
        {
            string sqlExpression = $"select Orders.Order_id, Orders.Master_id, Masters.Name, Masters.Surname, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, " +
                                   $"Orders.Model, Orders.Description_of_problem from Orders Inner Join  Customers on Customers.Customer_id = Orders.Customer_id " +
                                   $"inner join TypesOfServices on Orders.Services_id = TypesOfServices.Service_id inner join Masters " +
                                   $"on Orders.Master_id = Masters.Master_id where Orders.Master_id is not null and Orders.State=0";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) 
                {
                    while (reader.Read())
                    {
                        int order_id = reader.GetInt32(0);
                        int master_id = reader.GetInt32(1);
                        string masterName = reader.GetString(2);
                        string masterSurname = reader.GetString(3);
                        string customerName = reader.GetString(4);
                        int price = reader.GetInt32(5);
                        string typeOfServices = reader.GetString(6);
                        string producer = reader.GetString(7);
                        string model = reader.GetString(8);
                        string description = reader.GetString(9);
                        infoAboutOrders= new InfoAboutOrders(order_id, master_id, masterName, masterSurname, customerName,
                            price, typeOfServices, producer, model,description);
                        Accepted.Add(infoAboutOrders);
                    }
                }

                reader.Close();
            }
        }
        private void NotAcceptedOrders()
        {
            string sqlExpression = $"select Orders.Order_id, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, " +
                                   $"Orders.Model, Orders.Description_of_problem from Orders Inner Join  Customers on Customers.Customer_id = Orders.Customer_id " +
                                   $"inner join TypesOfServices on Orders.Services_id = TypesOfServices.Service_id where Orders.Master_id is null";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int order_id = reader.GetInt32(0);
                        string customerName = reader.GetString(1);
                        int price = reader.GetInt32(2);
                        string typeOfServices = reader.GetString(3);
                        string producer = reader.GetString(4);
                        string model = reader.GetString(5);
                        string description = reader.GetString(6);
                        infoAboutOrders = new InfoAboutOrders(order_id,  customerName, price, typeOfServices, producer, model, description);
                        NotAccepted.Add(infoAboutOrders);
                    }
                }

                reader.Close();
            }
        }
        private void ReadyOrders()
        {
            string sqlExpression = $"select Orders.Order_id, Orders.Master_id, Masters.Name, Masters.Surname, Customers.Name, TypesOfServices.Price, TypesOfServices.Service, Orders.Producer, " +
                                   $"Orders.Model, Orders.Description_of_problem from Orders Inner Join  Customers on Customers.Customer_id = Orders.Customer_id " +
                                   $"inner join TypesOfServices on Orders.Services_id = TypesOfServices.Service_id inner join Masters " +
                                   $"on Orders.Master_id = Masters.Master_id where Orders.Master_id is not null and Orders.State=1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int order_id = reader.GetInt32(0);
                        int master_id = reader.GetInt32(1);
                        string masterName = reader.GetString(2);
                        string masterSurname = reader.GetString(3);
                        string customerName = reader.GetString(4);
                        int price = reader.GetInt32(5);
                        string typeOfServices = reader.GetString(6);
                        string producer = reader.GetString(7);
                        string model = reader.GetString(8);
                        string description = reader.GetString(9);
                        infoAboutOrders = new InfoAboutOrders(order_id, master_id, masterName, masterSurname, customerName,
                            price, typeOfServices, producer, model, description);
                        Ready.Add(infoAboutOrders);
                    }
                }

                reader.Close();
            }
        }

        private void ListOfMasters()
        {
            string sqlExpression = $"select * from Masters";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int master_id = reader.GetInt32(0);
                        string masterName = reader.GetString(1);
                        string masterSurname = reader.GetString(2);
                        string masterLogin = reader.GetString(3);
                        int numOforders = reader.GetInt32(5);
                        int income = reader.GetInt32(6);
                        infoAboutMasters= new InfoAboutMasters(master_id, masterLogin,masterName, masterSurname,numOforders,income);
                        Masters.Add(infoAboutMasters);
                    }
                }

                reader.Close();
            }
        }

        private int IncomeOutput()
        {
            string sqlExpression = $"select sum(Income) from Masters";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
                reader.Close();
            }
            return 0;
        }
        private int NumberOfCompletedOrdersOutput()
        {
            string sqlExpression = $"select sum(NumberOfCompletedOrders) from Masters";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
                reader.Close();
            }
            return 0;
        }

        private string BestEmployer()
        {
            string sqlExpression = $"select top(1) * from Masters order by Income desc";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        return $"{reader.GetString(1)} {reader.GetString(2)}";
                    }
                }
                reader.Close();
            }
            return "";
        }
       

        private void editPassword_Click_1(object sender, RoutedEventArgs e)
        {
            EditPasswordAdminWindow editPasswordAdminWindow= new EditPasswordAdminWindow();
            editPasswordAdminWindow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                reportIncome.Text = $"{IncomeOutput()} бел. руб.";
                reportOrders.Text = $"{NumberOfCompletedOrdersOutput()}";
                reportMaster.Text = BestEmployer();
                AcceptedOrders();
                acceptedOrders.ItemsSource = Accepted;
                ReadyOrders();
                readyOrders.ItemsSource = Ready;
                NotAcceptedOrders();
                notAcceptedOrders.ItemsSource = NotAccepted;
                ListOfMasters();
                listOfMasters.ItemsSource = Masters;
            }
            catch (Exception) { }
        }

        private void AdminWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                acceptedOrders.Height = adminWnd.ActualHeight - 70;
                notAcceptedOrders.Height = adminWnd.ActualHeight - 70;
                readyOrders.Height = adminWnd.ActualHeight - 70;
                listOfMasters.Height = adminWnd.ActualHeight  -70;
            }catch(Exception){}
        }

        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Masters.Clear();
            ListOfMasters();
            listOfMasters.ItemsSource = Masters;
        }
    }
}
