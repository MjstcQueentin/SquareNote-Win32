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
using Square_Note.Providers;
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

            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SquareNote\\ToDoLists"))
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\SquareNote\\ToDoLists");
            }

            ShowMainWindow();
        }

        public static MainWindow? TheMainWindow { get; private set; }

        public static void ShowMainWindow()
        {
            if (TheMainWindow is null)
            {
                TheMainWindow = new MainWindow();
                TheMainWindow.Closed += OnMainWindowClosed;
            }

            TheMainWindow.Activate();
        }

        private static void OnMainWindowClosed(object sender, WindowEventArgs args)
        {
            TheMainWindow = null;
        }

        public static LinkedList<ToDoListWindow>? ToDoListWindows { get; private set; }

        /// <summary>
        /// Afficher une to-do list dans une fenêtre dédiée.
        /// Si une fenêtre de ladite list existe encore, elle est placée au premier plan.
        /// Sinon la fenêtre est crée.
        /// </summary>
        /// <param name="ToDoListID">ID de la liste</param>
        public static void ShowToDoListWindow(int ToDoListID)
        {
            // Chercher la fenêtre existante
            ToDoListWindows ??= new LinkedList<ToDoListWindow>();
            ToDoListWindow? existingWindow = ToDoListWindows.FirstOrDefault(w => w.CurrentList.ID == ToDoListID);

            if (existingWindow is not null)
            {
                // Elle existe déjà, on la place au premier plan
                existingWindow.Activate();
                return;
            }
            else
            {
                // Elle n'existe pas, on l'instancie
                ToDoListWindow window = new(ToDoListProvider.GetToDoList(ToDoListID));
                ToDoListWindows.AddLast(window);

                // La retirer de la liste quand elle est fermée
                window.Closed += (s, e) =>
                {
                    ToDoListWindows?.Remove(window);
                };

                // Placer au premier plan
                window.Activate();
            }
        }
    }
}
