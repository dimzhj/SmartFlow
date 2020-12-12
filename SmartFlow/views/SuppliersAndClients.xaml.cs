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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartFlow.views
{
    /// <summary>
    /// Interaction logic for SuppliersAndClients.xaml
    /// </summary>
    public partial class SuppliersAndClients : Page
    {
        public SuppliersAndClients()
        {
            InitializeComponent();
            comboWarehouse.ItemsSource = (from w in MainWindow.ctx.Warehouses select w).ToList();
            getAllData();
        }

        private void getAllData()
        {
            lvDataBinding.ItemsSource = (from w in MainWindow.ctx.SuppliersClients select w).ToList();
        }

        private void CheckField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            checkField();
        }

        private void comboWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkField();
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            if (name.Length > 100)
            {
                MessageBox.Show("The name length must be 1-100 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string address = tbAddress.Text;
            if (address.Length > 150)
            {
                MessageBox.Show("The address length must be 1-150 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string phone = tbPhoneNumber.Text;
            if (phone.Length > 45)
            {
                MessageBox.Show("The phone length must be 1-45 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string sctype = "Supplier";
            if(rbClient.IsChecked==true)
                sctype = "Client";
            Warehouse wh = (Warehouse)comboWarehouse.SelectedItem;
            if (wh == null)
            {
                MessageBox.Show("The Warehouse must be selected", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SuppliersClient sc = new SuppliersClient() { Name = name, Address = address, PhoneNumber = phone, Warehouse = wh, Disabled = (cbDisabled.IsChecked==true), Type = sctype };
            MainWindow.ctx.SuppliersClients.Add(sc);
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The " + sctype + " cannot be added.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tbId.Text = "";
            tbName.Text = "";
            tbAddress.Text = "";
            tbPhoneNumber.Text = "";
            comboWarehouse.Text = "";
            btAdd.IsEnabled = false;

            getAllData();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            if (name.Length > 100)
            {
                MessageBox.Show("The name length must be 1-100 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string address = tbAddress.Text;
            if (address.Length > 150)
            {
                MessageBox.Show("The address length must be 1-150 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string phone = tbPhoneNumber.Text;
            if (phone.Length > 45)
            {
                MessageBox.Show("The phone length must be 1-45 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string sctype = "Supplier";
            if (rbClient.IsChecked == true)
                sctype = "Client";

            selectedsc.Name = name;
            selectedsc.Address = address;
            selectedsc.PhoneNumber = phone;
            selectedsc.Warehouse = (Warehouse)comboWarehouse.SelectedItem;
            selectedsc.Disabled = (cbDisabled.IsChecked==true);
            selectedsc.Type = sctype;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The "+ sctype + " cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tbId.Text = "";
            tbName.Text = "";
            tbAddress.Text = "";
            tbPhoneNumber.Text = "";
            comboWarehouse.Text = "";
            btUpdate.IsEnabled = false;
            btAdd.IsEnabled = false;
            selectedsc = null;
            getAllData();
            lvDataBinding.SelectedIndex = -1;
        }

        SuppliersClient selectedsc;

        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                tbId.Text = "";
                tbName.Text = "";
                tbAddress.Text = "";
                tbPhoneNumber.Text = "";
                comboWarehouse.Text = "";
                rbSupplier.IsChecked = true;
                cbDisabled.IsChecked = false;
                btUpdate.IsEnabled = false;
                btAdd.IsEnabled = false;
                selectedsc = null;
            }
            else
            {
                selectedsc = (SuppliersClient)lvDataBinding.SelectedItem;
                tbId.Text = selectedsc.Id+"";
                tbName.Text = selectedsc.Name;
                tbAddress.Text = selectedsc.Address;
                tbPhoneNumber.Text = selectedsc.PhoneNumber;
                comboWarehouse.Text = selectedsc.Warehouse.Name;
                cbDisabled.IsChecked = selectedsc.Disabled;
                if (selectedsc.Type == "Supplier")
                    rbSupplier.IsChecked = true;
                else
                    rbClient.IsChecked = true;
                btUpdate.IsEnabled = true;
            }
        }
/*
        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            selectedsc = (SuppliersClient)((Button)sender).DataContext;
            if(selectedsc!=null)
            {
                selectedsc.Disabled = true;
                getAllData();
                selectedsc = null;
            }
*//*            if (selectedsc != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete: \n" + selectedsc.Name, "Delete Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    try
                    {
                        MainWindow.ctx.SuppliersClients.Remove(selectedsc);
                        MainWindow.ctx.SaveChanges();
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show("Deletion cannot finished: \n" + ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    getAllData();
                    selectedsc = null;
                }
            }*//*
        }*/

        private void checkField()
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
                btUpdate.IsEnabled = false;
            }
            else
            {
                btAdd.IsEnabled = false;
                btUpdate.IsEnabled = true;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            selectedsc = (SuppliersClient)((CheckBox)sender).DataContext;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                selectedsc = null;
                MessageBox.Show("The supplier/client cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            selectedsc = null;
        }
    }
}
