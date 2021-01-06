using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisnessLogical
{
    public class Cart: IEnumerable
    {
        public DISEASE BName { get; set; }
        public Seller Seller { get; set; }
        public Dictionary<MEDICAMENT, int> Lekarstvoes { get; set; }

        public Cart(Seller seller)
        {
            Seller= seller;
            Lekarstvoes = new Dictionary<MEDICAMENT, int>();
        }

        public void Add(MEDICAMENT medicament)
        {
            if (Lekarstvoes.TryGetValue(medicament, out int count))
            {
                Lekarstvoes[medicament] = ++count;
            }
            else
            {
                Lekarstvoes.Add(medicament, 1);
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var lekarstvo in Lekarstvoes.Keys)
            {
                for (int i = 0; i < Lekarstvoes[lekarstvo]; i++)
                {
                    yield return lekarstvo;
                }
            }
        }

        public List<MEDICAMENT> GetAll()
        {
            var result = new List<MEDICAMENT>();
            foreach (MEDICAMENT i in this)
            {
                result.Add(i);
            }
            return result;
        }
    }
}

