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
    /// Логика взаимодействия для AddAtx.xaml
    /// </summary>
    public partial class AddAtx : Window
    {
        public static FARMOKAIPKAEntities dbContext = new FARMOKAIPKAEntities();
        public AddAtx(FARMOKAIPKAEntities entities)
        {
            InitializeComponent();
            
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Grid_Initialized(object sender, EventArgs e)
        {

        }
    }
}
