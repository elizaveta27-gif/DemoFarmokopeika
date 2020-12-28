using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для dataMedicament.xaml
    /// </summary>
    public partial class dataMedicament : Window
    {
        public ObservableCollection<MEDICAMENT> med;
        public dataMedicament()
        {
            InitializeComponent();
            var dbContext = FARMOKAIPKAEntities.GetContext();
            var str = "Головная боль";

            var transactions = from m in dbContext.MEDICAMENTs
                               join a in dbContext.ATXes on m.ATX_A_ID equals a.A_ID
                               join MS in dbContext.MEDICAMENT_has_SYMPTOMS on m.M_ID equals MS.M_ID
                               join s in dbContext.MEDICAMENT_has_SYMPTOMS on MS.S_ID equals s.S_ID
                               join manafactur in dbContext.MANUFACTURERs on m.MR_ID equals manafactur.MR_ID
                               join med in dbContext.MEDICATIONs on s.S_ID equals med.D_ID

                               select new
                               {
                                   Name = m.M_NAME,
                                   Composition = m.M_COMPOSITION,
                                   ATX = a.NAME,
                                   MethodUse = m.M_METHOD_USE_DOSAGE,
                                   o = s.SYMPTOM.S_NAME,
                                   manafacturer = manafactur.NAME

                               };


            foreach (var item in transactions.Where(t=>t.o.ToUpper()==str.ToUpper() || t.Name.ToUpper() == str.ToUpper()))
            {
                col.Text =item.Name;
                col2.Text = item.ATX;
                col3.Text = item.manafacturer;
                

            }
            
        }

        private void Grid_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
