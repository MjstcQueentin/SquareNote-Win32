using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Square_Note.Objects;
using Windows.Graphics;

namespace Square_Note
{
    /// <summary>
    /// Fenêtre contenant une to-do list
    /// </summary>
    public sealed partial class ToDoListWindow : Window
    {
        public ToDoListWindow(ToDoList list)
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(WindowTitleBar);

            OverlappedPresenter? p = AppWindow.Presenter as OverlappedPresenter;
            p!.IsMaximizable = false;
            AppWindow.Resize(new SizeInt32(350, 500));

            MyItemsRepeater.ItemsSource = list.Items;
            TitleTextBlock.Text = list.Title;
        }

        private void EditTitleButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
