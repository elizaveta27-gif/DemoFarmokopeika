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
    
    public partial class MEDICAMENT_has_SYMPTOMS
    {
        public int ID { get; set; }
        public int M_ID { get; set; }
        public int S_ID { get; set; }
    
        public virtual MEDICAMENT MEDICAMENT { get; set; }
        public virtual SYMPTOM SYMPTOM { get; set; }
    }
}
