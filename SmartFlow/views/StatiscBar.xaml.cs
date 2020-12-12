using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SmartFlow.views
{
    /// <summary>
    /// Interaction logic for StatiscBar.xaml
    /// </summary>
    public partial class StatiscBar : Page
    {
        public StatiscBar()
        {
            InitializeComponent();
            this.DataContext = this;

            Data = new ObservableCollection<MyData>();
            DateTime now = DateTime.Now;
            var tss = (from w in MainWindow.ctx.Transports where w.TransportDate.Year == now.Year select w).Select(o => o.TransportDate.Month).Distinct().ToList();
            foreach (int m in tss)
            {
                now = new System.DateTime(now.Year, m, 1);
                var inamt = (from i in MainWindow.ctx.StorageChanges where i.TransportID != null && i.ChangeDate.Month == m && i.ChangeType == "In" select i).Sum(i => (double?)i.Quantity * (double?)i.Price) ?? 0;
                Data.Add(new MyData() { Month = now, Value = inamt, WorkType = DataTypes.Inbound });
                                
                var outamt = (from o in MainWindow.ctx.StorageChanges where o.TransportID != null && o.ChangeDate.Month == m && o.ChangeType == "Out" select o).Sum(o => (double?)o.Quantity * (double?)o.Price) ?? 0;
                Data.Add(new MyData() { Month = now, Value = outamt, WorkType = DataTypes.Outbound });
            }

            BarChart1.ItemsSource = Data;
        }

        private ObservableCollection<MyData> _data = null;
        public ObservableCollection<MyData> Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
    }

    public enum DataTypes
    {
        Inbound,
        Outbound,
        Remaining
    }

    public class MyData
    {
        public MyData()
        {
        }

        public DateTime Month { get; set; }

        public double Value { get; set; }

        public DataTypes WorkType { get; set; }
    }
}
