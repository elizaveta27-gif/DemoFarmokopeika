using DocumentFormat.OpenXml.ExtendedProperties;
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
using System.ComponentModel;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        //public static string word;
        public static Cart cart = new Cart(MainWindow.seller); //создаем объект корзины и присваем статическое свойство продавец
        public static FARMOKAIPKAEntities dbContext = FARMOKAIPKAEntities.GetContext();
        string findWord = ""; //слово, которое используется в поиске
        Seller seller = new Seller();

        public void queryMainCatalog()
        {
            var transactions = from m in dbContext.MEDICAMENTs //запрос для заполнения главного каталога
                               join a in dbContext.MEDICAMENT_has_ATX on m.ATX_A_ID equals a.A_ID
                               join atx in dbContext.MEDICAMENT_has_ATX on m.M_ID equals atx.M_ID
                               join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                               join MS in dbContext.MEDICAMENT_has_SYMPTOMS on m.M_ID equals MS.M_ID
                               join s in dbContext.MEDICAMENT_has_SYMPTOMS on MS.S_ID equals s.S_ID
                               join d in dbContext.MEDICATIONs on MS.S_ID equals d.S_ID
                               //join med in dbContext.MEDICATIONs on s.S_ID equals med.D_ID
                               join g in dbContext.MEDICAMENTOS_has_GROUP on m.M_ID equals g.M_ID
                               join gr in dbContext.Groups on g.G_ID equals gr.G_ID

                               select new
                               {
                                   Name = m.M_NAME,
                                   Composition = m.M_COMPOSITION,
                                   ATX = a.ATX.NAME,
                                   MethodUse = m.M_METHOD_USE_DOSAGE,
                                   o = MS.SYMPTOM.S_NAME,
                                   disease = d.DISEASE.NAME,
                                   MR = MR.NAME,
                                   sym = s.SYMPTOM.S_NAME,
                                   Group = gr.NAME

                               };

            foreach (var item in transactions.Distinct())
            {
                DgridMedicament.Items.Add(item);
            }


        }


        public MainPage()
        {

            InitializeComponent();
            queryMainCatalog();
           
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgridMedicament.Items.Clear();
            if (!explation.IsHitTestVisible)
            {
                queryMainCatalog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            findWord = wordFind.Text;
            var transactions = from m in dbContext.MEDICAMENTs

                                  
                               join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                               join a in dbContext.MEDICAMENT_has_ATX on m.ATX_A_ID equals a.A_ID
                               join MS in dbContext.MEDICAMENT_has_SYMPTOMS on m.M_ID equals MS.M_ID
                               join s in dbContext.MEDICAMENT_has_SYMPTOMS on MS.S_ID equals s.S_ID
                               join d in dbContext.MEDICATIONs on MS.S_ID equals d.S_ID
                               join g in dbContext.MEDICAMENTOS_has_GROUP on m.M_ID equals g.M_ID
                               join gr in dbContext.Groups on g.G_ID equals gr.G_ID
                               where m.M_NAME.ToLower().Contains(findWord.ToLower()) || MR.NAME.ToLower().Contains(findWord.ToLower()) || d.DISEASE.NAME.ToLower().Contains(findWord.ToLower()) || s.SYMPTOM.S_NAME.ToLower().Contains(findWord.ToLower())


                               select new
                               {
                                   Name = m.M_NAME,
                                   Composition = m.M_COMPOSITION,
                                   ATX = a.ATX.NAME,
                                   MethodUse = m.M_METHOD_USE_DOSAGE,
                                   o = MS.SYMPTOM.S_NAME,
                                   disease = d.DISEASE.NAME,
                                   MR = MR.NAME,
                                   sym = s.SYMPTOM.S_NAME,
                                   Group = gr.NAME
                               };
            if (transactions.Count() == 0)
            {
                MessageBox.Show("Поиск не дал результата.");
            }
            else
            {
                DgridMedicament.Items.Clear();
                foreach (var item in transactions.Distinct())
                {
                    DgridMedicament.Items.Add(item);
                }
               

            }






        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            var form = new Medicament();
            form.Show();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            string word = "";
            var firstSelectedCellContent = this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem);// выделенное название
            word = ((TextBlock)firstSelectedCellContent).Text;
            dataMedicament dm = new dataMedicament(word);
            dm.Show();
        }

        private void WordFind_LostMouseCapture(object sender, MouseEventArgs e)
        {
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        { //сделать исключение

            try
            {
                var nameMed = ((TextBlock)(this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem))).Text;//название лекарства
                var meds = dbContext.MEDICAMENTs.Where(m => m.M_NAME == nameMed);
                foreach (var item in meds)
                {
                    cart.Add(item);
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Выделите товар, который хотите положить в корзину");
            }
            

            var report = new AddForm(null);
            report.Show();


        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var firstSelectedCellContent = ((TextBlock)(this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem))).Text;
            var name = dbContext.MEDICAMENTs.First(m => m.M_NAME == firstSelectedCellContent);
            var report = new AddForm(name);
            report.Show();

            var carts = new cartUI();
            carts.Show();
        }

        private void DgridMedicament_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
