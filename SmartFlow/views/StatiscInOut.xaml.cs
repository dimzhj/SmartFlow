using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartFlow.views
{
    /// <summary>
    /// Interaction logic for StatiscInOut.xaml
    /// </summary>
    public partial class StatiscInOut : Page
    {
        private ObservableCollection<AssetClass> classes = new ObservableCollection<AssetClass>();
        public StatiscInOut()
        {
            InitializeComponent();

            var whs = (from w in MainWindow.ctx.Warehouses select w).ToList();
            foreach(Warehouse wh in whs)
            {
                string whname = wh.Name;
                var inamt = (from i in MainWindow.ctx.StorageChanges where i.WarehouseId == wh.Id && i.ChangeType=="In" select i).Sum(i=> (double?)i.Quantity * (double?)i.Price) ?? 0;
                var outamt = (from o in MainWindow.ctx.StorageChanges where o.WarehouseId == wh.Id && o.ChangeType == "Out" select o).Sum(o => (double?)o.Quantity * (double?)o.Price) ?? 0;
                var remaining = (from r in MainWindow.ctx.Products where r.WarehouseId == wh.Id select r).Sum(r => (double?)r.CurrentQuantity * (double?)r.Cost) ?? 0; 
                classes.Add(new AssetClass { Warehouse = whname, InAmount = inamt, OutAmount = outamt, Remaining = remaining });
            }
            this.DataContext = classes;
        }

        private void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewColumn column = ((GridViewColumnHeader)e.OriginalSource).Column;
                piePlotter.PlottedProperty = column.Header.ToString();
            }
            catch(InvalidCastException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
