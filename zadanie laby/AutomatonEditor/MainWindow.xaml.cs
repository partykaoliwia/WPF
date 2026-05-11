using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace AutomatonEditor;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = this;

    }
    public Automaton MyAutomaton { get; set; } = new Automaton();
    private int _stateCounter = 0;
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName]string? name = null) { PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name)); }
    private State? _selectedState;
    public State? SelectedState
    {
        get => _selectedState;
        set
        {
            _selectedState = value;
            OnPropertyChanged();
        }
    }
    public void Canvas_MouseLeftButtonDown(object sender,MouseButtonEventArgs e)
    {
        if(e.ClickCount==2)
        {
            Point position = e.GetPosition((IInputElement)sender);
            State newState = new State
            {
                Name = $"q{_stateCounter}",
                X = position.X - 25,
                Y = position.Y - 25,
            };
            if(MyAutomaton.States.Count()==0)
            {
                newState.IsInitial = true;
            }
            MyAutomaton.States.Add(newState);
            _stateCounter++;
        }
        else
        {
            if(SelectedState != null)
            {
                SelectedState.IsSelected = false;
                SelectedState = null;

            }
        }
    }
    private void State_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var element = sender as FrameworkElement;
        if(element?.DataContext is State clickedState)
        {
            if(SelectedState!=null)
            {
                SelectedState.IsSelected = false;
            }
            SelectedState = clickedState;
            SelectedState.IsSelected = true;
            _isDragging = true;
            _draggedState = clickedState;
            _clickOffset = e.GetPosition(element);
            element.CaptureMouse();
            e.Handled = true;
        }
    }
    private bool _isDragging = false;
    private Point _clickOffset;
    private State? _draggedState;
    private void State_MouseMove(object sender, MouseEventArgs e)
    {
        if (_isDragging || _draggedState != null)
        {
                Point mousePos = e.GetPosition(ObszarRysowania);
                _draggedState.X = mousePos.X - _clickOffset.X;
                _draggedState.Y = mousePos.Y - _clickOffset.Y;
        }
    }

    private void State_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if(_isDragging)
        {
            _isDragging= false;
            _draggedState= null;
            ((FrameworkElement)sender).ReleaseMouseCapture();
        }
    }

    private void Button_DeleteState(object sender, RoutedEventArgs e)
    {
        if(SelectedState !=null)
        {
            if(SelectedState.IsInitial==true)
            {
                MessageBox.Show("Stan, który chcesz usunąć jest stanem początkowym. Wybierz inny stan, by usunąć ten.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var przejsciaDoUsuniecia = MyAutomaton.Transitions.Where(t=> t.Source ==SelectedState || t.Target == SelectedState).ToList();
            foreach(var przejscie in przejsciaDoUsuniecia)
            {
                MyAutomaton.Transitions.Remove(przejscie);
            }
            MyAutomaton.States.Remove(SelectedState);
           // _stateCounter--;
            SelectedState = null;
        }
    }


    private void Checkbox_Initial(object sender, RoutedEventArgs e)
    {

        if(InitialCheckbox.IsChecked == true)
        {
            foreach(var state in MyAutomaton.States)
            {
                if (state !=SelectedState)
                {
                    state.IsInitial = false;
                }
            }
        }
        else // musi być dokładnie jeden stan początkowy
        {
            InitialCheckbox.IsChecked = true;
            SelectedState.IsInitial = true;
            MessageBox.Show("Automat musi posiadać dokładnie jeden stan początkowy. Nie możesz go odznaczyć, możesz jedynie przypisać ten status innemu stanowi.", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }

    private void Checkbox_Accepting(object sender, RoutedEventArgs e)
    {

    }

    private void Button_AddTransition(object sender, RoutedEventArgs e)
    {
        var poczatkowy = (State?)StanPoczatkowyCombobox.SelectedItem;
        var koncowy = (State?)StanKoncowyCombobox.SelectedItem;
        if(poczatkowy==null || koncowy==null)
        {
            MessageBox.Show("Wybierz oba stany by dodać przejście", "",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            return;
        }
        if(poczatkowy==koncowy)
        {
            MessageBox.Show("Stany początkowy i końcowy muszą być różne!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }
        var przejscie = new Transition();
        przejscie.Source = poczatkowy;
        przejscie.Target = koncowy;
        MyAutomaton.Transitions.Add(przejscie);
    }
}