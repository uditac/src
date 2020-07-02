using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf.App.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            Nullable<bool> result = openFileDlg.ShowDialog();
            if (result == true)
            {
                txtFile.Text = openFileDlg.FileName;
                BindGrid(openFileDlg);
                HighlightRows();
            }
        }

        private void BindGrid(OpenFileDialog file)
        {
            dgBooks.ItemsSource = CsvHelper.GetCSVContent(file.FileName);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<DataRowView> lstDelete = new List<DataRowView>();
            foreach(DataRowView rowview in dgBooks.Items)
            {
                if (rowview.Row.Field<string>("InStock") != "True")
                {
                    lstDelete.Add(rowview);
                }
            }
            foreach(DataRowView rowview in lstDelete)
            {
                DataView dataView = dgBooks.ItemsSource as DataView;
                dataView.Table.Rows.Remove(rowview.Row);
            }
        }

        private void HighlightRows()
        {
            
            for (int i = 0; i < dgBooks.Items.Count; i++)
            {
                dgBooks.UpdateLayout();
                dgBooks.ScrollIntoView(dgBooks.Items[i]);
                DataRowView rowview = dgBooks.Items[i] as DataRowView;
                DataGridRow row = (DataGridRow)dgBooks.ItemContainerGenerator.ContainerFromIndex(i);
                var val = rowview.Row.Field<string>("InStock");
                if (val != null && val.Equals("False"))
                {
                    object item = dgBooks.Items[i];           
                    var converter = new System.Windows.Media.BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#FFFFFF90");
                    row.Background = brush;
                   
                }

               

            }


      

        }
    }
}
