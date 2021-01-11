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
        //public int idManufacturer=0;
        public int idAtx = 0;
        public int idSym = 0;
        public int idGroup = 0;
        List<int> atxId = new List<int>();
        List<int> symList = new List<int>();
        List<int> groupList = new List<int>();
        //public List<MANUFACTURER> mANUFACTURERs = dbContext.MANUFACTURERs.ToList();
        public AddForm(MEDICAMENT selectedMedicament)
        {

            InitializeComponent();
            if (selectedMedicament != null)
            {
                medicament = selectedMedicament;
              
                var listatx = medicament.MEDICAMENT_has_ATX;
            }
            DataContext = medicament;
            listManafacturer.ItemsSource = FARMOKAIPKAEntities._context.MANUFACTURERs.ToList();
            listATX.ItemsSource = FARMOKAIPKAEntities._context.ATXes.ToList();
            listSym.ItemsSource = FARMOKAIPKAEntities.GetContext().SYMPTOMS.ToList();
            listGroup.ItemsSource = FARMOKAIPKAEntities.GetContext().Groups.ToList();
          
            listDisease.ItemsSource = FARMOKAIPKAEntities.GetContext().DISEASEs.ToList();
            var transaction = from m in dbContext.MEDICAMENTs
                              join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                              join atx in dbContext.MEDICAMENT_has_ATX on m.M_ID equals atx.M_ID
                              select new
                              {
                                  MR_ID = MR.MR_ID,
                                  ATX_ID = atx.A_ID
                              };
            //mANUFACTURERs = FARMOKAIPKAEntities._context.MANUFACTURERs.ToList();
            
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
            var mANUFACTURERs = dbContext.MANUFACTURERs;
            var nameMR = listManafacturer.Text;
            //}
            if (dbContext.MEDICAMENTs.Where(m=>m.M_NAME.ToUpper() == TBName.Text.ToUpper()).Count() == 0)
            {
              
                var nameAtx = listATX.Text;
                var nameSym = listSym.Text;
               
                var ATX = dbContext.ATXes;
                var sYMPTOMs = dbContext.SYMPTOMS;
                var groupId = dbContext.Groups; 
                //сделать исключение 
                int idManufacturer = (int)(mANUFACTURERs.First(m => m.NAME == nameMR).MR_ID);
                //idAtx = (int)(ATX.First(m => m.NAME == nameAtx).A_ID);
                //int idSym = (int)(sYMPTOMs.First(m => m.S_NAME == nameSym).S_ID);(int)(mANUFACTURERs.First(m => m.NAME == nameMR).MR_ID)
                //int idGroup = (int)(groupId.First(m => m.NAME == nameSym).G_ID);


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
                    MR_ID = 2,
                    M_PRICE = Convert.ToDecimal(TBPrice.Text)





                };

               
            }
            medicament.MR_ID = (int)(mANUFACTURERs.First(m => m.NAME == nameMR).MR_ID);
            foreach (var item in atxId)
            {
                var a = new MEDICAMENT_has_ATX()
                {
                    A_ID = item,
                    M_ID = medicament.M_ID

                };
                FARMOKAIPKAEntities.GetContext().MEDICAMENT_has_ATX.Add(a);
            }

            foreach (var item in symList)
            {
                var a = new MEDICAMENT_has_SYMPTOMS()
                {
                    S_ID = item,
                    M_ID = medicament.M_ID

                };
                FARMOKAIPKAEntities.GetContext().MEDICAMENT_has_SYMPTOMS.Add(a);
            }

            foreach (var item in groupList)
            {
                var a = new MEDICAMENTOS_has_GROUP()
                {
                    G_ID = item,
                    M_ID = medicament.M_ID

                };
                FARMOKAIPKAEntities.GetContext().MEDICAMENTOS_has_GROUP.Add(a);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var nameSymptom = listSym.Text;
            var SymptomTable = dbContext.SYMPTOMS;
            idSym = (int)(SymptomTable.First(m => m.S_NAME == nameSymptom).S_ID);
            listSym.Text = "";
            symList.Add(idSym);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            var nameGroup = listGroup.Text;
            var GroupTable = dbContext.Groups;
            idGroup = (int)(GroupTable.First(m => m.NAME == nameGroup).G_ID);
            listGroup.Text = "";
            groupList.Add(idGroup);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = new MainPage();
            mainPage.Show();
            this.Close();
            
        }
    }
}
