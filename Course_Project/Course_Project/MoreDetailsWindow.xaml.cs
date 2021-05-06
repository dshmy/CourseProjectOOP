using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для MoreDetailsWindow.xaml
    /// </summary>
    public partial class MoreDetailsWindow : Window
    {
        public MoreDetailsWindow()
        {
            InitializeComponent();
        }
        public MoreDetailsWindow(Info info)
        {
            InitializeComponent();
            producer.Content = $"Производитель: {info.Producer}";
            model.Content = $"Модель ноутбука: {info.Model}";
            service.Content = $"Тип услуги: {info.Service}";
            description.Text = $"{info.Description}";
            
                MemoryStream strmImg = new MemoryStream(info.Photo);
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = strmImg;
                myBitmapImage.DecodePixelWidth = 280;
                myBitmapImage.DecodePixelHeight = 265;
                myBitmapImage.EndInit();
                photo.Source = myBitmapImage;
            

            price.Content = $"Стоимость услуги: {info.Price} руб.";
        }
    }
}
