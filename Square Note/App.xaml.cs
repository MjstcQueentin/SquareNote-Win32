using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Square_Note.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Square_Note
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SquareNote"))
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SquareNote");
            }

            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SquareNote\\QuickNotes"))
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SquareNote\\QuickNotes");
            }

            ShowMainWindow();
        }

        public static MainWindow? TheMainWindow { get; private set; }

        public void ShowMainWindow()
        {
            if (TheMainWindow is null)
            {
                TheMainWindow = new MainWindow();
                TheMainWindow.Closed += OnMainWindowClosed;
            }

            TheMainWindow.Activate();
        }

        private void OnMainWindowClosed(object sender, WindowEventArgs args)
        {
            TheMainWindow = null;
        }
    }
}
