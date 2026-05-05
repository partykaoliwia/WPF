using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Serialization;
using Microsoft.Win32;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Contact> Contacts { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Contacts = new ObservableCollection<Contact>();
            this.DataContext = Contacts;

        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            Close();
        }
        private void MenuItem_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To jest prosty contact manager.", "Contact Manager", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private void MenuItem_AddContact(object sender, RoutedEventArgs e)
        {
            Opacity = 0.5;
            var Window_AddContact = new AddContactWindow();
            if(Window_AddContact.ShowDialog().Value)
            {
                Contacts.Add(Window_AddContact.NewContact);
            }
            Opacity = 1;

        }
        private void MenuItem_ClearContacts(object sender, RoutedEventArgs e)
        {
            Contacts.Clear();
        }

        private void MenuItem_Import(object sender, RoutedEventArgs e)
        {
            // Otworzenie okna wyboru pliku
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml"; // Ograniczenie wyboru tylko do plików XML
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(ObservableCollection<Contact>));
                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        // Odczytanie z pliku i rzutowanie na odpowiedni typ
                        var importedContacts = (ObservableCollection<Contact>)serializer.Deserialize(reader);

                        // Zaktualizuj główną kolekcję
                        Contacts.Clear(); // Czyścimy starą listę kontaktów
                        foreach (var contact in importedContacts)
                        {
                            Contacts.Add(contact); // Dodajemy zaimportowane kontakty
                        }
                    }
                    MessageBox.Show("Kontakty zostały pomyślnie zaimportowane!", "Import zakończony", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas importu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MenuItem_Export(object sender, RoutedEventArgs e)
        {
            // Otwarcie okna zapisu pliku
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.FileName = "contacts"; // Domyślna nazwa pliku
            if(saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(ObservableCollection<Contact>));
                    using(var writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        serializer.Serialize(writer, Contacts);
                    }
                    MessageBox.Show("Kontakty zostały pomyślnie eksportowane!", "Eksport zakończony", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Wystąpił błąd podczas eksportu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void MenuItem_DeleteContact(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            var contactToDelete = menuItem?.DataContext as Contact;
            if (contactToDelete == null) return;
            if(Contacts.Contains(contactToDelete))
            {
                Contacts.Remove(contactToDelete);
            }

        }
    }
}