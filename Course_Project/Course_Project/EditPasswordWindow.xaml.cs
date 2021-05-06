using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Course_Project
{
    /// <summary>
    /// Логика взаимодействия для EditPasswordWindow.xaml
    /// </summary>
    public partial class EditPasswordWindow : Window
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        string strlogin;
        public EditPasswordWindow()
        {
            InitializeComponent();
        }
        public EditPasswordWindow(string str)
        {
            strlogin = str;
            InitializeComponent();
        }
        
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if(newPassword.Password.Length<=5)
            {
                newPassword.Background= new SolidColorBrush(Color.FromRgb(219, 88, 86));
            }
            if (checkOldPassword(strlogin, RegistrationWindow.Encrypt(oldPassword.Password, "kursach")) && newPassword.Password.Length>5)
            { 
                EditPassword(strlogin, RegistrationWindow.Encrypt(oldPassword.Password, "kursach"), RegistrationWindow.Encrypt(newPassword.Password, "kursach"));
                MessageBox.Show("Пароль успешно изменён.");
                this.Close();
            }
            else
            {
                 oldPassword.Background= new SolidColorBrush(Color.FromRgb(219, 88, 86));
            }
        }
        private static void EditPassword(string log, string oldpass, string newpass)
        {
            string sqlExpression = "editPassword";
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
                SqlParameter oldpasswordParam = new SqlParameter
                {
                    ParameterName = "@oldPassword",
                    Value = oldpass
                };
                command.Parameters.Add(oldpasswordParam);

                SqlParameter newpasswordParam = new SqlParameter
                {
                    ParameterName = "@newPassword",
                    Value = newpass
                };
                command.Parameters.Add(newpasswordParam);

                var result = command.ExecuteScalar();
            }
        }
        private bool checkOldPassword(string log, string oldPass)
        {
            string sqlExpression = "checkOldPasswordCustomer";
            bool flag = false;
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
                        string Password = reader.GetString(1);
                        if (Login == log && Password==oldPass)
                        {
                            return true;
                        }
                    }
                    flag = false;

                }
                reader.Close();
                return flag;
            }
        }
        

        
    }
}
