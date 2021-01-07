﻿using System;
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
    /// Логика взаимодействия для cartUI.xaml
    /// </summary>
    public partial class cartUI : Window
    {
        public List<MEDICAMENT> listMed;
        public static FARMOKAIPKAEntities dbContext = FARMOKAIPKAEntities.GetContext();
        public cartUI()
        {
            InitializeComponent();
            //добавить исключение
            listMed = MainPage.cart.GetAll();


            var tr = from f in listMed
                     group f by f.M_NAME into g
                     select new
                     {
                         name = g.Key,
                         Cout = g.Count(),
                         
                         
                     };

            var trans = listMed.GroupBy(p => p.M_NAME)
                        .Select(g => new
                        {
                            name = g.Key,
                            Cout = g.Count(),
                            price = g.Select(p => p.M_PRICE)
                        });
            
            foreach (var item in trans)
            {
                ListMedicament.Items.Add(item);
                
            }
            decimal cost=0;
            foreach (var item in listMed)
            {
               cost += (decimal)item.M_PRICE;
            }
            Cost.Text = cost.ToString();
           

            
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CHECK check = new CHECK()
            {
                C_ID = 1,
                DATA = DateTime.Now,
                S_ID = MainWindow.seller.S_ID,

            };
            dbContext.CHECKs.Add(check);
            foreach (var item in listMed)
            {
                Sell sell = new Sell()
                {
                    CK_ID = check.CK_ID,
                    M_ID = item.M_ID
                };
                dbContext.Sells.Add(sell);
                dbContext.SaveChanges();
            }


            MessageBox.Show("Заказ оформлен");

            ListMedicament.Items.Clear();
            Cost.Text = "0";
            this.Close();

        }
    }
}
