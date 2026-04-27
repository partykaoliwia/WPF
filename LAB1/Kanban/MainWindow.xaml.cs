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

namespace Kanban
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TaskItem> ToDoTasks { get; set; } = new ObservableCollection<TaskItem>();
        public ObservableCollection<TaskItem> DoneTasks { get; set; } = new ObservableCollection<TaskItem>();
        public ObservableCollection<TaskItem> InProgressTasks { get; set; } = new ObservableCollection<TaskItem>();
        private TaskItem _editedTask = null;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void LeftButton(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TaskItem;
            if (task == null) return;
            if (InProgressTasks.Contains(task))
            {
                InProgressTasks.Remove(task);
                ToDoTasks.Add(task);
            }
            else if (DoneTasks.Contains(task))
            {
                DoneTasks.Remove(task);
                InProgressTasks.Add(task);
            }
        }


        private void RightButton(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TaskItem;
            if (task == null) return;
            if (InProgressTasks.Contains(task))
            {
                InProgressTasks.Remove(task);
                DoneTasks.Add(task);
            }
            else if (ToDoTasks.Contains(task))
            {
                ToDoTasks.Remove(task);
                InProgressTasks.Add(task);
            }

        }

        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TaskItem;
            if (task == null) return;
            if (InProgressTasks.Contains(task))
            {
                InProgressTasks.Remove(task);
            }
            else if (DoneTasks.Contains(task))
            {
                DoneTasks.Remove(task);
            }
            else if(ToDoTasks.Contains(task))
            {
                ToDoTasks.Remove(task);
            }

        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            if (_editedTask != null)
            {
                _editedTask.Content = TaskTextBox.Text;
                _editedTask = null;
                ActionHeader.Content = "Add Task";
            }
            else
            {
                ToDoTasks.Add(new TaskItem { Content = TaskTextBox.Text });
            }
            TaskTextBox.Text = string.Empty;
        }

        private void EditButton(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var task = button?.DataContext as TaskItem;
            if (task == null) return;
            _editedTask = task;
            TaskTextBox.Text = task.Content;
            ActionHeader.Content = "Edit Task";

        }
        private void CancelButton(object sender, RoutedEventArgs e)
        {
            _editedTask = null;
            TaskTextBox.Text= string.Empty;
            ActionHeader.Content = "Add Task";

        }
    }
}