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
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using Exception = System.Exception;
using System.Configuration;

namespace Course_Project
{
    public partial class AuthorizationWindow : Window
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public AuthorizationWindow()
        {
            InitializeComponent();
        }
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow();
            registration.ShowDialog();
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text == "" || password.Password=="")
            {
                MessageBox.Show("Заполните все поля");
            }
            else LoginAccount(login.Text, RegistrationWindow.Encrypt(password.Password, "kursach"));
        }
        private void LoginAccount(string log, string pass)
        {
            if (MasterAccount(log,pass))
             {
                 MasterWindow masterWindow = new MasterWindow(login.Text);
                 masterWindow.Show();
                 this.Close();
             }
             else if (CustomerAccount(log, pass))
             {
                 CustomerWindow customerWindow = new CustomerWindow(login.Text);
                 customerWindow.Show();
                 this.Close();
             }
             else if (AdminAccount(log, pass) || AdminAccount(login.Text,password.Password))
             {
                 AdminWindow adminWindow = new AdminWindow();
                 adminWindow.Show();
                 this.Close();
             }
             else MessageBox.Show("Неправильное имя пользователя или пароль");
        }

        private bool AdminAccount(string login, string password)
        {
            string sqlExpression = $"select Login, Password from Admin";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (login == reader.GetString(0) && password == reader.GetString(1))
                            return true;
                    }
                }
                reader.Close();
                return false;
            }
        }
        private bool MasterAccount(string login, string password)
        {
            string sqlExpression = $"select Login, Password from Masters";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (login == reader.GetString(0) && password == reader.GetString(1))
                            return true;
                    }
                }
                reader.Close();
                return false;
            }
        }
        private bool CustomerAccount(string login, string password)
        {
            string sqlExpression = $"select Login, Password from Customers";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (login == reader.GetString(0) && password == reader.GetString(1))
                            return true;
                    }
                }
                reader.Close();
                return false;
            }
        }
    }
}
