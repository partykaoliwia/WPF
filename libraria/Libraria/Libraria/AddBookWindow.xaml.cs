using Microsoft.Win32;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Libraria
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
        }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }

        public string CoverImagePath { get; set; }
        private void ButtonClick_Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonClick_Save(object sender, RoutedEventArgs e)
        {
            BookTitle = TitleTextBox.Text;
            BookAuthor = AuthorTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void ButtonClick_Browse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog()==true)
            {
                CoverImagePath = openFileDialog.FileName;
                CoverImage.Source = new BitmapImage(new Uri(CoverImagePath));
            }
        }

        private void TitleChanged(object sender, TextChangedEventArgs e)
        {
            SaveButton.IsEnabled= !string.IsNullOrWhiteSpace(TitleTextBox.Text);

        }
    }
}
