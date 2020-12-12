using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for StorageChange.xaml
    /// </summary>
    public partial class StorageChange : Page
    {
        public StorageChange()
        {
            InitializeComponent();
            comboWarehouse.ItemsSource = (from w in MainWindow.ctx.Warehouses select w).ToList();
            comboProduct.ItemsSource = (from w in MainWindow.ctx.Products where w.WarehouseId == MainWindow.currentUser.Warehouse.Id && w.Disabled==false select w).ToList();
            comboSupClt.ItemsSource = (from w in MainWindow.ctx.SuppliersClients where w.WarehouseID == MainWindow.currentUser.Warehouse.Id && w.Disabled == false select w).ToList();
            comboWarehouse.SelectedItem = MainWindow.currentUser.Warehouse;
            getAllData();
        }

        private void getAllData()
        {
            //from s in cont.Set<T>().AsNoTracking() select s;
            lvDataBinding.ItemsSource = (from w in MainWindow.ctx.StorageChanges where w.WarehouseId == MainWindow.currentUser.Warehouse.Id && w.TransportID==null select w).ToList();
        }

        private void comboProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == -1)
            {
                btAdd.IsEnabled = false;
                btUpdate.IsEnabled = false;
                return;
            }
            tbCost.Text = "";
            tbOnhandqty.Text = "";
            Product p = (Product)((ComboBox)sender).SelectedItem;
            if(p.Cost!=null)
                tbCost.Text = ((double)p.Cost).ToString("0.##");
            if (p.CurrentQuantity != null)
                tbOnhandqty.Text = p.CurrentQuantity + "";
            CheckFields();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)comboProduct.SelectedItem;
            int quantity = int.Parse(tbQuantity.Text);
            string ctype = (rbIn.IsChecked == true) ? "In" : "Out";
            if (ctype == "Out" && product.CurrentQuantity<quantity)
            {
                MessageBox.Show("The on-hand quantity less then Outbound quantity", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StorageChanx sc = new StorageChanx() { Product = product, Warehouse = (Warehouse)comboWarehouse.SelectedItem, Price = double.Parse(tbPrice.Text), Quantity = quantity, ChangeDate = DateTime.Parse(dpChangeDate.SelectedDate.ToString()), ChangeType = ctype, User = MainWindow.currentUser, SuppliersClient = (SuppliersClient)comboSupClt.SelectedItem};
            MainWindow.ctx.StorageChanges.Add(sc);

            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The product cannot be added.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MainWindow.ctx.Entry((Product)comboProduct.SelectedItem).Reload();

            tbId.Text = "";
            tbCost.Text = "";
            tbQuantity.Text = "";
            comboProduct.SelectedIndex = -1;
            comboSupClt.SelectedIndex = -1;
            tbOnhandqty.Text = "";
            tbPrice.Text = "";
            dpChangeDate.Text = "";
            rbIn.IsChecked = true;
            btUpdate.IsEnabled = false;

            getAllData();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (selectedsc == null)
            {
                btUpdate.IsEnabled = false;
                MessageBox.Show("Please select a change.\n", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product product = (Product)comboProduct.SelectedItem;
            int quantity = int.Parse(tbQuantity.Text);
            string ctype = (rbIn.IsChecked == true) ? "In" : "Out";
            if (ctype == "Out" && product.CurrentQuantity < quantity)
            {
                MessageBox.Show("The on-hand quantity less then Outbound quantity", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            selectedsc.Product = product;
            selectedsc.Warehouse = MainWindow.currentUser.Warehouse;
            selectedsc.Price = double.Parse(tbPrice.Text);
            selectedsc.Quantity = quantity;
            selectedsc.ChangeDate = DateTime.Parse(dpChangeDate.SelectedDate.ToString());
            selectedsc.ChangeType = ctype;
            selectedsc.User = MainWindow.currentUser;
            selectedsc.SuppliersClient = (SuppliersClient)comboSupClt.SelectedItem;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The product cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MainWindow.ctx.Entry((Product)comboProduct.SelectedItem).Reload();

            tbId.Text = "";
            tbCost.Text = "";
            tbQuantity.Text = "";
            comboProduct.SelectedIndex = -1;
            comboSupClt.SelectedIndex = -1;
            tbOnhandqty.Text = "";
            tbPrice.Text = "";
            dpChangeDate.Text = "";
            rbIn.IsChecked = true;
            btUpdate.IsEnabled = false;
            getAllData();
        }
        StorageChanx selectedsc;
        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                tbId.Text = "";
                tbCost.Text = "";
                tbQuantity.Text = "";
                tbOnhandqty.Text = "";
                tbPrice.Text = "";
                comboProduct.SelectedIndex = -1;
                comboSupClt.SelectedIndex = -1;
                rbIn.IsChecked = true;
                btUpdate.IsEnabled = false;
                btAdd.IsEnabled = false;
                selectedsc = null;
            }
            else
            {
                selectedsc = (StorageChanx)lvDataBinding.SelectedItem;
                tbId.Text = selectedsc.Id+"";
                tbCost.Text = "";
                if (selectedsc.Product.Cost!=null)
                {
                    tbCost.Text = ((double)selectedsc.Product.Cost).ToString("0.##");
                }
                if (selectedsc.ChangeType == "In")
                    rbIn.IsChecked = true;
                else
                    rbOut.IsChecked = true;
                tbOnhandqty.Text = selectedsc.Product.CurrentQuantity + "";
                tbQuantity.Text = selectedsc.Quantity.ToString();
                tbPrice.Text = selectedsc.Price+"";
                dpChangeDate.Text = selectedsc.ChangeDate.ToString();
                comboProduct.SelectedItem = selectedsc.Product;
                comboSupClt.SelectedItem = selectedsc.SuppliersClient;
                btUpdate.IsEnabled = true;
            }
        }

        private void tbQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
            CheckFields();
        }

        private void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
            CheckFields();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            selectedsc = (StorageChanx)((Button)sender).DataContext;
            if (selectedsc != null)
            {
                Product product = selectedsc.Product;
                int quantity = selectedsc.Quantity;
                string ctype = selectedsc.ChangeType;
                if (ctype == "In" && product.CurrentQuantity < quantity)
                {
                    MessageBox.Show("The on-hand quantity less then Inbound quantity, this change cannot be deleted", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete: \n" + selectedsc.Product.Name, "Delete Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    try
                    {
                        MainWindow.ctx.StorageChanges.Remove(selectedsc);
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
                    MainWindow.ctx.Entry((Product)comboProduct.SelectedItem).Reload();
                    getAllData();
                    selectedsc = null;
                }
            }
        }

        private void CheckFields()
        {
            if(comboProduct.SelectedIndex == -1 || tbPrice.Text.Length == 0 || tbQuantity.Text == "" || comboSupClt.SelectedIndex == -1 || dpChangeDate.Text == "")
            {
                btAdd.IsEnabled = false;
                btUpdate.IsEnabled = false;
                return;
            }

            if (tbId.Text == "")
            {
                btAdd.IsEnabled = true;
            }
            else
                btUpdate.IsEnabled = true;
        }

        private void rbIn_Click(object sender, RoutedEventArgs e)
        {
            CheckFields();
        }

        private void dpChangeDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckFields();
        }

        private void comboSupClt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckFields();
        }

        private void tbPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFields();
        }

        private bool productFilter(object item)
        {
            bool checkvalue = true;
            if (!String.IsNullOrEmpty(tbSearch.Text))
                checkvalue = (item as StorageChanx).Product.Name.IndexOf(tbSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            if (FromDate.Text != "")
                checkvalue = checkvalue && (item as StorageChanx).ChangeDate >= FromDate.SelectedDate;
            if (ToDate.Text != "")
                checkvalue = checkvalue && (item as StorageChanx).ChangeDate <= ToDate.SelectedDate;
            return checkvalue;
        }


        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = tbSearch.Text;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvDataBinding.ItemsSource);
            view.Filter = productFilter;
        }

        private void SearchDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvDataBinding.ItemsSource);
            view.Filter = productFilter;
        }
    }
}
