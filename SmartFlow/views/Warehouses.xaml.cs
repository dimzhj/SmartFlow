using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Warehouses.xaml
    /// </summary>
    public partial class Warehouses : Page
    {

        public Warehouses()
        {
            InitializeComponent();
            getAllData();
        }

        private void getAllData()
        {
            lvDataBinding.ItemsSource = (from w in MainWindow.ctx.Warehouses select w).ToList();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            if (name.Length > 50)
            {
                MessageBox.Show("The name length must be 1-50 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string address = tbAddress.Text;
            if (address.Length > 150)
            {
                MessageBox.Show("The address length must be 1-150 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string phonenumber = tbPhoneNumber.Text;
            if (phonenumber.Length > 45)
            {
                MessageBox.Show("The phone number length must be 1-45 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            wh.Name = name;
            wh.Address = address;
            wh.PhoneNumber = phonenumber;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The warehouse cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tbId.Text = "";
            tbName.Text = "";
            tbAddress.Text = "";
            tbPhoneNumber.Text = "";
            //btDelete.IsEnabled = false;
            btUpdate.IsEnabled = false;
            getAllData();
            wh = null;
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            if(name.Length>50)
            {
                MessageBox.Show("The name length must be 1-50 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string address = tbAddress.Text;
            if (address.Length > 150)
            {
                MessageBox.Show("The address length must be 1-150 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string phonenumber = tbPhoneNumber.Text;
            if (phonenumber.Length > 45)
            {
                MessageBox.Show("The phone number length must be 1-45 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Warehouse warehouse = new Warehouse() { Name = name, Address = address, PhoneNumber = phonenumber };
            MainWindow.ctx.Warehouses.Add(warehouse);
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The warehouse cannot be added.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tbId.Text = "";
            tbName.Text = "";
            tbAddress.Text = "";
            tbPhoneNumber.Text = "";
            btAdd.IsEnabled = false;

            getAllData();
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            wh = (Warehouse)((Button)sender).DataContext;
            if (wh != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete: \n" + wh.Name, "Delete Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    try
                    {
                        MainWindow.ctx.Warehouses.Remove(wh);
                        string msg = MainWindow.ContextSave();
                        if (msg != "")
                        {
                            MessageBox.Show("The warehouse cannot be deleted.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show("Deletion cannot finished: \n" + ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    getAllData();
                    wh = null;
                }
            }
        }

        private Warehouse wh;

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbName.Text.Length == 0)
            {
                btAdd.IsEnabled = false;
                btUpdate.IsEnabled = false;
                return;
            }                

            if (tbId.Text=="")
            {
                btAdd.IsEnabled = true;
            }
            else
                btUpdate.IsEnabled = true;
        }

        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                tbId.Text = "";
                tbName.Text = "";
                tbAddress.Text = "";
                tbPhoneNumber.Text = "";
                //btDelete.IsEnabled = false;
                btUpdate.IsEnabled = false;
                btAdd.IsEnabled = false;
                wh = null;
            }
            else
            {
                wh = (Warehouse)lvDataBinding.SelectedItem;
                tbId.Text = wh.Id + "";
                tbName.Text = wh.Name;
                tbAddress.Text = wh.Address;
                tbPhoneNumber.Text = wh.PhoneNumber;
                btUpdate.IsEnabled = true;
                //btDelete.IsEnabled = true;
            }
        }

    }
}
