using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

        public static string word { get; set; } 
        public bool flag = MainPage.flag;
        public static FARMOKAIPKAEntities dbContext = FARMOKAIPKAEntities.GetContext();
       
        public Medicament(string wordFind)
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

            word = wordFind;
            //var grouped =
            //    workers.GroupBy(worker => worker.Name).Select(
            //        group => new Worker { Name = group.Key, Salary = group.Sum(worker => worker.Salary) });

            
            var transactions = from m in dbContext.MEDICAMENTs
                                                 join a in dbContext.ATXes on m.ATX_A_ID equals a.A_ID
                                                 join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                                                 join MS in dbContext.MEDICAMENT_has_SYMPTOMS on m.M_ID equals MS.M_ID
                                                 //join s in dbContext.MEDICAMENT_has_SYMPTOMS on MS.S_ID equals s.S_ID
                                                 join d in dbContext.MEDICATIONs on MS.S_ID equals d.S_ID
                                                 //join med in dbContext.MEDICATIONs on s.S_ID equals med.D_ID
                                                 join g in dbContext.MEDICAMENTOS_has_GROUP on  m.M_ID equals g.M_ID
                                                 join gr in dbContext.Groups on g.G_ID equals gr.G_ID
                                                 //where word.ToUpper() == m.M_NAME.ToUpper() || word.ToUpper() == s.SYMPTOM.S_NAME
                                                 select new
                                                 {
                                                     Name = m.M_NAME,
                                                     Composition = m.M_COMPOSITION,
                                                     ATX = a.NAME,
                                                     MethodUse = m.M_METHOD_USE_DOSAGE,
                                                     o = MS.SYMPTOM.S_NAME,
                                                     disease = d.DISEASE.NAME,
                                                     MR = MR.NAME,
                                                     //sym = s.SYMPTOM.S_NAME,
                                                     Group = gr.NAME
                                                 };


            if (transactions.Where(t => t.Name.ToUpper() == word.ToUpper()).Count() == 0 )
            {
                MessageBox.Show("Нет");

            }
            else if(word!="all")
            {
                foreach (var item in transactions.Where(t => t.Name.ToUpper() == word.ToUpper()).Distinct())
                {

                    DgridMedicament.Items.Add(item);
                }
            }
            else
            {
                foreach (var item in transactions.Distinct())
                {
                    DgridMedicament.Items.Add(item);
                }
            }

            //MEDICAMENT mEDICAMENT = (MEDICAMENT)DgridMedicament.SelectedItem;
            //wordFind = mEDICAMENT.M_NAME;

        }


        public Medicament()
        {
            InitializeComponent();


            var transactions = from m in dbContext.MEDICAMENTs
                               join a in dbContext.ATXes on m.ATX_A_ID equals a.A_ID
                               join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                               join MS in dbContext.MEDICAMENT_has_SYMPTOMS on m.M_ID equals MS.M_ID
                               join s in dbContext.MEDICAMENT_has_SYMPTOMS on MS.S_ID equals s.S_ID
                               join d in dbContext.MEDICATIONs on s.S_ID equals d.S_ID
                               join med in dbContext.MEDICATIONs on s.S_ID equals med.D_ID
                               //where word.ToUpper() == m.M_NAME.ToUpper() || word.ToUpper() == s.SYMPTOM.S_NAME
                               select new
                               {
                                   Name = m.M_NAME,
                                   Composition = m.M_COMPOSITION,
                                   ATX = a.NAME,
                                   MethodUse = m.M_METHOD_USE_DOSAGE,
                                   o = s.SYMPTOM.S_NAME,
                                   disease = d.DISEASE.NAME,
                                   MR = MR.NAME,
                                   sym = s.SYMPTOM.S_NAME
                               };


            foreach (var item in transactions.Distinct())
            {
                DgridMedicament.Items.Add(item);
            }



        }
       

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

            var firstSelectedCellContent = this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem);
            var firstSelectedCell = firstSelectedCellContent != null ? firstSelectedCellContent.Parent : null;
            //MessageBox.Show(firstSelectedCell.ToString());
            word = ((TextBlock)firstSelectedCellContent).Text;
            dataMedicament dm = new dataMedicament(word);
            dm.Show();
        }

        private void DgridMedicament_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
