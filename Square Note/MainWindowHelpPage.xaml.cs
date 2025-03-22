using Microsoft.UI.Xaml.Controls;
using Square_Note.Services;
using System.Diagnostics;
using Windows.ApplicationModel;

namespace Square_Note
{
    public sealed partial class MainWindowHelpPage : Page
    {
        public MainWindowHelpPage()
        {
            InitializeComponent();
            MajesticielsUpdater.Instance.UpdateAvailable += MajesticielsUpdater_UpdateAvailable;
            MajesticielsUpdater.Instance.CheckUpdatesNow();

            VersionNumberTextBlock.Text = string.Format("Version {0}.{1}.{2}.{3}",
                    Package.Current.Id.Version.Major,
                    Package.Current.Id.Version.Minor,
                    Package.Current.Id.Version.Build,
                    Package.Current.Id.Version.Revision);
        }

        private void MajesticielsUpdater_UpdateAvailable(object sender, MajesticielsUpdate e)
        {
            UpdateAvailableInfoBar.IsOpen = true;
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
