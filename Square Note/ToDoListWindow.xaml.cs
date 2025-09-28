using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Square_Note.Objects;
using Square_Note.Providers;
using Windows.Graphics;

namespace Square_Note
{
    /// <summary>
    /// Fenêtre contenant une to-do list
    /// </summary>
    public sealed partial class ToDoListWindow : Window
    {
        readonly ToDoList CurrentList;

        public ToDoListWindow(ToDoList list)
        {
            InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(WindowTitleBar);

            OverlappedPresenter? p = AppWindow.Presenter as OverlappedPresenter;
            p!.IsMaximizable = false;
            AppWindow.Resize(new SizeInt32(350, 500));
            Title = list.Title;

            CurrentList = list;
            MyItemsRepeater.ItemsSource = list.Items;
            TitleTextBlock.Text = list.Title;
            EmptyListInfoBar.IsOpen = list.Items.Length == 0;
        }

        private void EditTitleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentList.PrependItem(new(NewItemTextBox.Text));
            MyItemsRepeater.ItemsSource = CurrentList.Items;
            ToDoListProvider.SaveToDoList(CurrentList);
        }

        private void ItemCheckbox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            int index = (int)c.Tag;
            CurrentList.Items[index].Checked = c.IsChecked ?? false;
            ToDoListProvider.SaveToDoList(CurrentList);
        }
    }
}
