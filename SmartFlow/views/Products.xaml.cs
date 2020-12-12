using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
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
using System.Windows.Shapes;

namespace SmartFlow.views
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Page
    {
        public Products()
        {
            InitializeComponent();
            comboWarehouse.ItemsSource = (from w in MainWindow.ctx.Warehouses where w.Id == MainWindow.currentUser.Warehouse.Id select w).ToList();
            comboWarehouse.SelectedIndex = 0;
            getAllData();
        }

        private void getAllData()
        {
            lvDataBinding.ItemsSource = (from w in MainWindow.ctx.Products where w.WarehouseId == MainWindow.currentUser.Warehouse.Id select w).ToList();
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbName.Text.Length==0 || comboWarehouse.SelectedIndex == -1)
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
                btAdd.IsEnabled = false;
        }

        private void comboWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tbName.Text.Length == 0 || comboWarehouse.SelectedIndex == -1)
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
                btAdd.IsEnabled = false;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            string productname = tbName.Text;
            if (productname.Length > 100)
            {
                MessageBox.Show("The product name length must be 1-100 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string desc = tbDescription.Text;
            if (desc.Length > 300)
            {
                MessageBox.Show("The description length must be 0-300 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Product product = new Product() { Name = productname, Description=desc, Warehouse = (Warehouse)comboWarehouse.SelectedItem, Disabled = (cbDisabled.IsChecked==true) };
            MainWindow.ctx.Products.Add(product);
            
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The product cannot be added.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tbId.Text = "";
            tbName.Text = "";
            tbDescription.Text = "";
            comboWarehouse.Text = "";
            tbCost.Text = "";
            tbQuantity.Text = "";
            cbDisabled.IsChecked = false;
            btAdd.IsEnabled = false;

            getAllData();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            string productname = tbName.Text;
            if (productname.Length > 100 || productname.Length == 0)
            {
                MessageBox.Show("The product name length must be 1-100 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string desc = tbDescription.Text;
            if (desc.Length > 300)
            {
                MessageBox.Show("The description length must be 0-300 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(selectedProd==null)
            {
                MessageBox.Show("Please select a product first.\n", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            selectedProd.Name = productname;
            selectedProd.Description = desc;
            selectedProd.Disabled = (cbDisabled.IsChecked==true);

            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The product cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tbId.Text = "";
            tbName.Text = "";
            tbDescription.Text = "";
            comboWarehouse.Text = "";
            tbCost.Text = "";
            tbQuantity.Text = "";
            cbDisabled.IsChecked = false;
            btAdd.IsEnabled = false;

            selectedProd = null;
            getAllData();
            lvDataBinding.SelectedIndex = -1;
        }
        private Product selectedProd;
        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                tbId.Text = "";
                tbName.Text = "";
                tbDescription.Text = "";
                tbCost.Text = "";
                tbQuantity.Text = "";
                cbDisabled.IsChecked = false;
                btUpdate.IsEnabled = false;
                btAdd.IsEnabled = false;
                selectedProd = null;
            }
            else
            {
                selectedProd = (Product)lvDataBinding.SelectedItem;
                tbId.Text = ""+ selectedProd.Id;
                tbName.Text = selectedProd.Name;
                tbDescription.Text = selectedProd.Description;
                if (selectedProd.Cost != null)
                    tbCost.Text = ((double)selectedProd.Cost).ToString("N2");
                else
                    tbCost.Text = "";
                tbQuantity.Text = selectedProd.CurrentQuantity+"";
                cbDisabled.IsChecked = selectedProd.Disabled;
                btUpdate.IsEnabled = true;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            selectedProd = (Product)((CheckBox)sender).DataContext;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                selectedProd = null;
                MessageBox.Show("The product cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            selectedProd = null;
        }
    }
}
