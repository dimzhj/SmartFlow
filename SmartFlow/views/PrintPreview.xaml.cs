using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
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
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace SmartFlow.views
{
    /// <summary>
    /// Interaction logic for PrintPreview.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {

        private delegate void LoadXpsMethod();
        private readonly Object m_data;
        private readonly FlowDocument m_doc;

        public PrintPreview(string strTmplName, Object data, IDocumentRenderer renderer = null)
        {
            InitializeComponent();
            m_data = data;
            m_doc = LoadDocumentAndRender(strTmplName, data, renderer);
            Dispatcher.BeginInvoke(new LoadXpsMethod(LoadXps), DispatcherPriority.ApplicationIdle);
        }

        public static FlowDocument LoadDocumentAndRender(string strTmplName, Object data, IDocumentRenderer renderer = null)
        {
            FlowDocument doc = (FlowDocument)Application.LoadComponent(new Uri(strTmplName, UriKind.RelativeOrAbsolute));
            doc.PagePadding = new Thickness(50);
            doc.DataContext = data;
            if (renderer != null)
            {
                renderer.Render(doc, data);
            }
            return doc;
        }

        public void LoadXps()
        {        
            MemoryStream ms = new MemoryStream();        
            Package package = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);        
            Uri DocumentUri = new Uri("pack://InMemoryDocument.xps");        
            PackageStore.RemovePackage(DocumentUri);        
            PackageStore.AddPackage(DocumentUri, package);
            XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Fast, DocumentUri.AbsoluteUri); 
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            writer.Write(((IDocumentPaginatorSource)m_doc).DocumentPaginator); 
            docViewer.Document = xpsDocument.GetFixedDocumentSequence();
            xpsDocument.Close();
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
