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
    
    public partial class MEDICAMENTOS_has_GROUP
    {
        public int ID { get; set; }
        public int M_ID { get; set; }
        public int G_ID { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual MEDICAMENT MEDICAMENT { get; set; }
    }
}