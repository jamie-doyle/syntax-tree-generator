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
        private Document _parserDoc;
        private string _fileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                return;
            
            // Open the XML document - present errors found to the user
            try
            {
                _parserDoc = new Document(dialog.FileName);
            }
            catch (XmlException ex)
            {
                // Can't continue to parsing stage if an error occurs, break here
                MessageBox.Show($"This file cannot be opened as {ex.Message}");
                return;
            }
            
            // Show XML 
            XmlViewer.Text = FormatXmlFile(Path.GetFullPath(dialog.FileName));

            // Hide overlay and clear previous file
            Overlay.Visibility = Visibility.Hidden;
            CodeViewer.Text = "";

            // Add filename to window title
            _fileName = Path.GetFileName(dialog.FileName);
            Title = $"Syntax Tree Generator - {_fileName}";
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            // Check a document is loaded
            if (_parserDoc == null)
            {
                MessageBox.Show("No XML file selected. \n\nOpen a file using File > Open");
                return;
            }
            // Check code has been generated
            if (string.IsNullOrEmpty(CodeViewer.Text))
            {
                MessageBox.Show("No code has been generated. \n\nGenerate code using Run > Generate");
                return;
            }

            // Get the user's desired save location
            var sfd = new SaveFileDialog
            {
                Filter = "C# Source Code file | *.cs",
                DefaultExt = "cs"
            };

            var isPathSelected = sfd.ShowDialog();

            // Exit if no location selected
            if (isPathSelected != true) return;

            // Use a StreamWriter to add each line of generated code
            using (var sw = new StreamWriter(sfd.OpenFile()))
            {
                sw.WriteLine($"/* Generated from {_fileName} by Syntax Tree Generator on {DateTime.Now}*/");
                sw.WriteLine(CodeViewer.Text);
            }
        }

        /// <summary>
        /// Parses the nodes contained in the the current XML file, then constructs a representation in C#
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Generate(object sender, RoutedEventArgs e)
        {
            // Check a document is loaded
            if (_parserDoc == null)
            {
                MessageBox.Show("No XML file selected. \n\nOpen a file using File > Open");
                return;
            }
            
            // Parse the XML document, presenting errors to the user
            try
            {
                var nodes = _parserDoc.GetNodes();
                // Generate code from parsed tree nodes, display the result to the user
                CodeViewer.Text = nodes.ToString();
            }
            catch (XmlException ex)
            {
                // Show line number if available
                var msg = $"This file cannot be parsed as {ex.Message}.";

                MessageBox.Show(msg);
            }
        }

        /// <summary>
        /// Loads an XML file and returns its content as a formatted string 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string FormatXmlFile(string path)
        {
            var doc = new XmlDocument();
            doc.Load(new FileStream(path, FileMode.Open, FileAccess.Read));

            var sb = new StringBuilder();

            using (var wr = new XmlTextWriter(new StringWriter(sb)) { Formatting = Formatting.Indented })
            {
                doc.Save(wr);
            }

            return sb.ToString();
        }
    }
}