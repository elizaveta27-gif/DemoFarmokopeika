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
        public static FARMOKAIPKAEntities dbContext = FARMOKAIPKAEntities.GetContext();
        public int idManufacturer=0;
        public int idAtx = 0;
        List<int> atxId = new List<int>();
        //public List<MANUFACTURER> mANUFACTURERs = dbContext.MANUFACTURERs.ToList();
        public AddForm(MEDICAMENT selectedMedicament)
        {

            InitializeComponent();
            if (selectedMedicament != null)
            {
                medicament = selectedMedicament;
            }
            DataContext = medicament;
            listManafacturer.ItemsSource = FARMOKAIPKAEntities._context.MANUFACTURERs.ToList();
            listATX.ItemsSource = FARMOKAIPKAEntities._context.ATXes.ToList();
            var transaction = from m in dbContext.MEDICAMENTs
                              join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                              join atx in dbContext.ATXes on m.ATX_A_ID equals atx.A_ID
                              select new
                              {
                                  MR_ID = MR.MR_ID,
                                  ATX_ID = atx.A_ID
                              };
            //mANUFACTURERs = FARMOKAIPKAEntities._context.MANUFACTURERs.ToList();
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
            if (dbContext.MEDICAMENTs.Where(m=>m.M_NAME.ToUpper() == TBName.Text.ToUpper()).Count() == 0)
            {
                var nameMR = listManafacturer.Text;
                var nameAtx = listATX.Text;

                var mANUFACTURERs = dbContext.MANUFACTURERs;
                var ATX = dbContext.ATXes;
                //сделать исключение 
                idManufacturer = (int)(mANUFACTURERs.First(m => m.NAME == nameMR).MR_ID);
                idAtx = (int)(ATX.First(m => m.NAME == nameAtx).A_ID);



                MEDICAMENT medicament = new MEDICAMENT()
                {
                    M_NAME = TBName.Text,
                    M_COMPOSITION = TBCOMPOSITION.Text,
                    M_PHARMACOLOGICAL__ACTION = TBPHARMACOLOGICAL__ACTION.Text,
                    M_METHOD_USE_DOSAGE = TBMethodUse.Text,
                    M_DRUG_INTERACTIONS = TBDRUG_INTERACTIONS.Text,
                    M_SPECIFIC_INDUCTION = TBSPECIFIC_INDUCTION.Text,
                    M_STORAGE_CONDITIONS = TBSTORAGE_CONDITIONS.Text,
                    M_EXPITY_DATE = TBEXPITY_DATE.Text,
                    M_AVAILABILITY_PRESCRIPTIONS = TBAVAILABILITY_PRESCRIPTIONS.Text,
                    M_APPEARANCE = TBAPPEARANCE.Text,
                    M_OVERDOSE = TB_OVERDOSE.Text,
                    MR_ID = idManufacturer,
                    ATX_A_ID = idAtx




                };

                foreach (var item in atxId)
                {
                    var a = new MEDICAMENT_has_ATX()
                    {
                        A_ID = item,
                        M_ID = medicament.M_ID

                    };
                    FARMOKAIPKAEntities.GetContext().MEDICAMENT_has_ATX.Add(a);
                }
            }
           


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }


            if (medicament.M_ID == 0)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var nameAtx = listATX.Text;
            var ATX = dbContext.ATXes;
            idAtx = (int)(ATX.First(m => m.NAME == nameAtx).A_ID);
            listATX.Text = "";
            atxId.Add(idAtx);
           


        }
    }
}
