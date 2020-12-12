using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace SmartFlow.views
{
    /// <summary>
    /// Interaction logic for ManagerProducts.xaml
    /// </summary>
    public partial class ManagerProducts : Window
    {
        Transport transport;
        public ManagerProducts(Transport ts)
        {
            InitializeComponent();
            transport = ts;
            comboProduct.ItemsSource = (from w in MainWindow.ctx.Products where w.WarehouseId == MainWindow.currentUser.Warehouse.Id && w.Disabled == false select w).ToList();
            getAllData();
        }

        private void getAllData()
        {
            //from s in cont.Set<T>().AsNoTracking() select s;
            lvDataBinding.ItemsSource = (from w in MainWindow.ctx.StorageChanges where w.TransportID == transport.Id select w).ToList();
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
            if (p.Cost != null)
                tbCost.Text = ((double)p.Cost).ToString("0.##");
            if (p.CurrentQuantity != null)
                tbOnhandqty.Text = p.CurrentQuantity + "";
            CheckFields();
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

        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            selectedsc = (StorageChanx)((Button)sender).DataContext;
            if (selectedsc != null)
            {
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
                    getAllData();
                    selectedsc = null;
                }
            }
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
                btUpdate.IsEnabled = false;
                btAdd.IsEnabled = false;
                selectedsc = null;
            }
            else
            {
                selectedsc = (StorageChanx)lvDataBinding.SelectedItem;
                tbId.Text = selectedsc.Id + "";
                tbCost.Text = "";
                if (selectedsc.Product.Cost != null)
                {
                    tbCost.Text = ((double)selectedsc.Product.Cost).ToString("0.##");
                }

                tbOnhandqty.Text = selectedsc.Product.CurrentQuantity + "";
                tbQuantity.Text = selectedsc.Quantity.ToString();
                tbPrice.Text = selectedsc.Price + "";
                comboProduct.SelectedItem = selectedsc.Product;
                btUpdate.IsEnabled = true;
            }
        }

        private void tbQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckFields();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            Product product = (Product)comboProduct.SelectedItem;
            int quantity = int.Parse(tbQuantity.Text);
            string ctype = transport.TransType;
            if (ctype == "Out" && product.CurrentQuantity < quantity)
            {
                MessageBox.Show("The on-hand quantity less then Outbound quantity", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StorageChanx sc = new StorageChanx() { Product = product, Warehouse = MainWindow.currentUser.Warehouse, Price = double.Parse(tbPrice.Text), Quantity = quantity, ChangeDate = transport.TransportDate, ChangeType = ctype, User = MainWindow.currentUser, SuppliersClient = transport.SuppliersClient, TransportID = transport.Id };
            MainWindow.ctx.StorageChanges.Add(sc);

            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The product cannot be added.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MainWindow.ctx.Entry((Product)comboProduct.SelectedItem).Reload();

         
            resetPage();
            getAllData();
        }

        private void CheckFields()
        {
            if (comboProduct.SelectedIndex == -1 || tbPrice.Text.Length == 0 || tbQuantity.Text == "")
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

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
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
            string ctype = transport.TransType;
            if (ctype == "Out" && product.CurrentQuantity < quantity)
            {
                MessageBox.Show("The on-hand quantity less then Outbound quantity", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            selectedsc.Product = product;
            selectedsc.Warehouse = MainWindow.currentUser.Warehouse;
            selectedsc.Price = double.Parse(tbPrice.Text);
            selectedsc.Quantity = quantity;
            selectedsc.ChangeType = ctype;
            selectedsc.User = MainWindow.currentUser;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The product cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MainWindow.ctx.Entry((Product)comboProduct.SelectedItem).Reload();

            resetPage();
            getAllData();
        }

        private void resetPage()
        {
            tbId.Text = "";
            tbCost.Text = "";
            tbQuantity.Text = "";
            comboProduct.SelectedIndex = -1;
            tbOnhandqty.Text = "";
            tbPrice.Text = "";
            btUpdate.IsEnabled = false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = 800;
            double windowHeight = 600;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
