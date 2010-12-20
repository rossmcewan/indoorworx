using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Documents.Model;
using System.IO;
using Telerik.Windows.Documents.FormatProviders;
using System.Reflection;
using Telerik.Windows.Documents;
using Telerik.Windows.Documents.Layout;

namespace IndoorWorx.Library.Controls
{
    public partial class DocumentViewer : UserControl
    {
        private RadDocumentBindingSource bindingSource = new RadDocumentBindingSource();

        public DocumentViewer()
        {
            InitializeComponent();
           
        }

        public string ResourceDocument
        {
            get { return GetValue(ResourceDocumentProperty) as string; }
            set { SetValue(ResourceDocumentProperty, value); }
        }

        public static readonly DependencyProperty ResourceDocumentProperty =
            DependencyProperty.Register("ResourceDocument", typeof(string), typeof(DocumentViewer), new PropertyMetadata(string.Empty, (sender, e) =>
            {
                var doc = sender as DocumentViewer;
                doc.Initialize(e.NewValue.ToString());
            }));


        #region Methods
        
        private void Initialize(string documentName)
        {
            using (Stream stream = Application.GetResourceStream(this.GetResourceUri(documentName)).Stream)
            {
                this.LoadDocument(stream, ".doc");
            }

            this.PageViewer.DataContext = this.bindingSource;
        }

        private Uri GetResourceUri(string resource)
        {
            AssemblyName assemblyName = new AssemblyName(this.GetType().Assembly.FullName);
            string resourcePath = "/IndoorWorx.Silverlight;component/" + resource;
            Uri resourceUri = new Uri(resourcePath, UriKind.Relative);
            return resourceUri;
        }

  

        private void LoadDocument(Stream stream, string extension)
        {
            RadDocument doc;

            IDocumentFormatProvider provider = DocumentFormatProvidersManager.GetProviderByExtension(extension);
            if (provider != null)
            {
                doc = provider.Import(stream);
            }
            else
            {
                MessageBox.Show("Unknown format.");
                return;
            }

            doc.Measure(RadDocument.MAX_DOCUMENT_SIZE);
            doc.Arrange(new RectangleF(PointF.Empty, doc.DesiredSize));

            doc.LayoutMode = DocumentLayoutMode.Paged;

            doc.DefaultPageLayoutSettings.Width = 320 / 0.7F;
            doc.DefaultPageLayoutSettings.Height = 390 / 0.7F;
            doc.SectionDefaultPageMargin = new Padding(55);
            doc.Sections.First.PageMargin = new Padding(55);

            doc.UpdateLayout();

            this.ViewManager.Document = doc;
            bindingSource.Document = doc;
        }
        # endregion

    }
}
