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
        readonly private MEDICAMENT medicament = new MEDICAMENT();
        public static FARMOKAIPKAEntities dbContext = FARMOKAIPKAEntities.GetContext();
        public int idAtx = 0;
        public int idSym = 0;
        public int idGroup = 0;
        readonly List<int> atxId = new List<int>();
        readonly List<int> symList = new List<int>();
        readonly List<int> groupList = new List<int>();
        public MainPage mainPage;
        public AddForm(MEDICAMENT selectedMedicament,MainPage mainPage)
        {
            this.mainPage = mainPage; 

            InitializeComponent();
            if (selectedMedicament != null)
            {
                medicament = selectedMedicament;
            }
            DataContext = medicament;
            listManafacturer.ItemsSource = FARMOKAIPKAEntities._context.MANUFACTURERs.ToList();
            listATX.ItemsSource = FARMOKAIPKAEntities._context.ATXes.ToList();
            listSym.ItemsSource = FARMOKAIPKAEntities.GetContext().SYMPTOMS.ToList();
            listGroup.ItemsSource = FARMOKAIPKAEntities.GetContext().Groups.ToList();
            
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                var mANUFACTURERs = dbContext.MANUFACTURERs;
                var nameMR = listManafacturer.Text;
                if (dbContext.MEDICAMENTs.Where(m => m.M_NAME.ToUpper() == TBName.Text.ToUpper()).Count() == 0)
                {

                    var nameAtx = listATX.Text;
                    var nameSym = listSym.Text;

                    var ATX = dbContext.ATXes;
                    var sYMPTOMs = dbContext.SYMPTOMS;
                    var groupId = dbContext.Groups;
             
                    int idManufacturer = (int)(mANUFACTURERs.First(m => m.NAME == nameMR).MR_ID);



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

                if (medicament.M_ID == 0)
                {
                    FARMOKAIPKAEntities.GetContext().MEDICAMENTs.Add(medicament);
                }

                FARMOKAIPKAEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                mainPage.Close();
                MainPage mainPage2 = new MainPage
                {
                    WindowState = (WindowState)System.Windows.Forms.FormWindowState.Maximized
                };
                mainPage2.Show();
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var nameAtx = listATX.Text;
                var ATX = dbContext.ATXes;
                idAtx = (int)(ATX.First(m => m.NAME == nameAtx).A_ID);
                listATX.Text = "";
                atxId.Add(idAtx);
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите ATX из списка");
                
            }
           

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var nameSymptom = listSym.Text;
                var SymptomTable = dbContext.SYMPTOMS;
                idSym = (int)(SymptomTable.First(m => m.S_NAME == nameSymptom).S_ID);
                listSym.Text = "";
                symList.Add(idSym);
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите симптом из списка");
            }
          
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var nameGroup = listGroup.Text;
                var GroupTable = dbContext.Groups;
                idGroup = (int)(GroupTable.First(m => m.NAME == nameGroup).G_ID);
                listGroup.Text = "";
                groupList.Add(idGroup);
            }
            catch (Exception)
            {

                MessageBox.Show("Выберите группу лекарства из списка");
            }
           
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            var mainPage = new MainPage();
            mainPage.Show();
            this.Close();
            
        }

       
    }
}
