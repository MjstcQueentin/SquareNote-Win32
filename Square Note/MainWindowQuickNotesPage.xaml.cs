using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Square_Note.Providers;
using System;
using Microsoft.UI.Dispatching;
using Square_Note.Objects;
using System.Collections.Generic;

namespace Square_Note
{
    /// <summary>
    /// Représente l'onglet "Notes rapides" de la fenêtre principale
    /// </summary>
    public sealed partial class MainWindowQuickNotesPage : Page
    {
        private int? CurrentContext;

        public MainWindowQuickNotesPage()
        {
            InitializeComponent();

            LoadQuickNotes();
            QuickNoteProvider.QuickNotesModified += QuickNoteProvider_QuickNotesModified;
        }

        private void LoadQuickNotes()
        {
            List<QuickNote> list = QuickNoteProvider.GetQuickNotes();
            MyItemsRepeater.ItemsSource = list;
            EmptyInfoBar.Visibility = list.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void QuickNoteProvider_QuickNotesModified(object? sender, EventArgs e)
        {
            DispatcherQueue.TryEnqueue(() =>
            {
                LoadQuickNotes();
            });
        }

        private void Grid_Tapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Grid castedSender = (Grid)sender;
            string? gridTag = castedSender.Tag.ToString();
            if (gridTag is not null)
            {
                int noteID = int.Parse(gridTag);
                QuickNoteWindow w = new();
                w.SetNote(noteID);
                w.Activate();
            }
        }

        private void Grid_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            Grid castedSender = (Grid)sender;
            string? gridTag = castedSender.Tag.ToString();
            if (gridTag is not null)
            {
                CurrentContext = int.Parse(gridTag);
            }
        }

        private void NewQuickNoteButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            QuickNoteWindow w = new();
            w.Activate();
        }

        private void DeleteQuickNoteButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (CurrentContext is not null) { 
                QuickNoteProvider.DeleteQuickNote((int)CurrentContext);
            }
        }
    }
}
