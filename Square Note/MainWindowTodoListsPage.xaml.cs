using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Square_Note.Objects;
using Square_Note.Providers;
using System;
using System.Collections.Generic;

namespace Square_Note
{
    /// <summary>
    /// Représente l'onglet Listes de la fenêtre principale.
    /// </summary>
    public sealed partial class MainWindowTodoListsPage : Page
    {
        public MainWindowTodoListsPage()
        {
            InitializeComponent();

            LoadTodoLists();
            ToDoListProvider.ToDoListsModified += ToDoListProvider_ToDoListsModified;
        }

        private void ToDoListProvider_ToDoListsModified(object? sender, EventArgs e)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                LoadTodoLists();
            });
        }

        private void LoadTodoLists()
        {
            List<ToDoList> list = ToDoListProvider.GetToDoLists();
            MyItemsRepeater.ItemsSource = list;
            EmptyInfoBar.Visibility = list.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void NewListButton_Click(object sender, RoutedEventArgs e)
        {
            ToDoList list = ToDoListProvider.CreateNewToDoList();
            ToDoListProvider.SaveToDoList(list);
        }

        private void DeleteListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToDoListButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int id = (int)btn.Tag;
            App.ShowToDoListWindow(id);
        }
    }
}
