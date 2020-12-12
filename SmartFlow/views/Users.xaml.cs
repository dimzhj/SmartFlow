using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Page
    {
        public Users()
        {
            InitializeComponent();
            comboWarehouse.ItemsSource = (from w in MainWindow.ctx.Warehouses select w).ToList();
            getAllData();
        }
        private void getAllData()
        {
            lvDataBinding.ItemsSource = (from w in MainWindow.ctx.Users select w).ToList();
        }

        private void btImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "Image Files|*.jpg;*.png;*.bmp;*.gif";
            if (openFileDlg.ShowDialog() == true)
            {
                btImage.Source = new BitmapImage(new Uri(openFileDlg.FileName));
            }
        }

        public static ImageSource ByteToImage(byte[] imageData)
        {
            if (imageData == null)
                return null;
            try
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(imageData);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                ImageSource imgSrc = biImg as ImageSource;

                return imgSrc;
            }
            catch (NotSupportedException ex)
            {
                return null;
            }
            catch (InvalidOperationException ex)
            {
                return null;
            }
        }

        private User selecteduser;

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            if (name.Length > 25)
            {
                MessageBox.Show("The name length must be 1-25 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string username = tbUserName.Text;
            if (username.Length > 10)
            {
                MessageBox.Show("The username length must be 1-10 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string psw = pwbPassword.Password;
            if (psw.Length > 10)
            {
                MessageBox.Show("The password length must be 1-10 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            byte[] bytes = null;
            var bitmapSource = (BitmapSource)btImage.Source;
            if (bitmapSource != null)
            {
                try
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    using (var stream = new MemoryStream())
                    {
                        encoder.Save(stream);
                        bytes = stream.ToArray();
                    }
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("The photo cannot be accessed.\n" + ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            User user = new User() { Name = name, UserName = username, Password = psw, Warehouse = (Warehouse)comboWarehouse.SelectedItem, Photo = bytes, IsAdmin = cbIsadmin.IsChecked, Language = "EN" };
            MainWindow.ctx.Users.Add(user);
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The user cannot be added.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tbId.Text = "";
            tbName.Text = "";
            tbUserName.Text = "";
            pwbPassword.Password = "";
            comboWarehouse.Text = "";
            btAdd.IsEnabled = false;

            getAllData();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            if (name.Length > 25)
            {
                MessageBox.Show("The name length must be 1-25 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string username = tbUserName.Text;
            if (username.Length > 10)
            {
                MessageBox.Show("The username length must be 1-10 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string psw = pwbPassword.Password;
            if (psw.Length > 10)
            {
                MessageBox.Show("The password length must be 1-10 chars", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            byte[] bytes = null;
            var bitmapSource = (BitmapSource)btImage.Source;
            if (bitmapSource != null)
            {
                try
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                    using (var stream = new MemoryStream())
                    {
                        encoder.Save(stream);
                        bytes = stream.ToArray();
                    }
                }
                catch (SystemException ex)
                {
                    MessageBox.Show("The photo cannot be accessed.\n" + ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            selecteduser.Name = name;
            selecteduser.UserName = username;
            selecteduser.Password = psw;
            selecteduser.Warehouse = (Warehouse)comboWarehouse.SelectedItem;
            selecteduser.IsAdmin = cbIsadmin.IsChecked;
            selecteduser.Photo = bytes;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                MessageBox.Show("The user cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            tbUserName.Text = "";
            tbId.Text = "";
            tbName.Text = "";            
            pwbPassword.Password = "";
            cbIsadmin.IsChecked = false;
            comboWarehouse.Text = "";
            btImage.Source = null;
            btUpdate.IsEnabled = false;
            btAdd.IsEnabled = false;
            selecteduser = null;
            getAllData();
            lvDataBinding.SelectedIndex = -1;
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            selecteduser = (User)((Button)sender).DataContext;
            if (selecteduser != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete: \n" + selecteduser.Name, "Delete Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    try
                    {
                        MainWindow.ctx.Users.Remove(selecteduser);
                        string msg = MainWindow.ContextSave();
                        if (msg != "")
                        {
                            MessageBox.Show("The user cannot be deleted.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show("Deletion cannot finished: \n" + ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    getAllData();
                    selecteduser = null;
                }
            }
        }

        private void CheckField_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            checkField();
        }

        private void lvDataBinding_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDataBinding.SelectedIndex == -1)
            {
                tbId.Text = "";
                tbName.Text = "";
                tbUserName.Text = "";
                pwbPassword.Password = "";
                btImage.Source = null;
                cbIsadmin.IsChecked = false;
                btUpdate.IsEnabled = false;
                btAdd.IsEnabled = false;
                selecteduser = null;
            }
            else
            {
                selecteduser = (User)lvDataBinding.SelectedItem;
                tbId.Text = selecteduser.Id + "";
                tbName.Text = selecteduser.Name;
                tbUserName.Text = selecteduser.UserName;
                pwbPassword.Password = selecteduser.Password;
                comboWarehouse.Text = selecteduser.Warehouse.Name;
                btImage.Source = ByteToImage(selecteduser.Photo);
                cbIsadmin.IsChecked = selecteduser.IsAdmin;
                btUpdate.IsEnabled = true;
                //btDelete.IsEnabled = true;
            }
        }

        private void pwbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            checkField();
        }

        private void comboWarehouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkField();
        }

        private void checkField()
        {
            if (tbName.Text == "" || tbUserName.Text == "" || pwbPassword.Password == "" || comboWarehouse.SelectedIndex == -1 )
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
            selecteduser = (User)((CheckBox)sender).DataContext;
            string msg = MainWindow.ContextSave();
            if (msg != "")
            {
                selecteduser = null;
                MessageBox.Show("The user cannot be updated.\n" + msg, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            selecteduser = null;
        }
    }


}
