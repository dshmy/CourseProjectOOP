using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        Control brush= new Control();
        
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public RegistrationWindow()
        {
            InitializeComponent();
        }
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            
                if (login.Text == "" || password.Password == "" || name.Text == "")
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
                else if(!Regex.Match(name.Text, "^[A-ZА-Я]+[а-яa-z]+$").Success)
                {
                    name.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                    MessageBox.Show("Введите имя с большой буквы\n русского или латинского алфавита");
                }
                else if(password.Password.Length<=5)
                {
                    password.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                    MessageBox.Show("Длина пароля не меньше 6 символов");
                }
                else 
                {
                    if (checkCustomer(login.Text))
                    {
                        AddCustomer(login.Text, Encrypt(password.Password, "kursach"), name.Text);
                        MessageBox.Show("Регистрация прошла успешно");
                        this.Close();
                    }
                    else login.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
                }
            
            
        }
        public static string Encrypt(string plainText, string password,
            string salt = "Kosher", string hashAlgorithm = "SHA1",
            int passwordIterations = 2, string initialVector = "OFRna73m*aze01xY",
            int keySize = 256)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            PasswordDeriveBytes derivedPassword = new PasswordDeriveBytes(password, saltValueBytes, hashAlgorithm, passwordIterations);
            byte[] keyBytes = derivedPassword.GetBytes(keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmetricKey.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }
        private static void AddCustomer(string log, string pass, string name)
        {
            string sqlExpression = "addCustomer";
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
                name.Background= new SolidColorBrush(Color.FromRgb(219, 88, 86));
            else name.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void login_LostFocus(object sender, RoutedEventArgs e)
        {
            login.Text = login.Text.Trim();
            if (login.Text.Length<5 || (!Regex.Match(login.Text, "^[A-Za-z]+$").Success))
                login.Background= new SolidColorBrush(Color.FromRgb(219, 88, 86));
            else login.Background= new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        private void password_LostFocus(object sender, RoutedEventArgs e)
        {
            password.Password = password.Password.Trim();
            if(password.Password.Length<=5)
                password.Background= new SolidColorBrush(Color.FromRgb(219, 88, 86));
            else password.Background= new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
