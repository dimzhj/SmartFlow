//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartFlow
{
    using System;
    using System.Collections.Generic;
    
    public partial class SuppliersClient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SuppliersClient()
        {
            this.StorageChanges = new HashSet<StorageChanx>();
            this.Transports = new HashSet<Transport>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public int WarehouseID { get; set; }
        public bool Disabled { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageChanx> StorageChanges { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transport> Transports { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
