using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SmartFlow.views
{
    public class AssetClass : INotifyPropertyChanged
    {
        private String warehouse;

        public String Warehouse
        {
            get { return warehouse; }
            set {
                warehouse = value;
                RaisePropertyChangeEvent("Warehouse");
            }
        }

        private double inamount;

        public double InAmount
        {
            get { return inamount; }
            set {
                inamount = value;
                RaisePropertyChangeEvent("InAmount");
            }
        }

        private double outamount;

        public double OutAmount
        {
            get { return outamount; }
            set {
                outamount = value;
                RaisePropertyChangeEvent("OutAmount");
            }
        }

        private double remaining;

        public double Remaining
        {
            get { return remaining; }
            set {
                remaining = value;
                RaisePropertyChangeEvent("Remaining");
            }
        }



/*        public static List<AssetClass> ConstructTestData()
        {
            List<AssetClass> assetClasses = new List<AssetClass>();

            assetClasses.Add(new AssetClass(){ Warehouse = "Montreal", InAmount = 1.56, OutAmount = 1.56, Remaining = 4.82});
            assetClasses.Add(new AssetClass(){ Warehouse = "Brossard", InAmount = 2.92, OutAmount = 2.92, Remaining = 17.91});
            assetClasses.Add(new AssetClass(){ Warehouse = "Kirkland", InAmount = 13.24, OutAmount = 2.40, Remaining = 6.04});

            return assetClasses;
        }*/

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangeEvent(String propertyName)
        {
            if (PropertyChanged!=null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            
        }

        #endregion
    }
}
