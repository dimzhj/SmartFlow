using MySql.Data.MySqlClient;
using SmartFlow.views;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace SmartFlow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static public User currentUser;
        static public testJianZhanagEntities ctx = new testJianZhanagEntities();
        public MainWindow()
        {
            InitializeComponent();
            userImage.Source = views.Users.ByteToImage(currentUser.Photo);
            userPhotoBorder.ToolTip = currentUser.Name;
            if (currentUser.IsAdmin == false)
            {
                lbiAdmin.Visibility = Visibility.Hidden;
                lbiAdmin.IsEnabled = false;
                ExpAdmin.Visibility = Visibility.Hidden;
                ExpAdmin.IsEnabled = false;
                adminIcon.Visibility = Visibility.Hidden;
            }
            comboxLanguage.SelectedIndex = 0;
        }

        private void btNavigation_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);

            Point relativePoint = ((Button)e.Source).TransformToVisual(btHome)
                          .Transform(new Point(0, 0));

            GridCursorOrage.Margin = new Thickness(0, relativePoint.Y, 0, 0);

            switch (index)
            {
                case 0:
                    showFrame.Source = new Uri("views/Home.xaml", UriKind.Relative);
                    break;
                case 1:
                    showFrame.Source = new Uri("views/StorageChange.xaml", UriKind.Relative);
                    break;
                case 2:
                    showFrame.Source = new Uri("views/TransportPage.xaml", UriKind.Relative);
                    break;
                case 3:
                    showFrame.Source = new Uri("views/Products.xaml", UriKind.Relative);
                    break;
                case 4:
                    showFrame.Source = new Uri("views/StatiscInOut.xaml", UriKind.Relative);
                    break;
                case 5:
                    showFrame.Source = new Uri("views/StatiscBar.xaml", UriKind.Relative);
                    break;
                case 6:
                    showFrame.Source = new Uri("views/Warehouses.xaml", UriKind.Relative);
                    break;
                case 7:
                    showFrame.Source = new Uri("views/Users.xaml", UriKind.Relative);
                    break;
                case 8:
                    showFrame.Source = new Uri("views/SuppliersAndClients.xaml", UriKind.Relative);
                    break;
            }
        }

        private void btSignOut_Click(object sender, RoutedEventArgs e)
        {
            currentUser = null;
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void btMax_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void btClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        public static string ContextSave()
        {
            try
            {
                MainWindow.ctx.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                UpdateException updateException = (UpdateException)ex.InnerException;
                MySqlException sqlException = (MySqlException)updateException.InnerException;
                if (sqlException != null)
                    return sqlException.Message;
                else
                    return "Unkown error from db";
            }
            catch(DbEntityValidationException ex)
            {
                return ex.Message;
            }
            return "";
        }

        private void comboxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int culture = ((ComboBox)sender).SelectedIndex;
            if(culture==1)
                App.SelectCulture("FR");
            else
                App.SelectCulture("EN");
        }
    }
}
