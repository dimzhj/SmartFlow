using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SmartFlow
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = Username.Text;
            string password = Password.Password;

            var tempUser = MainWindow.ctx.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (tempUser == null)
            {
                tbInvalidInfor.Text= "Invalid username or password";
                return;
            }
            MainWindow.currentUser = tempUser;

            this.IsEnabled = false;
            LoginDialog.OpacityMask = this.Resources["ClosedBrush"] as LinearGradientBrush;
            Storyboard std = this.Resources["ClosedStoryboard"] as Storyboard;
            std.Completed += delegate { this.Close(); };
            std.Begin();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.5) };
            timer.Start();
            timer.Tick += (sender1, args) =>
            {
                timer.Stop();
                MainWindow m = new MainWindow();
                m.Show();
                this.Close();
            };
        }
    }
}
