
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for TransportPage.xaml
    /// </summary>
    public partial class TransportPage : Page
    {
        public TransportPage()
        {
            InitializeComponent();
            comboWarehouse.ItemsSource = (from w in MainWindow.ctx.Warehouses select w).ToList();
            comboSupClt.ItemsSource = (from w in MainWindow.ctx.SuppliersClients where w.WarehouseID == MainWindow.currentUser.Warehouse.Id && w.Disabled == false select w).ToList();
            comboWarehouse.SelectedItem = MainWindow.currentUser.Warehouse;
            getAllData();
        }
        private void getAllData()
        {
            lvDataBinding.ItemsSource = (from w in MainWindow.ctx.Transports where w.WarehouseId == MainWindow.currentUser.Warehouse.Id select w).ToList();
        }

        private void comboSupClt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboSupClt.SelectedIndex == -1 || dpChangeDate.SelectedDate == null)
            {
                btAdd.IsEnabled = false;
                btUpdate.IsEnabled = false;
                btPrint.IsEnabled = false;
                btManagerProd.IsEnabled = false;
                return;
            }
            if (tbId.Text != "")
            {
                btUpdate.IsEnabled = true;
                btPrint.IsEnabled = true;
                btManagerProd.IsEnabled = true;
            }
            else
                btAdd.IsEnabled = true;
        }


        private void dpChangeDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboSupClt.SelectedIndex == -1 || dpChangeDate.SelectedDate == null)
            {
                btAdd.IsEnabled = false;
                btUpdate.IsEnabled = false;
                btPrint.IsEnabled = false;
                btManagerProd.IsEnabled = false;
                rbIn.IsEnabled = true;
                rbOut.IsEnabled = true;
                return;
            }
            if (tbId.Text != "")
            {
                btUpdate.IsEnabled = true;
                btPrint.IsEnabled = true;
                btManagerProd.IsEnabled = true;
                rbIn.IsEnabled = false;
                rbOut.IsEnabled = false;
            }                
            else
            {
                rbIn.IsEnabled = true;
                rbOut.IsEnabled = true;
                btAdd.IsEnabled = true;
            }                
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            Transport ts = new Transport() {TransType = (rbIn.IsChecked == true) ? "In" : "Out", Warehouse = (Warehouse)comboWarehouse.SelectedItem, TransportDate = DateTime.Parse(dpChangeDate.SelectedDate.ToString()), User = MainWindow.currentUser, SuppliersClient = (SuppliersClient)comboSupClt.SelectedItem };
            MainWindow.ctx.Transports.Add(ts);

            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The product cannot be added.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //int id = ts.Id;
            //MainWindow.ctx.Entry((Product)comboProduct.SelectedItem).Reload();

            tbId.Text = ""+ ts.Id;
            selectedts = ts;
            btUpdate.IsEnabled = true;
            btPrint.IsEnabled = true;
            btManagerProd.IsEnabled = true;
            btAdd.IsEnabled = false;
            rbIn.IsEnabled = false;
            rbOut.IsEnabled = false;

            getAllData();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedts == null)
            {
                rbIn.IsEnabled = true;
                rbOut.IsEnabled = true;
                btUpdate.IsEnabled = false;
                btPrint.IsEnabled = false;
                btManagerProd.IsEnabled = false;
                MessageBox.Show("Please select a transport.\n", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            selectedts.Warehouse = MainWindow.currentUser.Warehouse;
            selectedts.TransportDate = DateTime.Parse(dpChangeDate.SelectedDate.ToString());
            //selectedts.TransType = (rbIn.IsChecked == true) ? "In" : "Out";
            selectedts.User = MainWindow.currentUser;
            selectedts.SuppliersClient = (SuppliersClient)comboSupClt.SelectedItem;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The product cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var scs = (from s in MainWindow.ctx.StorageChanges where s.TransportID == selectedts.Id select s).ToList();
            foreach(StorageChanx sc in scs)
            {
                MainWindow.ctx.Entry(sc.Product).Reload(); 
            }
            
            tbId.Text = "";
            tbAmount.Text = "";
            tbTotalqty.Text = "";
            comboSupClt.SelectedIndex = -1;
            dpChangeDate.Text = "";
            comboSupClt.SelectedIndex = -1;
            btUpdate.IsEnabled = false;
            btPrint.IsEnabled = false;
            btAdd.IsEnabled = false;
            rbIn.IsEnabled = true;
            rbOut.IsEnabled = true;

            btManagerProd.IsEnabled = false;
            selectedts = null;
            getAllData();
        }

        Transport selectedts;
        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                tbId.Text = "";
                tbAmount.Text = "";
                tbTotalqty.Text = "";
                comboSupClt.SelectedIndex = -1;
                dpChangeDate.Text = "";
                comboSupClt.SelectedIndex = -1;
                btUpdate.IsEnabled = false;
                btPrint.IsEnabled = false;
                btAdd.IsEnabled = false;
                rbIn.IsEnabled = true;
                rbOut.IsEnabled = true;
                btManagerProd.IsEnabled = false;
                selectedts = null;
            }
            else
            {
                selectedts = (Transport)lvDataBinding.SelectedItem;
                tbId.Text = selectedts.Id + "";
                tbAmount.Text = "";

                if (selectedts.TransType == "In")
                    rbIn.IsChecked = true;
                else
                    rbOut.IsChecked = true;
                dpChangeDate.Text = selectedts.TransportDate.ToString();
                comboSupClt.SelectedItem = selectedts.SuppliersClient;
                btUpdate.IsEnabled = true;
                btPrint.IsEnabled = true;
                btAdd.IsEnabled = false;
                rbIn.IsEnabled = false;
                rbOut.IsEnabled = false;
                btManagerProd.IsEnabled = true;
                var sc = (from w in MainWindow.ctx.StorageChanges where w.TransportID == selectedts.Id select w);
                int qty = sc.Sum(c => (int?)c.Quantity) ?? 0;
                var amount = sc.Sum(c => (double?)c.Quantity * c.Price) ?? 0;
                tbTotalqty.Text = qty + "";
                tbAmount.Text = amount + "";
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            selectedts = (Transport)((Button)sender).DataContext;
            if (selectedts != null)
            {
                var scs = (from s in MainWindow.ctx.StorageChanges where s.TransportID == selectedts.Id select s).ToList();
                foreach(StorageChanx sc in scs)
                {
                    Product product = sc.Product;
                    int quantity = sc.Quantity;
                    string ctype = sc.ChangeType;
                    if (ctype == "In" && product.CurrentQuantity < quantity)
                    {
                        MessageBox.Show("The on-hand quantity of "+ product.Name +" less then Inbound quantity, this trans cannot be deleted", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }


                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete transport of " + selectedts.SuppliersClient.Name, "Delete Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    try
                    {
                        MainWindow.ctx.Transports.Remove(selectedts);
                        string msg = MainWindow.ContextSave();
                        if (msg != "")
                        {
                            MessageBox.Show("The change cannot be deleted.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show("Deletion cannot finished: \n" + ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var prods = (from s in MainWindow.ctx.Products select s).ToList();
                    foreach (Product prod in prods)
                    {
                        MainWindow.ctx.Entry(prod).Reload();
                    }
                    getAllData();
                    selectedts = null;
                }
            }
        }

        private void btManagerProd_Click(object sender, RoutedEventArgs e)
        {
            if (selectedts == null)
            {
                tbId.Text = "";
                tbAmount.Text = "";
                tbTotalqty.Text = "";
                comboSupClt.SelectedIndex = -1;
                dpChangeDate.Text = "";
                comboSupClt.SelectedIndex = -1;
                btUpdate.IsEnabled = false;
                btPrint.IsEnabled = false;
                btAdd.IsEnabled = false;
                rbIn.IsEnabled = true;
                rbOut.IsEnabled = true;
                btManagerProd.IsEnabled = false;
                MessageBox.Show("Please select a transport again. \n", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
                
            ManagerProducts dig = new ManagerProducts(selectedts);
            dig.ShowInTaskbar = false;
            if (dig.ShowDialog() == true)
            {
                var sc = (from w in MainWindow.ctx.StorageChanges where w.TransportID == selectedts.Id select w);
                int qty = sc.Sum(c => (int?)c.Quantity) ?? 0;
                var amount = sc.Sum(c => (double?)c.Quantity*c.Price) ?? 0;
                tbTotalqty.Text = qty + "";
                tbAmount.Text = amount +"";
            }
        }

        private bool productFilter(object item)
        {
            if (String.IsNullOrEmpty(tbSearch.Text))
                return true;
            else
                return ((item as Transport).SuppliersClient.Name.IndexOf(tbSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = tbSearch.Text;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvDataBinding.ItemsSource);
            view.Filter = productFilter;
        }

        private void btPrint_Click(object sender, RoutedEventArgs e)
        {
            if (selectedts == null)
            {
                tbId.Text = "";
                tbAmount.Text = "";
                tbTotalqty.Text = "";
                comboSupClt.SelectedIndex = -1;
                dpChangeDate.Text = "";
                comboSupClt.SelectedIndex = -1;
                btUpdate.IsEnabled = false;
                btPrint.IsEnabled = false;
                btAdd.IsEnabled = false;
                btManagerProd.IsEnabled = false;
                MessageBox.Show("Please select a transport again. \n", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TransMaster m_trans = new TransMaster { 
                TransportNo = selectedts.Id + "",
                SupCltName = selectedts.SuppliersClient.Name,
                TransDate = selectedts.TransportDate.ToString("yyyy-MM-dd"),
                WarehouseName = selectedts.Warehouse.Name,
                UserName = selectedts.User.Name
            };
            var scs = (from w in MainWindow.ctx.StorageChanges where w.TransportID == selectedts.Id select w).ToArray();
            foreach(StorageChanx sc in scs)
            {
                m_trans.m_transDetails.Add(
                    new TransDetail
                    {
                        ProductName = sc.Product.Name,
                        Quantity = sc.Quantity,
                        Price = (decimal)sc.Price
                    }
                );
            }

            PrintPreview preview = new PrintPreview("views/TransportPrintPage.xaml", m_trans, new TransDocumentRenderer());
            preview.ShowInTaskbar = false;
            preview.ShowDialog();
        }

    }

}

