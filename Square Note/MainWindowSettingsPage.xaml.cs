using Microsoft.UI.Xaml.Controls;
using Square_Note.Objects;
using Square_Note.Providers;
using Square_Note.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using WinRT.Interop;

namespace Square_Note
{
    public sealed partial class MainWindowSettingsPage : Page
    {
        public MainWindowSettingsPage()
        {
            InitializeComponent();
        }

        private async void ExportAllDataButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Le choix du fichier
            var savePicker = new Windows.Storage.Pickers.FileSavePicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "squarenote.xml",
            };
            savePicker.FileTypeChoices.Add("Fichier de données Square Note", [".xml"]);
            nint windowHandle = WindowNative.GetWindowHandle(App.TheMainWindow);
            InitializeWithWindow.Initialize(savePicker, windowHandle);
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file == null) return;

            // Convertir les notes en fichier de données
            List<QuickNote> notes = QuickNoteProvider.GetQuickNotes();
            SquareNoteDataFile dataFile = new()
            {
                QuickNotes = [.. notes]
            };

            // Mettre en XML
            CachedFileManager.DeferUpdates(file);
            XmlSerializer x = new(typeof(SquareNoteDataFile));
            StringWriter textWriter = new();
            x.Serialize(textWriter, dataFile);
            await FileIO.WriteTextAsync(file, textWriter.ToString());

            Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
            if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
            {
                ExportAllDataButton.Content = "Fichier " + file.Name + " enregistré.";
            }
            else
            {
                ExportAllDataButton.Content = "Fichier " + file.Name + " enregistré.";
            }
        }

        private async void ImportAllDataButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            // Le choix du fichier
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add(".xml");
            nint windowHandle = WindowNative.GetWindowHandle(App.TheMainWindow);
            InitializeWithWindow.Initialize(openPicker, windowHandle);
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file == null) return;

            // Ouvrir le fichier
            FileStream stream = File.OpenRead(file.Path);

            XmlSerializer serializer = new(typeof(SquareNoteDataFile));
            object? deserializedXml = serializer.Deserialize(stream);
            if (deserializedXml is not null)
            {
                var dataFile = (SquareNoteDataFile)deserializedXml;
                for (int i = 0; i < dataFile.QuickNotes?.Length; i++)
                {
                    QuickNoteProvider.SaveQuickNote(dataFile.QuickNotes[i]);
                }
            }
            stream.Close();
        }
    }
}
