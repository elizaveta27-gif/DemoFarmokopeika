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
    /// Логика взаимодействия для cartUI.xaml
    /// </summary>
    public partial class cartUI : Window
    {
        public List<MEDICAMENT> listMed;
        public cartUI()
        {
            InitializeComponent();
            //добавить исключение
            listMed = MainPage.cart.GetAll();


            var tr = from f in listMed
                     group f by f.M_NAME into g
                     select new
                     {
                         name = g.Key,
                         Cout = g.Count(),
                         
                         
                     };

            var trans = listMed.GroupBy(p => p.M_NAME)
                        .Select(g => new
                        {
                            Name = g.Key,
                            Count = g.Count(),
                            price = g.Select(p => p.M_PRICE)
                        });

            foreach (var item in trans)
            {
                ListMedicament.Items.Add(item);
              
            }
            
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
