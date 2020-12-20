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
    /// Логика взаимодействия для Medicament.xaml
    /// </summary>
    public partial class Medicament : Window
    {
        public Medicament()
        {
            InitializeComponent();
            DgridMedicament.ItemsSource = FARMOKAIPKAEntities.GetContext().MEDICAMENTs.ToList();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddForm addForm = new AddForm();
            addForm.Show();
        }
    }
}
