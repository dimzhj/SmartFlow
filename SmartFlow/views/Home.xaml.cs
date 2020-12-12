using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartFlow.views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            tbWelcome.Text = MainWindow.currentUser.Name + "," + tbWelcome.Text;
            ImageSource img = views.Users.ByteToImage(MainWindow.currentUser.Photo);

            //Point p = new Point(img.Width / 2, img.Height / 2);
/*            elliPhoto.Center = p;
            elliPhoto.RadiusX = Math.Min(p.X,p.Y)*2;
            elliPhoto.RadiusY = Math.Min(p.X, p.Y)*2;*/
            
/*            userPhoto.Width = Math.Min(p.X, p.Y);
            userPhoto.Height = Math.Min(p.X, p.Y);*/
            userPhoto.Source = img;
        }
    }
}
