using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Square_Note.Objects;
using Square_Note.Providers;
using System;
using Windows.Graphics;

namespace Square_Note
{
    /// <summary>
    /// Fenêtre contenant une to-do list
    /// </summary>
    public sealed partial class ToDoListWindow : Window
    {
        public ToDoList CurrentList { get; private set; }

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

        private async void EditTitleButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new();
            ToDoListWindowEditTitleDialog dialogContent = new(CurrentList.Title);

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = Content.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Changer le titre";
            dialog.PrimaryButtonText = "Enregistrer";
            dialog.IsSecondaryButtonEnabled = false;
            dialog.CloseButtonText = "Annuler";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = dialogContent;

            ContentDialogResult result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                CurrentList.Title = dialogContent.InputText;
                TitleTextBlock.Text = CurrentList.Title;
                Title = CurrentList.Title;
                ToDoListProvider.SaveToDoList(CurrentList);
            }
        }

        private void DisplayMainWindowButton_Click(object sender, RoutedEventArgs e)
        {
            App.ShowMainWindow();
        }

        private void NewItemTextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CurrentList.PrependItem(new(NewItemTextBox.Text));
                MyItemsRepeater.ItemsSource = CurrentList.Items;
                ToDoListProvider.SaveToDoList(CurrentList);
            }
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
