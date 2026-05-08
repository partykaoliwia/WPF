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

namespace ContactManager
{
    /// <summary>
    /// Interaction logic for AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        public Contact NewContact { get; private set; }
        public AddContactWindow()
        {
            InitializeComponent();
            NewContact = new Contact();
            DataContext = NewContact;
        }

        private void AddContact(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        public void ApplyRules(ValidationRule? nameRule, ValidationRule? surnameRule, ValidationRule? emailRule, ValidationRule? phoneRule)
        {
            void AddRuleToBinding(TextBox textBox, ValidationRule? rule)
            {
                if (rule == null) return;

                BindingExpression bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                if (bindingExpression != null)
                {
                    Binding oldBinding = bindingExpression.ParentBinding;

                    // 1. Tworzymy NOWE wiązanie na podstawie ścieżki starego (np. "Name", "Surname")
                    Binding newBinding = new Binding(oldBinding.Path.Path)
                    {
                        Mode = oldBinding.Mode,
                        UpdateSourceTrigger = oldBinding.UpdateSourceTrigger,
                        NotifyOnValidationError = true // Zgłaszaj błędy (czerwona ramka)
                    };

                    // 2. Kopiujemy stare reguły, jeśli jakieś były (dobra praktyka)
                    foreach (var r in oldBinding.ValidationRules)
                    {
                        newBinding.ValidationRules.Add(r);
                    }

                    // 3. Dodajemy naszą nową regułę wybraną w ustawieniach
                    newBinding.ValidationRules.Add(rule);

                    // 4. Przypisujemy nowe wiązanie do TextBoxa (zastępując stare)
                    textBox.SetBinding(TextBox.TextProperty, newBinding);
                }
            }

            AddRuleToBinding(NameInput, nameRule);
            AddRuleToBinding(SurnameInput, surnameRule);
            AddRuleToBinding(EmailInput, emailRule);
            AddRuleToBinding(PhoneInput, phoneRule);
        }
    }
}
