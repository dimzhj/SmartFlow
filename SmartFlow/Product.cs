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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.StorageChanges = new HashSet<StorageChanx>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<int> CurrentQuantity { get; set; }
        public int WarehouseId { get; set; }
        public bool Disabled { get; set; }
        public string Name { get; set; }
    
        public virtual Warehouse Warehouse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StorageChanx> StorageChanges { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}