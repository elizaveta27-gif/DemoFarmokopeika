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
using BisnessLogical;
namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для registration.xaml
    /// </summary>
    public partial class registration : Window
    {
        FARMOKAIPKAEntities dbcontext = new FARMOKAIPKAEntities();
        List<string> listsAccess = new List<string>();
        public registration()
        {
            InitializeComponent();
            listAccess.ItemsSource = FARMOKAIPKAEntities.GetContext().Access_Seller.ToList();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var idAccess = dbcontext.Sellers.FirstOrDefault(s => s.Access_Seller.NAME == listAccess.Text).AS_ID;
                Seller seller = new Seller()
                {
                    S_FIRST_NAME = TBName.Text,
                    S_LAST_NAME = TBSurname.Text,
                    S_PATRONYMIC = TBLastName.Text,
                    S_POST = TBpost.Text,
                    S_PHONE = TBPhone.Text,
                    S_ADRESS = TBAdress.Text,
                    S_PASSWORD = TBPassword.Text,
                    S_LOGIN = TBLogin.Text,
                    AS_ID = idAccess

                };


                dbcontext.Sellers.Add(seller);
                dbcontext.SaveChanges();
                MessageBox.Show("Информация сохранена");
                this.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show("Введите все данные");
            }
           
          
        }
    }
}
