using Microsoft.UI.Xaml.Controls;
using Square_Note.Services;
using System.Diagnostics;
using System.Reflection;

namespace Square_Note
{
    public sealed partial class MainWindowHelpPage : Page
    {
        public MainWindowHelpPage()
        {
            InitializeComponent();
            MajesticielsUpdater.Instance.UpdateAvailable += MajesticielsUpdater_UpdateAvailable;
            MajesticielsUpdater.Instance.CheckUpdatesNow();

            VersionNumberTextBlock.Text = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
        }

        private void MajesticielsUpdater_UpdateAvailable(object sender, MajesticielsUpdate e)
        {
            if (e.UpdateAvailable)
            {
                UpdateAvailableInfoBar.IsOpen = true;
            }
        }

        private void UpdateAvailableInfoBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = MajesticielsUpdater.Instance.UpdateLocation,
                UseShellExecute = true,
                Arguments = MajesticielsUpdater.Instance.UpdateData?.NewVersion?.UpdateArgs
            });
        }
    }
}
