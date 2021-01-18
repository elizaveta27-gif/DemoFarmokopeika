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
        

        public  void queryMainCatalog()
        {
            var transactions = from m in dbContext.MEDICAMENTs //запрос для заполнения главного каталога
                               join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                               select new
                               {
                                   name = m.M_NAME,
                                   MR = MR.NAME,
                                   Price = m.M_PRICE
                               };

            foreach (var item in transactions.Distinct())
            {
                DgridMedicament.Items.Add(item);
            }


        }

        public  void querySubdirectory()
        {
            var sym = from m in dbContext.MEDICAMENT_has_SYMPTOMS
                      select new
                      {
                          sym = m.SYMPTOM.S_NAME,

                      };
            var gr = from g in dbContext.MEDICAMENTOS_has_GROUP
                     group g by g.Group.NAME into gg
                     select new
                     {
                         gr = gg.Key
                     };
            var diasese = from s in dbContext.MEDICATIONs
                          join d in dbContext.MEDICAMENT_has_SYMPTOMS on s.S_ID equals d.S_ID

                          select new
                          {
                              d = s.DISEASE.NAME
                          };

            foreach (var item in diasese.GroupBy(d => d.d))
            {
                DgridDisease.Items.Add(item);
            }

            foreach (var item in gr)
            {
                DgridGroup.Items.Add(item);
            }

            

            foreach (var item in sym.Distinct())
            {
                DgridSym.Items.Add(item);
            }
        }

        public void  clearDataGrid()
        {
            DgridMedicament.Items.Clear();
            DgridGroup.Items.Clear();
            DgridDisease.Items.Clear();
            DgridSym.Items.Clear();
        }

        public  MainPage()
        {

            InitializeComponent();
            var med = from m in dbContext.MEDICAMENTs
                      join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                      select new
                      {
                            name = m.M_NAME,
                            MR = MR.NAME,
                            Price = m.M_PRICE
                      };
            var sym = from m in dbContext.MEDICAMENT_has_SYMPTOMS
                      select new
                      {
                          sym = m.SYMPTOM.S_NAME,
                     
                      };
            var gr = from g in dbContext.MEDICAMENTOS_has_GROUP
                     group g by g.Group.NAME into gg
                     select new
                     {
                         gr = gg.Key
                     };
            var diasese = from s in dbContext.MEDICATIONs
                          join d in dbContext.MEDICAMENT_has_SYMPTOMS on s.S_ID equals d.S_ID
                    
                          select new
                          {
                              d = s.DISEASE.NAME
                          };
           
            foreach (var item in diasese.GroupBy(d => d.d))
            {
                DgridDisease.Items.Add(item);
            }

            foreach (var item in gr)
            {
                DgridGroup.Items.Add(item);
            }

            foreach (var item in med)
            {
                DgridMedicament.Items.Add(item);
            }

            foreach (var item in sym.Distinct())
            {
                DgridSym.Items.Add(item);
            }
            if (MainWindow.access != "Администратор")
            {
          
                AddBut.Visibility = Visibility.Hidden;
                DelBut.Visibility = Visibility.Hidden;
                EditBtn.Visibility = Visibility.Hidden;
                regestration.IsEnabled = false;
            }


        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            clearDataGrid();
                if (!explation.IsHitTestVisible)
                {
                    queryMainCatalog();
                    querySubdirectory();
                }
           
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            findWord = wordFind.Text;
            var transactions = from m in dbContext.MEDICAMENTs
                               join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                               join MS in dbContext.MEDICAMENT_has_SYMPTOMS on m.M_ID equals MS.M_ID
                               join s in dbContext.MEDICAMENT_has_SYMPTOMS on MS.S_ID equals s.S_ID
                               join d in dbContext.MEDICATIONs on MS.S_ID equals d.S_ID

                               select new
                               {
                                   name = m.M_NAME,
                                   sym = s.SYMPTOM.S_NAME,
                                   MR = MR.NAME,
                                   Price = m.M_PRICE,
                                   disease = d.DISEASE.NAME
                               };
          

            if (transactions.Count() == 0)
            {
                MessageBox.Show("Поиск не дал результата.");
            }
            else
            {

                DgridMedicament.Items.Clear();
                var tr = transactions.ToList().FindAll(i=> i.sym.ToLower().Contains(findWord.ToLower()) || i.name.ToLower().Contains(findWord.ToLower()) || i.disease.ToLower().Contains(findWord.ToLower()));
                foreach (var item in tr.Distinct().GroupBy(m=>m.name))
                {
                    DgridMedicament.Items.Add(item);
                }



            }



        }


        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
            
        //    var form = new Medicament("");
        //    form.Show();
        //}

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

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var carts = new cartUI();
            carts.Show();
        }

        private void DgridMedicament_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (DgridMedicament.SelectedCells!=null && DgridMedicament.SelectedCells.Count > 0)
            {
               
                    var cell = DgridMedicament.SelectedCells[0];
                    var row = DgridMedicament.ItemContainerGenerator.ContainerFromItem(cell.Item) as DataGridRow;
                if (row != null && row.IsSelected)
                    {
                        var selectStr = ((TextBlock)(this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem))).Text;//название лекарства
                        DgridSym.Items.Clear();
                        var symptom = from s in dbContext.MEDICAMENT_has_SYMPTOMS
                                      where s.MEDICAMENT.M_NAME == selectStr
                                      select new
                                      {
                                          name = s.MEDICAMENT.M_NAME,
                                          sym = s.SYMPTOM.S_NAME
                                      };
                        foreach (var item in symptom.GroupBy(m => m.sym))
                        {
                            DgridSym.Items.Add(item);
                        }
                        var diasese = from s in dbContext.MEDICATIONs
                                      join d in dbContext.MEDICAMENT_has_SYMPTOMS on s.S_ID equals d.S_ID
                                      where d.MEDICAMENT.M_NAME == selectStr
                                      select new
                                      {
                                          d = s.DISEASE.NAME
                                      };
                        DgridDisease.Items.Clear();
                        foreach (var item in diasese.GroupBy(d=>d.d))
                        {
                            DgridDisease.Items.Add(item);
                        }
                        var gr = from g in dbContext.MEDICAMENTOS_has_GROUP
                                      where g.MEDICAMENT.M_NAME == selectStr
                                      group g by g.Group.NAME into gg
                                      select new
                                      {
                                          gr = gg.Key
                                      };
                    DgridGroup.Items.Clear();
                    foreach (var item in gr)
                    {
                        DgridGroup.Items.Add(item);
                    }

                }
                  
                
            }


        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var cartUI = new cartUI();
            cartUI.Show();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            var report = new report();
            report.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            var addForm = new AddForm(null,this);
            addForm.Show();
           
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                var firstSelectedCellContent = ((TextBlock)(this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem))).Text;
                var name = dbContext.MEDICAMENTs.First(m => m.M_NAME == firstSelectedCellContent);
                var editForm = new AddForm(name,this);
                editForm.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Выделите объект, который хотите редактировать");
              
            }
           

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                var firstSelectedCellContent = ((TextBlock)(this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem))).Text;
                var name = dbContext.MEDICAMENTs.First(m => m.M_NAME == firstSelectedCellContent);

                if (MessageBox.Show($"Вы точно хотите удалить следующие {firstSelectedCellContent.Count()} элементов",
                    "Внмимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        
                        FARMOKAIPKAEntities._context.MEDICAMENTs.Remove(name);
                   
                        name.M_ID = 0;
                        FARMOKAIPKAEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены");
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                DgridMedicament.Items.Clear();
                queryMainCatalog(); 
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Выделите строку, которую хотите удалить");
            }
          
        }

        private void DgridMedicament_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
          
        }

        private void MenuItem_MouseLeave_1(object sender, MouseEventArgs e)
        {
           
        }

        private void MenuItem_MouseEnter_2(object sender, MouseEventArgs e)
        {
            strStates.Text = "Нажмите, чтобы добавить информацию в таблицу";
        }

        private void MenuItem_MouseLeave_2(object sender, MouseEventArgs e)
        {
            strStates.Text = "Наведите на элемент, о котором хотите получить информацию";
        }

        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            strStates.Text = "Выберите элемент и нажмите, чтобы отредактировать его";
        }

        private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            strStates.Text = "Наведите на элемент, о котором хотите получить информацию";
        }

        private void MenuItem_MouseEnter_1(object sender, MouseEventArgs e)
        {
            strStates.Text = "Выберите элемент и нажмите, чтобы удалить его";
        }

        private void MenuItem_MouseLeave_3(object sender, MouseEventArgs e)
        {
            strStates.Text = "Наведите на элемент, о котором хотите получить информацию";
        }

        private void MenuItem_MouseEnter_3(object sender, MouseEventArgs e)
        {
            strStates.Text = "Нажмите, чтобы посмотреть содержимое корзины";
        }

        private void MenuItem_MouseLeave_4(object sender, MouseEventArgs e)
        {
            strStates.Text = "Наведите на элемент, о которм хотите получить информацию";
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            strStates.Text = "Введите название, лекарства, симптома и нажмите, чтобы получить список лекарств";
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            strStates.Text = "Наведите на элемент, о котором хотите получить информацию";
        }

        private void Button_MouseEnter_1(object sender, MouseEventArgs e)
        {
            strStates.Text = "Выделите объект и нажмите, чтобы добавить в корзину";
        }

        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            strStates.Text = "Наведите на элемент, о котором хотите получить информацию";
        }

        private void MenuItem_MouseEnter_4(object sender, MouseEventArgs e)
        {
            strStates.Text = "Нажмите, чтобы выйти из системы";
        }

        private void MenuItem_MouseEnter_5(object sender, MouseEventArgs e)
        {

        }

        private void MenuItem_MouseEnter_6(object sender, MouseEventArgs e)
        {

        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            registration registration = new registration();
            registration.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var addForm = new AddForm(null, this);
            addForm.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                var firstSelectedCellContent = ((TextBlock)(this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem))).Text;
                var name = dbContext.MEDICAMENTs.Where(m => m.M_NAME == firstSelectedCellContent);

                if (MessageBox.Show($"Вы точно хотите удалить следующие {firstSelectedCellContent.Count()} элементов",
                    "Внмимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        FARMOKAIPKAEntities._context.MEDICAMENTs.RemoveRange(name);
                        FARMOKAIPKAEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены");
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                DgridMedicament.Items.Clear();
                queryMainCatalog();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Выделите строку, которую хотите удалить");
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                var firstSelectedCellContent = ((TextBlock)(this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem))).Text;
                var name = dbContext.MEDICAMENTs.First(m => m.M_NAME == firstSelectedCellContent);
                var editForm = new AddForm(name, this);
                editForm.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Выделите строку, который хотите редактировать");

            }

        }

        private void DgridMedicament_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.access == "Администратор")
            {
                try
                {
                    var firstSelectedCellContent = ((TextBlock)(this.DgridMedicament.Columns[0].GetCellContent(DgridMedicament.SelectedItem))).Text;
                    var name = dbContext.MEDICAMENTs.First(m => m.M_NAME == firstSelectedCellContent);
                    var editForm = new AddForm(name, this);
                    editForm.Show();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Выделите объект, который хотите редактировать");

                }
            }
        }

        private void Button_MouseEnter_2(object sender, MouseEventArgs e)
        {
            strStates.Text = "Нажмите, чтобы добавить информацию в таблицу";
        }

        private void Button_MouseLeave_2(object sender, MouseEventArgs e)
        {
            strStates.Text = "Наведите на элемент, о котором хотите получить информацию";
        }

        private void Button_MouseEnter_3(object sender, MouseEventArgs e)
        {
            strStates.Text = "Выберите элемент и нажмите, чтобы удалить его";
        }

        private void Button_MouseLeave_3(object sender, MouseEventArgs e)
        {
            strStates.Text = "Выберите элемент и нажмите, чтобы удалить его";
        }

        private void Button_MouseEnter_4(object sender, MouseEventArgs e)
        {
            strStates.Text = "Выберите элемент и нажмите, чтобы отредактировать его";
        }

        private void Button_MouseLeave_4(object sender, MouseEventArgs e)
        {
            strStates.Text = "Наведите на элемент, о котором хотите получить информацию";
        }
    }
}
