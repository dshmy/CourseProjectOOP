using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для NewMasterWindow.xaml
    /// </summary>
    public partial class NewMasterWindow : Window
    {
        Control brush = new Control();

        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public NewMasterWindow()
        {
            InitializeComponent();
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            if (login.Text == "" || password.Password == "" || name.Text == "" || surname.Text=="")
            {

                if (login.Text == "")
                    login.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                if (password.Password == "")
                    password.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                if (name.Text == "")
                    name.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                MessageBox.Show("Заполните все поля");
            }
            else if ((!Regex.Match(login.Text, "^[A-Za-z]+$").Success))
            {
                login.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                MessageBox.Show("Используйте буквы латинского алфавита");
            }
            else if (login.Text.ToLower().Contains("admin"))
            {
                login.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                MessageBox.Show("Нельзя использовать логин,\n включающий в себя слово Admin");
            }
            else if (!Regex.Match(name.Text, "^[A-ZА-Я]+[а-яa-z]+$").Success)
            {
                name.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                MessageBox.Show("Введите имя с большой буквы\n русского или латинского алфавита");
            }
            else if (password.Password.Length <= 5)
            {
                password.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                MessageBox.Show("Длина пароля не меньше 6 символов");
            }
            else if (!Regex.Match(surname.Text, "^[A-ZА-Я]+[а-яa-z]+$").Success)
            {
                surname.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                MessageBox.Show("Введите фамилию с большой буквы\n русского или латинского алфавита");
            }
            else
            {
                if (checkCustomer(login.Text))
                {
                    AddMaster(login.Text, RegistrationWindow.Encrypt(password.Password, "kursach"), name.Text, surname.Text);
                    MessageBox.Show("Регистрация прошла успешно");
                    this.Close();
                }
                else login.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
            }
        }
        private static void AddMaster(string log, string pass, string name, string surname)
        {
            string sqlExpression = "addMaster";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter loginParam = new SqlParameter
                {
                    ParameterName = "@login",
                    Value = log
                };
                command.Parameters.Add(loginParam);
                SqlParameter passwordParam = new SqlParameter
                {
                    ParameterName = "@password",
                    Value = pass
                };
                command.Parameters.Add(passwordParam);

                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);

                SqlParameter surnameParam = new SqlParameter
                {
                    ParameterName = "@surname",
                    Value = surname
                };
                command.Parameters.Add(surnameParam);

                var result = command.ExecuteScalar();
            }
        }
        private bool checkCustomer(string log)
        {
            login.Text = login.Text.Trim();
            string sqlExpression = "getCustomers";
            bool flag = true;
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
                        if (Login == login.Text)
                        {
                            MessageBox.Show("Этот логин уже занят");
                            reader.Close();
                            return false;
                        }
                    }
                    flag = true;

                }
                reader.Close();
                return checkMasterLogin(log);
                return flag;
            }
        }
        private bool checkMasterLogin(string log)
        {
            login.Text = login.Text.Trim();
            string sqlExpression = "getMasters";
            bool flag = true;
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
                        if (Login == login.Text)
                        {
                            MessageBox.Show("Этот логин уже занят");
                            reader.Close();
                            return false;
                        }
                    }
                    flag = true;

                }
                reader.Close();
                return flag;
            }
        }
        private void name_LostFocus(object sender, RoutedEventArgs e)
        {
            name.Text = name.Text.Trim();
            if (!Regex.Match(name.Text, "^[A-ZА-Я]+[а-яa-z]+$").Success)
                name.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
            else name.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        private void surname_LostFocus(object sender, RoutedEventArgs e)
        {
            surname.Text = surname.Text.Trim();
            if (!Regex.Match(surname.Text, "^[A-ZА-Я]+[а-яa-z]+$").Success)
                surname.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
            else surname.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        private void login_LostFocus(object sender, RoutedEventArgs e)
        {
            login.Text = login.Text.Trim();
            if (login.Text.Length < 5 || (!Regex.Match(login.Text, "^[A-Za-z]+$").Success))
                login.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
            else login.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void password_LostFocus(object sender, RoutedEventArgs e)
        {
            password.Password = password.Password.Trim();
            if (password.Password.Length <= 5)
                password.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
            else password.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}

