using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using DocumentFormat.OpenXml.Wordprocessing;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для dataMedicament.xaml
    /// </summary>
    public partial class dataMedicament : Window
    {
        public ObservableCollection<MEDICAMENT> med;
        FARMOKAIPKAEntities dbContext = FARMOKAIPKAEntities.GetContext();
        public string str = "";
        
        public dataMedicament(string wordFind)
        {
           
            InitializeComponent();
            //var dbContext = FARMOKAIPKAEntities.GetContext();
            str = wordFind;
            medName.Text = str;
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
                                   manafacturer = manafactur.NAME,
                                   prescript = m.M_AVAILABILITY_PRESCRIPTIONS,
                                   appearance = m.M_APPEARANCE,
                                   expDate = m.M_EXPITY_DATE

                               };

            string SumStr = "";
            
            var sym = from MS in dbContext.MEDICAMENT_has_SYMPTOMS
                      where MS.MEDICAMENT.M_NAME.ToUpper() == str.ToUpper() || MS.MEDICAMENT.M_NAME == str.ToUpper()
                      select new
                      {
                          sname = MS.SYMPTOM.S_NAME
                      };

            var group = from MG in dbContext.MEDICAMENTOS_has_GROUP
                        where MG.MEDICAMENT.M_NAME.ToUpper() == str.ToUpper() || MG.MEDICAMENT.M_NAME == str.ToUpper()
                        select new
                        {
                            mgroup = MG.Group.NAME
                        };


            foreach (var item in transactions.Where(t => t.o.ToUpper() == str.ToUpper() || t.Name.ToUpper() == str.ToUpper()))
            {
               
                MedRecep.Text = item.prescript;
                MedManafacturer.Text = item.manafacturer;
                MedApperence.Text = item.appearance;
                Date.Text = item.expDate;


            }



            foreach (var item in group.Distinct())
            {
                SumStr += $"{ item.mgroup}\n";

            }
            Med_Type.Text = SumStr;

    

        }



        public void convertStr(string str,TextBox textBox)
        {
            int count = 300; // Количество строк


            var list = new List<string>();
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                if (str == String.Empty || str.Length<132)
                {
                    break;
                }
                string pattern = @"^.{0,150}\s|$"; // где 100 - максимальная длина выходных строк (в символах)
                Match match = Regex.Match(str, pattern);
                if (match.Success)
                {
                    list.Add(match.Value.Trim());
                    str = str.Remove(0, match.Length);
                    //textBox.Text += $"{match.Value.Trim()}\n";
                    
                }

            }

            foreach (var item in list)
            {
                if (item!="")
                {
                    textBox.Text += $"{item}\n";
                    
                }
               
            }


           

        }
       

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //col22.Text = "Состав";
            //mainGrid.Visibility = Visibility.Hidden;
            //Composition_grid.Visibility = Visibility.Visible;
            //var composition = from com in dbContext.MEDICAMENTs
            //                  where com.M_NAME.ToUpper() == str.ToUpper()
            //                  select new
            //                  {
            //                      name = com.M_NAME,
            //                      com = com.M_COMPOSITION
            //                  };
            //foreach (var item in composition)
            //{
            //    Composition.Text = item.com;
            //}                    
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var composition = from com in dbContext.MEDICAMENTs
                              where com.M_NAME.ToUpper() == str.ToUpper()
                              select new
                              {
                                  name = com.M_NAME,
                                  com = com.M_COMPOSITION
                              };
            foreach (var item in composition)
            {
      
                Composition.Text = item.com;
            }
        }

        private void MedName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            FarmAction.Text = "";
            var composition = from com in dbContext.MEDICAMENTs
                              where com.M_NAME.ToUpper() == str.ToUpper()
                              select new
                              {
                                  action = com.M_PHARMACOLOGICAL__ACTION
                                
                              };

            var information = "";
           
            foreach (var item in composition)
            {

                information=item.action;
            }

            int count = 100; // Количество строк
           
           
            var list = new List<string>();
            list.Clear();
            for (int i = 0; i < count; i++)
            {
                if (information == String.Empty)
                {
                    //throw new Exception("Нечего больше разбивать))");
                }
                string pattern = @"^.{0,100}\s"; // где 100 - максимальная длина выходных строк (в символах)
                Match match = Regex.Match(information, pattern);
                if (match.Success)
                {
                    list.Add(match.Value.Trim());
                    information = $"{information.Remove(0,match.Length)}\n";
                    FarmAction.Text += $"{match.Value.Trim()}\n"; 
                }
            }

            
        }

        private void Label_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            MedInduction.Text = "";
            var specInduction = from inst in dbContext.MEDICAMENTs
                              where inst.M_NAME.ToUpper() == str.ToUpper()
                              select new
                              {
                                  instructions = inst.M_SPECIFIC_INDUCTION

                              };
            var instructions = "";
            foreach (var item in specInduction)
            {
               instructions = item.instructions;
            }

             convertStr(instructions,MedInduction);
        }

        private void Label_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {

            Contraindications.Text = "";
            var contraindications = from con in dbContext.CONTRAINDICATIONS_has_MEDICAMENT
                                where con.MEDICAMENT.M_NAME.ToUpper() == str.ToUpper()
                                select new
                                {
                                    instructions = con.CONTRAINDICATION.C_NAME

                                };
         
            foreach (var item in contraindications)
            {
                convertStr(item.instructions, Contraindications);

            }

        }

       

        private void Label_MouseLeftButtonDown_5(object sender, MouseButtonEventArgs e)
        {
            SideEffect.Text = "";
            var sideEffect = from se in dbContext.SIDE_EFFECT_has_MEDICAMENT
                                where se.MEDICAMENT.M_NAME.ToUpper() == str.ToUpper()
                                select new
                                {
                                    sideEffect = se.SIDE_EFFECT.SE_NAME

                                };
           
            foreach (var item in sideEffect)
            {
                convertStr(item.sideEffect,SideEffect);
            }

            
        }

        private void Label_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            MethodDose.Text = "";
            var methodDose = from md in dbContext.MEDICAMENTs
                             where md.M_NAME.ToUpper() == str.ToUpper()
                             select new
                             {
                                 methodDose = md.M_METHOD_USE_DOSAGE

                             };

            foreach (var item in methodDose)
            {
                convertStr(item.methodDose, MethodDose);
            }

        }

        private void Label_MouseLeftButtonDown_6(object sender, MouseButtonEventArgs e)
        {
            overDose.Text = "";
            var methodDose = from overDose in dbContext.MEDICAMENTs
                             where overDose.M_NAME.ToUpper() == str.ToUpper()
                             select new
                             {
                                overDose = overDose.M_OVERDOSE

                             };

            foreach (var item in methodDose)
            {
                convertStr(item.overDose, overDose);
            }
        }

        private void Label_MouseLeftButtonDown_7(object sender, MouseButtonEventArgs e)
        {
            overDose.Text = "";
            var methodDose = from m in dbContext.MEDICATIONs
                             join s in dbContext.MEDICAMENT_has_SYMPTOMS on m.S_ID equals s.S_ID
                             join d in dbContext.MEDICATIONs on s.S_ID equals d.S_ID

                             select new
                             {
                                name =  d.DISEASE.NAME

                             };

            foreach (var item in methodDose)
            {
                convertStr(item.name, overDose);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string  findWord = findText.Text;
            string text = overDose.Text;

         

        }

        private void Med_Type_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
