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
    
    public partial class Active_Substance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Active_Substance()
        {
            this.MEDICAMENT_HAS_ACTIVESUBSTANCE = new HashSet<MEDICAMENT_HAS_ACTIVESUBSTANCE>();
        }
    
        public int AS_ID { get; set; }
        public string AS_NAME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MEDICAMENT_HAS_ACTIVESUBSTANCE> MEDICAMENT_HAS_ACTIVESUBSTANCE { get; set; }
    }
}
