using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Square_Note.Objects;
using Square_Note.Providers;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics;
using DebounceThrottle;
using Microsoft.UI.Xaml.Controls;

namespace Square_Note
{
    public sealed partial class QuickNoteWindow : Window
    {
        private QuickNote note;
        private readonly DebounceDispatcher SaveDispatcher = new(
            interval: TimeSpan.FromSeconds(1),
            maxDelay: TimeSpan.FromSeconds(3)
        );

        public QuickNoteWindow()
        {
            InitializeComponent();
            note = QuickNoteProvider.CreateNewQuickNote();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(WindowTitleBar);

            OverlappedPresenter? p = AppWindow.Presenter as OverlappedPresenter;
            p!.IsMaximizable = false;
            p!.IsMinimizable = false;
            p!.IsResizable = false;

            AppWindow.Resize(new SizeInt32(350, 350));

            Closed += QuickNoteWindow_Closed;
        }

        private void QuickNoteWindow_Closed(object sender, WindowEventArgs args)
        {
            SaveDispatcher.FlushAndDispose();
        }

        public void SetNote(int noteID)
        {
            note = QuickNoteProvider.GetQuickNote(noteID);
        }

        private void BodyTextBox_TextChanged(object sender, Microsoft.UI.Xaml.Controls.TextChangedEventArgs e)
        {
            note.Body = BodyTextBox.Text;
            note.UpdateTime = DateTime.Now;
            SaveDispatcher.Debounce(() =>
            {
                QuickNoteProvider.SaveQuickNote(note);
            });
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManagerInterop.GetForWindow((IntPtr)AppWindow.Id.Value).DataRequested += (sender, args) =>
            {
                args.Request.Data.Properties.Title = "Partager le pense-bête";
                args.Request.Data.SetText(BodyTextBox.Text);
            };

            DataTransferManagerInterop.ShowShareUIForWindow((IntPtr)AppWindow.Id.Value);
        }

        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            OverlappedPresenter? p = AppWindow.Presenter as OverlappedPresenter;
            p!.IsAlwaysOnTop = PinButton.IsChecked == true;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            QuickNoteProvider.DeleteQuickNote(note.ID);
            Close();
        }

        private void NoteListButton_Click(object sender, RoutedEventArgs e)
        {
            App? a = Application.Current as App;
            a?.ShowMainWindow();
        }

        private void ColorSwitchMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem i = (MenuFlyoutItem)sender;
            string? tagStr = i.Tag?.ToString();
            if (tagStr is null) return;
            int newColourIdx = int.Parse(tagStr);

            // Exclure un index qui n'existe pas
            if(newColourIdx < 0 || newColourIdx > QuickNote.Colours.Length - 1) return;

            // Appliquer la couleur
            note.BackgroundColour = QuickNote.Colours[newColourIdx];

            // Enregistrer
            SaveDispatcher.Debounce(() =>
            {
                QuickNoteProvider.SaveQuickNote(note);
            });

            DispatcherQueue.TryEnqueue(() =>
            {
                Bindings.Update();
            });
        }
    }
}
