using Microsoft.UI.Xaml.Controls;
using Square_Note.Services;
using System.Diagnostics;
using System.Reflection;
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
                    Assembly.GetExecutingAssembly().GetName().Version?.Major,
                    Assembly.GetExecutingAssembly().GetName().Version?.Minor,
                    Assembly.GetExecutingAssembly().GetName().Version?.Build,
                    Assembly.GetExecutingAssembly().GetName().Version?.Revision);
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
