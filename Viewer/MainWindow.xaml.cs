using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Microsoft.Win32;
using SyntaxTreeGen.Models;
using SyntaxTreeGen.XML;

namespace Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string FormattedXml { get; private set; }
        private ClassNode _nodes;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "XML Document (.xml) | *.xml",
                Multiselect = false
            };

            dialog.ShowDialog();

            // If dialog closes without a file being selected, do nothing
            if (string.IsNullOrEmpty(dialog.FileName))
            {
                return;
            }

            // Open the XML document - present errors found to the user
            Document parserDoc;

            try
            {
                parserDoc = new Document(dialog.FileName);
            }
            catch (XmlException ex)
            {
                // Can't continue to parsing stage if an error occurs, break here
                MessageBox.Show($"This file cannot be opened as {ex.Message}");
                return;
            }

            // Parse the XML document
            try
            {
                _nodes = parserDoc.GetNodes();

            }
            catch (XmlException ex)
            {
                // Show line number if available
                var msg = ex.LineNumber != 0
                    ? $"This file cannot be paarsed as {ex.Message}. Line {ex.LineNumber}, Position {ex.LinePosition}"
                    : $"This file cannot be parsed as {ex.Message}.";

                MessageBox.Show(msg);
            }

            // Show XML and code
            //XmlViewer.Navigate(Path.GetFullPath(dialog.FileName));
            XmlViewer.Text = FormatXmlFile(Path.GetFullPath(dialog.FileName));
            CodeViewer.Text = Node.FormatCSharp(_nodes.ToString());

            var res = Node.FormatCSharp(_nodes.ToString());

            // Hide overlay
            Overlay.Visibility = Visibility.Hidden;

            // Add filename to window title
            Title += " - " + Path.GetFileName(dialog.FileName);
        }

        private void Save(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Parses and formats an XML file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <remarks>
        /// Adapted from: http://stackoverflow.com/a/20075446/2997408
        /// </remarks>
        private static string FormatXmlFile(string path)
        {
            var doc = new XmlDocument();
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            doc.Load(fs);

            var sb = new StringBuilder();

            using (var wr = new XmlTextWriter(new StringWriter(sb)) {Formatting = Formatting.Indented})
                doc.Save(wr);
            
            return sb.ToString();
        }
    }
}