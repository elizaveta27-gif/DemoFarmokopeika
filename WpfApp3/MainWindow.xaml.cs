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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BisnessLogical;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow : Window
    {
        public static string NameSeller;

        public static Seller seller;
        public static string access;
        public MainWindow()
        {
             
            InitializeComponent();

            using (var ctx = new FARMOKAIPKAEntities())
            {
                ctx.Database.Initialize(true);
            }
        }
        

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MinButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            } 
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FARMOKAIPKAEntities dbContext = FARMOKAIPKAEntities.GetContext();
            string Login = login.Text;
            //string Password = password.Text;
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Введите логин");

            }else if (string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Введите пароль");
            }
            else
            {
                var transaction = from user in dbContext.Sellers
                                  where user.S_LOGIN == Login && user.S_PASSWORD == password.Text
                                  select new
                                  {
                                      name = user.S_FIRST_NAME,
                                      last_name = user.S_LAST_NAME,
                                      patranymic = user.S_PATRONYMIC,
                                      login = user.S_LOGIN,
                                      password = user.S_PASSWORD
                                  };


                foreach (var item in transaction)
                {
                    NameSeller = $"{item.last_name} {item.name} {item.patranymic}";

                }

                if (transaction.Count() == 0)
                {
                    MessageBox.Show("Неверно введен логин или пароль");

                }
                else
                {

                    var sel = dbContext.Sellers.Where(s => s.S_LOGIN == Login);
                    foreach (var item in sel)
                    {
                        seller = item;
                    }
                    MessageBox.Show("Пользователь авторизовался");
                    access = seller.Access_Seller.NAME;
                    var MainPage = new MainPage();
                    MainPage.Show();
                    this.Close();
                }

            }



        }
    }
}
