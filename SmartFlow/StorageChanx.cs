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
    
    public partial class StorageChanx
    {
        public int Id { get; set; }
        public string ChangeType { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int SuppliersClientsId { get; set; }
        public int WarehouseId { get; set; }
        public System.DateTime ChangeDate { get; set; }
        public int UserId { get; set; }
        public Nullable<int> TransportID { get; set; }
    
        public virtual SuppliersClient SuppliersClient { get; set; }
        public virtual User User { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }
    }
}