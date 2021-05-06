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
    /// Логика взаимодействия для EditPasswordAdminWindow.xaml
    /// </summary>
    public partial class EditPasswordAdminWindow : Window
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public EditPasswordAdminWindow()
        {
            InitializeComponent();
        }
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (newPassword.Password.Length <= 5)
            {
                newPassword.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
            }
            if ((checkOldPassword( RegistrationWindow.Encrypt(oldPassword.Password, "kursach"))|| checkOldPassword(oldPassword.Password)) && newPassword.Password.Length >= 5)
            {
                EditPassword(  RegistrationWindow.Encrypt(newPassword.Password, "kursach"));
                MessageBox.Show("Пароль успешно изменён.");
                this.Close();
            }
            else
            {
                oldPassword.Background = new SolidColorBrush(Color.FromRgb(219, 88, 86));
            }
        }
        private static void EditPassword(string newpass)
        {
            string sqlExpression = $"update Admin set Password='{newpass}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int reader = command.ExecuteNonQuery();
            }
        }
        private bool checkOldPassword(string oldPass)
        {
            string sqlExpression = $"select * from Admin";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (oldPass == reader.GetString(2))
                            return true;
                    }
                }

                reader.Close();
            }

            return false;
        }
    }
}
