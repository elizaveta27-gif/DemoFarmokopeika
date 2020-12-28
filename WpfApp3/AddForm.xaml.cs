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
    /// Логика взаимодействия для AddForm.xaml
    /// </summary>
    public partial class AddForm : Window    
    {
        private MEDICAMENT medicament = new MEDICAMENT();
        public AddForm()
        {
            InitializeComponent();
            DataContext = medicament;
            using (FARMOKAIPKAEntities db = new FARMOKAIPKAEntities())
            {
                var medicaments = db.MEDICAMENTs.Join(db.ATXes,
                    m => m.ATX_A_ID,
                    a => a.A_ID,
                    (m, a) => new
                    {
                        Name = m.M_NAME,
                        Composition = m.M_COMPOSITION,
                        pharmacologicalAction = m.M_PHARMACOLOGICAL__ACTION,
                        methodUseDosage = m.M_METHOD_USE_DOSAGE,
                        drugInteractions = m.M_DRUG_INTERACTIONS,
                        specificInduction = m.M_SPECIFIC_INDUCTION,
                        storageConditions = m.M_STORAGE_CONDITIONS,
                        expityDate = m.M_EXPITY_DATE,
                        availability_pr = m.M_AVAILABILITY_PRESCRIPTIONS,
                        appearance = m.M_APPEARANCE,
                        overdose = m.M_OVERDOSE,
                        mrId = m.MR_ID,
                        atx= a.A_ID
                        
                    });
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            //if(string.IsNullOrWhiteSpace(medicament.M_NAME))
            //{
            //    errors.AppendLine("Введите название лекарства");
            //}
            //if (string.IsNullOrWhiteSpace(medicament.M_COMPOSITION))
            //{
            //    errors.AppendLine("Введите название состав лекарства");
            //}
            //if (string.IsNullOrWhiteSpace(medicament.M_PHARMACOLOGICAL__ACTION) || string.IsNullOrWhiteSpace(medicament.M_METHOD_USE_DOSAGE ) || string.IsNullOrWhiteSpace(medicament.M_DRUG_INTERACTIONS))
            //{
            //    errors.AppendLine("Нельзя оставлять это поле пустым");
            //}

           
            if (errors.Length>0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
       

            if(medicament.M_ID == 0)
            {
                FARMOKAIPKAEntities.GetContext().MEDICAMENTs.Add(medicament);
            }

            try
            {
                FARMOKAIPKAEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
