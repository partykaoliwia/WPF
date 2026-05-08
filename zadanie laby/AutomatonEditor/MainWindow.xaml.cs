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
            MyAutomaton.States.Remove(SelectedState);
           // _stateCounter--;
            SelectedState = null;
        }
    }
}