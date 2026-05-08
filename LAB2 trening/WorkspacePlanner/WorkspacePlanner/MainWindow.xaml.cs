using System.Collections.ObjectModel;
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

namespace WorkspacePlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<WorkspaceItem> Items { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Items = new ObservableCollection<WorkspaceItem>();
            this.DataContext = this;
        }

        private void Button_AddRectangle(object sender, RoutedEventArgs e)
        {
            var newRectangle = new WorkspaceItem
            {
                Name = "Rect " + (Items.Count + 1),
                ShapeType = "Rectangle",
                X = 50,
                Y = 50
            };
            Items.Add(newRectangle);
            var rect = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.DarkMagenta,
                Stroke= Brushes.Black,
                StrokeThickness = 1,
            };

            rect.DataContext = newRectangle; // !!!

            Binding bindX = new Binding("X")
            {
                Source = newRectangle,
                Mode = BindingMode.TwoWay,
            };
            Binding bindY = new Binding("Y")
            {
                Source = newRectangle,
                Mode = BindingMode.TwoWay,
            };
            rect.SetBinding(Canvas.LeftProperty, bindX);
            rect.SetBinding(Canvas.TopProperty, bindY);

            rect.MouseDown += Shape_MouseDown;
            rect.MouseMove += Shape_MouseMove;
            rect.MouseUp += Shape_MouseUp;

            AttachContextMenu(rect);

            WorkspaceCanvas.Children.Add(rect);
        }
        private void Shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _selectedShape = sender as Shape;
            if (_selectedShape == null) return;
            var item = _selectedShape.DataContext as WorkspaceItem;
            ShapesDataGrid.SelectedItem = item;

            _isDragging = true;
            _selectedShape.CaptureMouse();
            _selectedShape.StrokeThickness = 3;
            _clickOffset = e.GetPosition(_selectedShape); // gdzie wewnatrz ksztaltu kliknelismy
        }
        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging || _selectedShape == null) return;
            var item = _selectedShape.DataContext as WorkspaceItem;
            if (item == null) return;
            Point mousePos = e.GetPosition(WorkspaceCanvas); // pozycja myszy wzgledem plotna
            item.X = mousePos.X - _clickOffset.X;
            item.Y = mousePos.Y - _clickOffset.Y;
        }
        private void Shape_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if(!_isDragging || _selectedShape == null) return;

            _isDragging = false;
            _selectedShape.ReleaseMouseCapture();
            _selectedShape.StrokeThickness = 1;
            _selectedShape = null;
        }


        private void Button_AddEllipse(object sender, RoutedEventArgs e)
        {
            var newEllipse = new WorkspaceItem
            {
                Name = "Eli " + (Items.Count + 1),
                ShapeType = "Elipse",
                X = 50, Y = 50
            };
            Items.Add(newEllipse);
            var elli = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Green,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            elli.DataContext = newEllipse; // !!!!

            Binding bindX = new Binding("X")
            {
                Source = newEllipse,
                Mode = BindingMode.TwoWay,
            };
            Binding bindY = new Binding("Y")
            {
                Source = newEllipse,
                Mode = BindingMode.TwoWay,
            };
            elli.SetBinding(Canvas.LeftProperty, bindX);
            elli.SetBinding(Canvas.TopProperty, bindY);
            elli.MouseDown += Shape_MouseDown;
            elli.MouseMove += Shape_MouseMove;
            elli.MouseUp += Shape_MouseUp;

            AttachContextMenu(elli);

            WorkspaceCanvas.Children.Add(elli);
        }

        private bool _isDragging;
        private Point _clickOffset;
        private Shape _selectedShape;
        private int _topZIndex = 0;

        private void AttachContextMenu(Shape shape)
        {
            ContextMenu contextMenu = new ContextMenu();


            MenuItem colorItem = new MenuItem { Header = "Change color" };
            colorItem.Click += (s, args) =>
            {
                Random rnd = new Random();
                Color randomColor = Color.FromRgb((byte)rnd.Next(256), (byte)rnd.Next(256), (byte)rnd.Next(256));
                shape.Fill = new SolidColorBrush(randomColor);
            };


            MenuItem frontItem = new MenuItem { Header = "Bring to front" };
            frontItem.Click += (s, args) => { _topZIndex++; Canvas.SetZIndex(shape, _topZIndex); };

            MenuItem deleteItem = new MenuItem { Header = "Delete" };
            deleteItem.Click += (s, args) =>
            {
                var item = shape.DataContext as WorkspaceItem;
                if (item != null)
                {
                    Items.Remove(item);
                }
                WorkspaceCanvas.Children.Remove(shape);
                if (_selectedShape == shape)
                {
                    _selectedShape = null;
                }
            };

            contextMenu.Items.Add(colorItem);
            contextMenu.Items.Add(frontItem);
            contextMenu.Items.Add(deleteItem);
            shape.ContextMenu = contextMenu;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (_selectedShape == null) return;
            var item = _selectedShape.DataContext as WorkspaceItem;
            if (item == null) return;

            int step = 5;

            switch(e.Key)
            {
                case Key.Up:
                    item.Y -= step;
                    break;
                case Key.Down:
                    item.Y += step;
                    break;
                case Key.Left:
                    item.X -= step;
                    break;
                case Key.Right:
                    item.X += step;
                    break;
                case Key.Delete:
                    Items.Remove(item);
                    WorkspaceCanvas.Children.Remove(_selectedShape);
                    _selectedShape = null;
                    break;
            }
        }
    }
}