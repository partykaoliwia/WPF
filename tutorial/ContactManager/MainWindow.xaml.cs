using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using Microsoft.Win32;
using ContactManager.Validation;

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        private bool _isValidationUnlocked;
        public bool IsValidationUnlocked
        { 
            get 
            { return _isValidationUnlocked; } 
            set 
            { _isValidationUnlocked = value; OnPropertyChanged(nameof(IsValidationUnlocked)); } } 
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainWindow()
        {
            InitializeComponent();
            Contacts = new ObservableCollection<Contact>();
            this.DataContext = this;

        }
        private void ToggleValidationSettings_Click(object sender, RoutedEventArgs e)
        {
            IsValidationUnlocked = !IsValidationUnlocked;
            if (IsValidationUnlocked)
            {
                LoadRulesFromDLLs();
            }
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
            // Przekazujemy reguły do nowego okna
            Window_AddContact.ApplyRules(
                SelectedNameRule as ValidationRule,
                SelectedSurnameRule as ValidationRule,
                SelectedEmailRule as ValidationRule,
                SelectedPhoneRule as ValidationRule
            );
            if (Window_AddContact.ShowDialog().Value)
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
        // Kolekcja wszystkich reguł znalezionych w DLL-kach
        public ObservableCollection<IValidation> AvailableRules { get; set; } = new();

        // Właściwości przechowujące wybraną regułę dla każdego pola
        private IValidation? _selectedNameRule;
        public IValidation? SelectedNameRule
        {
            get => _selectedNameRule;
            set { _selectedNameRule = value; OnPropertyChanged(nameof(SelectedNameRule)); }
        }

        // ... analogicznie dla Surname, Email i Phone ...
        private IValidation? _selectedSurnameRule;
        public IValidation? SelectedSurnameRule { get => _selectedSurnameRule; set { _selectedSurnameRule = value; OnPropertyChanged(nameof(SelectedSurnameRule)); } }

        private IValidation? _selectedEmailRule;
        public IValidation? SelectedEmailRule { get => _selectedEmailRule; set { _selectedEmailRule = value; OnPropertyChanged(nameof(SelectedEmailRule)); } }

        private IValidation? _selectedPhoneRule;
        public IValidation? SelectedPhoneRule { get => _selectedPhoneRule; set { _selectedPhoneRule = value; OnPropertyChanged(nameof(SelectedPhoneRule)); } }


        private void LoadRulesFromDLLs()
        {
            AvailableRules.Clear();
            // Szukamy w folderze, gdzie znajduje się nasz plik .exe
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] dllFiles = Directory.GetFiles(path, "*.dll");

            foreach (string file in dllFiles)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    // Szukamy klas, które implementują IValidation i nie są interfejsami/abstrakcyjne
                    var rules = assembly.GetTypes()
                        .Where(t => typeof(IValidation).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var ruleType in rules)
                    {
                        var ruleInstance = Activator.CreateInstance(ruleType) as IValidation;
                        if (ruleInstance != null)
                        {
                            AvailableRules.Add(ruleInstance);
                        }
                    }
                }
                catch { /* Niektóre DLL mogą nie być bibliotekami .NET lub być zablokowane */ }
            }
        }


    }
}