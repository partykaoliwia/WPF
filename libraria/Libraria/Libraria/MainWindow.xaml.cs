using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Libraria
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Book> Books { get; set; }
        private Book _selectedBook;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Book SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
        }

        public MainWindow()
        {
            InitializeComponent();
            Books = new ObservableCollection<Book>();
            DataContext = this;
        }


        private void MenuItem_AddBook(object sender, RoutedEventArgs e)
        {
            AddBookWindow dialog = new AddBookWindow { Owner = this };
            if(dialog.ShowDialog()==true)
            {
                Books.Add(new Book
                {
                    Title = dialog.BookTitle,
                    Author = dialog.BookAuthor,
                    CoverImagePath = dialog.CoverImagePath,
                });
            }

        }

        private void MenuItem_DeleteBook(object sender, RoutedEventArgs e)
        {
            if (SelectedBook != null)
            {
                Books.Remove(SelectedBook);
            }
        }
    }
}