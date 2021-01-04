using DocumentFormat.OpenXml.ExtendedProperties;
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

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        //public static string word;
        public static bool flag;
        public MainPage()
        {
            InitializeComponent();
           
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string word = wordFind.Text;
            var form = new Medicament(word);
            form.Show();
            flag = false;
   

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            flag = true;
            var form = new Medicament();
            form.Show();
        }
    }
}
