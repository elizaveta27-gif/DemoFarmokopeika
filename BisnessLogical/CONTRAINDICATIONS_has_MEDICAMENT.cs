//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BisnessLogical
{
    using System;
    using System.Collections.Generic;
    
    public partial class CONTRAINDICATIONS_has_MEDICAMENT
    {
        public int ID { get; set; }
        public int C_ID { get; set; }
        public int M_ID { get; set; }
    
        public virtual CONTRAINDICATION CONTRAINDICATION { get; set; }
        public virtual MEDICAMENT MEDICAMENT { get; set; }
    }
}