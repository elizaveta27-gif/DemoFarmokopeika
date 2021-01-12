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
    /// Логика взаимодействия для report.xaml
    /// </summary>
    public partial class report : Window
    {
        public static FARMOKAIPKAEntities dbContext = FARMOKAIPKAEntities.GetContext();
        public report()
        {
            InitializeComponent();
            #region заполняем самый продаваемый товар
            var transaction = from m in dbContext.MEDICAMENTs //Запрос для таблицы лекарства
                              join MR in dbContext.MANUFACTURERs on m.MR_ID equals MR.MR_ID
                              select new
                              {
                                  id = m.M_ID,
                                  name = m.M_NAME,
                                  price = m.M_PRICE,
                                  MR = MR.NAME
                              };

            

            var query = dbContext.Sells.GroupBy(x => x.M_ID)
              //.Where(g => g.Count() )
              .Select(y => new { Element = y.Key, Counter = y.Count()} );//количество проданного товара

            int maxElem = 0;//товар, который продали больше всего
            int maxCout = 0;
            foreach (var item in query)
            {
                
                if (maxElem<item.Counter)
                {
                    maxCout = item.Counter;
                    maxElem = item.Element;

                }
                
               
            }

            foreach (var item in transaction.Where(t=>t.id == maxElem))
            {
                TBName.Text = item.name;
                TBPrice.Text = item.price.ToString();
                TBMAnafacturer.Text = item.MR;
            }//самый продаваемый товар
            #endregion

            var transaction2 = from m in query
                               join s in dbContext.Sells on m.Element equals s.M_ID
                               join MR in dbContext.MANUFACTURERs on s.MEDICAMENT.MR_ID equals MR.MR_ID
                               select new
                               {
                                   Name = s.MEDICAMENT.M_NAME,
                                   Price = s.MEDICAMENT.M_PRICE,
                                   Cout = m.Counter,
                                   Manufacturer = MR.NAME
                               };





            foreach (var item in transaction2.Distinct().OrderByDescending(t=>t.Cout))
            {
                rating.Items.Add(item);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
