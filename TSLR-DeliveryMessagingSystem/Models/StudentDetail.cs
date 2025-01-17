//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TSLR_DeliveryMessagingSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentDetail()
        {
            this.CustomMessages = new HashSet<CustomMessage>();
        }
    
        public int Id { get; set; }
        public string Building_No { get; set; }
        public string Room_No_ { get; set; }
        public string Name { get; set; }
        public string Email_Address_ { get; set; }
        public string Contact_Number_ { get; set; }
        public string University_of_Study_ { get; set; }
        public string Course_ { get; set; }
        public string Student_ID_ { get; set; }
        public Nullable<int> BuildingId { get; set; }
        public Nullable<int> MessageId { get; set; }
        public Nullable<int> CompanyId { get; set; }
    
        public virtual Building Building { get; set; }
        public virtual Company Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomMessage> CustomMessages { get; set; }
        public virtual Message Message { get; set; }
    }
}
