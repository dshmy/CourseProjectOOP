using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
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
    /// Логика взаимодействия для MasterWindow.xaml
    /// </summary>
    public partial class MasterWindow : Window
    {
        public BindingList<Info> accepted = new BindingList<Info>();
        public BindingList<Info> notAccepted = new BindingList<Info>();
        public BindingList<Info> ready = new BindingList<Info>();
        Info info;
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string strlogin;
        int id; 
        public MasterWindow()
        {
            InitializeComponent();
        }
        public MasterWindow(string log)
        {
            InitializeComponent();
            strlogin = log;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            readyButt.Height = 50;
            pickOrderButt.Height = 0;
            masterName.Content = MasterName(strlogin);
            try
            {
                AcceptedOrders();
                acceptedOrders.ItemsSource = accepted;
            }
            catch (Exception) { }
            try
            {
                NotAcceptedOrders();
                notAcceptedOrders.ItemsSource = notAccepted;
            }
            catch (Exception) { }
            try
            {
                ReadyOrders();
                readyOrders.ItemsSource = ready;
            }
            catch (Exception) { }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void TabItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            readyButt.Height = 50;
            pickOrderButt.Height = 0;
        }

        private void TabItem_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            readyButt.Height = 0;
            pickOrderButt.Height = 50;
        }

        private void TabItem_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            readyButt.Height = 0;
            pickOrderButt.Height = 0;
        }
        private void pickOrderButt_Click(object sender, RoutedEventArgs e)
        {
            if (notAcceptedOrders.SelectedIndex >= 0)
            {
                if (notAccepted[notAcceptedOrders.SelectedIndex].Price == 0 ||
                    notAccepted[notAcceptedOrders.SelectedIndex].Service == "Другое")
                {
                    PickOrder pickOrder = new PickOrder(notAccepted[notAcceptedOrders.SelectedIndex], this.id);
                    pickOrder.ShowDialog();
                    accepted.Clear();
                    notAccepted.Clear();
                    ready.Clear();
                    try
                    {
                        AcceptedOrders();
                        acceptedOrders.ItemsSource = accepted;
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        NotAcceptedOrders();
                        notAcceptedOrders.ItemsSource = notAccepted;
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        ReadyOrders();
                        readyOrders.ItemsSource = ready;
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    AddOrderInAccepted(notAccepted[notAcceptedOrders.SelectedIndex].Order_id, id);
                    accepted.Add(notAccepted[notAcceptedOrders.SelectedIndex]);
                    notAccepted.RemoveAt(notAcceptedOrders.SelectedIndex);
                }
            } else MessageBox.Show("Выберите нужный заказ");
        }
        private void AddOrderInAccepted(int order_id, int master_id)
        {
            string sqlExpression = "addInAcceptedOrders";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter orderIdParam = new SqlParameter
                {
                    ParameterName = "@order_id",
                    Value = order_id
                };
                command.Parameters.Add(orderIdParam);
                SqlParameter masterIdParam = new SqlParameter
                {
                    ParameterName = "@master_id",
                    Value = master_id
                };
                command.Parameters.Add(masterIdParam);
                var result = command.ExecuteScalar();
            }
        }
        private void readyButt_Click(object sender, RoutedEventArgs e)
        {
            if (acceptedOrders.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите нужный заказ");
            }
            else
            {
                NewNumberOfCompletedOrders(CurrentNumberOfCompletedOrders());
                NewIncome(CurrentIncome(), accepted[acceptedOrders.SelectedIndex].Price);
                AddOrderInReady(accepted[acceptedOrders.SelectedIndex].Order_id);
                ready.Add(accepted[acceptedOrders.SelectedIndex]);
                accepted.RemoveAt(acceptedOrders.SelectedIndex);
            }
        }
        private void editPassword_Click(object sender, RoutedEventArgs e)
        {
            EditMasterPasswordWindow editMasterPasswordWindow = new EditMasterPasswordWindow(strlogin);
            editMasterPasswordWindow.ShowDialog();
        }
        private void leaveAccount_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow();
            authorizationWindow.Show();
            this.Close();
        }

        private string MasterName(string log)
        {
            string sqlExpression = "MasterName";
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
                        int id = reader.GetInt32(0);
                        string Login = reader.GetString(1);
                        string Name = reader.GetString(2);
                        string Surname = reader.GetString(3);
                        if (Login == log)
                        {
                            this.id = id;
                            return $"{Surname} {Name[0]}.";
                        }
                    }
                }
                reader.Close();
                return "";
            }
        }
        private void AcceptedOrders()
        {
            string sqlExpression = "acceptedOrders";
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
                        int order_id = reader.GetInt32(0);
                        int master_id = reader.GetInt32(1);
                        string name = reader.GetString(2);
                        int price = reader.GetInt32(3);
                        string service = reader.GetString(4);
                        string producer = reader.GetString(5);
                        string model = reader.GetString(6);
                        string description = reader.GetString(7);
                        byte[] photo = (byte[])reader.GetValue(8);
                        bool state = reader.GetBoolean(9);
                        if (master_id==this.id)
                        {
                            info = new Info(order_id, name, price, service, producer, model, description, photo);
                            accepted.Add(info);
                        }
                    }
                }
                reader.Close();
            }
        }
        private void NotAcceptedOrders()
        {
            string sqlExpression = "notAcceptedOrders";
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
                        int order_id = reader.GetInt32(0);
                        string name = reader.GetString(2);
                        int price = reader.GetInt32(3);
                        string service = reader.GetString(4);
                        string producer = reader.GetString(5);
                        string model = reader.GetString(6);
                        string description = reader.GetString(7);
                        byte[] photo = (byte[])reader.GetValue(8);
                        bool state = reader.GetBoolean(9);
                        info = new Info(order_id, name, price, service,producer,model, description,photo);
                        notAccepted.Add(info);
                    }
                }
                reader.Close();
            }
        }
        private void ReadyOrders()
        {
            string sqlExpression = "readyOrders";
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
                        int order_id = reader.GetInt32(0);
                        string name = reader.GetString(2);
                        int price = reader.GetInt32(3);
                        string service = reader.GetString(4);
                        string producer = reader.GetString(5);
                        string model = reader.GetString(6);
                        string description = reader.GetString(7);
                        byte[] photo = (byte[])reader.GetValue(8);
                        if (this.id == reader.GetInt32(1))
                        {
                            info = new Info(order_id, name, price, service, producer, model, description, photo);
                            ready.Add(info);
                        }
                    }
                }
                reader.Close();
            }
        }
       
        private void AddOrderInReady(int order_id)
        {
            string sqlExpression = "addInReadyOrders";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter orderIdParam = new SqlParameter
                {
                    ParameterName = "@order_id",
                    Value = order_id
                };
                command.Parameters.Add(orderIdParam);
                var result = command.ExecuteScalar();
            }
        }

        private int CurrentIncome()
        {
            string sqlExpression = $"select Income from Masters where Master_id={this.id}";
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

        private void NewIncome(int currentIncome, int income)
        {
            string sqlExpression = $"update Masters set Income={currentIncome+income} where Master_id={this.id}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var result = command.ExecuteNonQuery();
            }
        }
        private int CurrentNumberOfCompletedOrders()
        {
            string sqlExpression = $"select NumberOfCompletedOrders from Masters where Master_id={this.id}";
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

        private void NewNumberOfCompletedOrders(int currentNum)
        {
            string sqlExpression = $"update Masters set NumberOfCompletedOrders={currentNum + 1} where Master_id={this.id}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                var result = command.ExecuteNonQuery();
            }
        }
        private void moreNotAccepted_Click(object sender, RoutedEventArgs e)
        {
            if (notAcceptedOrders.SelectedIndex < 0)
            {

            }
            else
            {
                MoreDetailsWindow moreDetailsWindow = new MoreDetailsWindow(notAccepted[notAcceptedOrders.SelectedIndex]);
                moreDetailsWindow.Show();
            }
        }

        private void moreAccepted_Click(object sender, RoutedEventArgs e)
        {
            if (acceptedOrders.SelectedIndex < 0)
            {

            }
            else
            {
                MoreDetailsWindow moreDetailsWindow = new MoreDetailsWindow(accepted[acceptedOrders.SelectedIndex]);
                moreDetailsWindow.Show();
            }
        }

        private void moreReady_Click(object sender, RoutedEventArgs e)
        {
            if (readyOrders.SelectedIndex < 0)
            {

            }
            else
            {
                MoreDetailsWindow moreDetailsWindow = new MoreDetailsWindow(ready[readyOrders.SelectedIndex]);
                moreDetailsWindow.Show();
            }
        }

        private void MasterWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                acceptedOrders.Height = masterWnd.ActualHeight - 70;
                notAcceptedOrders.Height = masterWnd.ActualHeight - 70;
                readyOrders.Height = masterWnd.ActualHeight - 70;
            }catch(Exception){}
        }
    }
}
