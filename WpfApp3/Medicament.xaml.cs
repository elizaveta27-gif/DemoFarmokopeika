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
    /// Логика взаимодействия для Medicament.xaml
    /// </summary>
    public partial class Medicament : Window
    {
        public ObservableCollection<MEDICAMENT> Employees;
        public Medicament()
        {
            InitializeComponent();
            //DgridMedicament.ItemsSource = FARMOKAIPKAEntities.GetContext().MEDICAMENTs.ToList();
            //var dbContext = FARMOKAIPKAEntities.GetContext();
            //var transactions = from m in dbContext.MEDICAMENTs
            //                   join a in dbContext.ATXes on m.ATX_A_ID equals a.A_ID
            //                   join mr in dbContext.MANUFACTURERs on m.MR_ID equals mr.MR_ID
            //                   select new
            //                   {
            //                       Name = m.M_NAME,
            //                       Composition = m.M_COMPOSITION,
            //                       ATX = a.NAME,
            //                       MethodUse = m.M_METHOD_USE_DOSAGE,
            //                       MR = mr.NAME
            //                    };

            var dbContext = FARMOKAIPKAEntities.GetContext();

            var transactions = from m in dbContext.MEDICAMENTs
                               join a in dbContext.ATXes on m.ATX_A_ID equals a.A_ID
                               join MS in dbContext.MEDICAMENT_has_SYMPTOMS on m.M_ID equals MS.M_ID
                               join s in dbContext.MEDICAMENT_has_SYMPTOMS on MS.S_ID equals s.S_ID

                               join med in dbContext.MEDICATIONs on s.S_ID equals med.D_ID
                               
                               select new
                               {
                                   Name = m.M_NAME,
                                   Composition = m.M_COMPOSITION,
                                   ATX = a.NAME,
                                   MethodUse = m.M_METHOD_USE_DOSAGE,
                                   o = s.SYMPTOM.S_NAME

                                   
                               };



            //var grouped =
            //    workers.GroupBy(worker => worker.Name).Select(
            //        group => new Worker { Name = group.Key, Salary = group.Sum(worker => worker.Salary) });


            foreach (var item in transactions)
            {
                DgridMedicament.Items.Add(item);

            }




        }


        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AddForm addForm = new AddForm();
            addForm.Show();
        }

        private void DgridMedicament_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
