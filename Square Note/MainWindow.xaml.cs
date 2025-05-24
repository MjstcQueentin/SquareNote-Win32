using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Square_Note.Services;

namespace Square_Note
{
    public sealed partial class MainWindow : Window
    {
        private double NavViewCompactModeThresholdWidth { get { return NavView.CompactModeThresholdWidth; } }

        public MainWindow()
        {
            this.InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            MajesticielsUpdater.Instance.UpdateAvailable += MajesticielsUpdater_UpdateAvailable;
            MajesticielsUpdater.Instance.CheckUpdatesNow();
        }

        private void MajesticielsUpdater_UpdateAvailable(object sender, MajesticielsUpdate e)
        {
            if (e.UpdateAvailable)
            {
                UpdateAvailableInfoBadge.Visibility = Visibility.Visible;
            }
        }

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            NavView.SelectedItem = NavView.MenuItems[0];
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected == true)
            {
                NavView_Navigate(typeof(MainWindowSettingsPage), args.RecommendedNavigationTransitionInfo);
            }
            else
            {
                switch (args.SelectedItemContainer.Tag.ToString())
                {
                    case "Square_Note.MainWindowQuickNotesPage":
                    default:
                        NavView_Navigate(typeof(MainWindowQuickNotesPage), args.RecommendedNavigationTransitionInfo);
                        break;
                    case "Square_Note.MainWindowHelpPage":
                        NavView_Navigate(typeof(MainWindowHelpPage), args.RecommendedNavigationTransitionInfo);
                        break;
                }
            }
        }

        private void NavView_Navigate(Type navPageType, NavigationTransitionInfo transitionInfo)
        {
            Type preNavPageType = ContentFrame.CurrentSourcePageType;

            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                ContentFrame.Navigate(navPageType, null, transitionInfo);
            }
        }

        private void ContentFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(MainWindowSettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag.
                NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
                NavView.Header = "Paramètres";
            }
            else if (ContentFrame.SourcePageType != null)
            {
                string? SourcePageTypeName = ContentFrame.SourcePageType.FullName;
                NavigationViewItem? selectedItem;

                try
                {
                    selectedItem = NavView.MenuItems.OfType<NavigationViewItem>().First(i => i.Tag.Equals(SourcePageTypeName));
                }
                catch
                {
                    selectedItem = NavView.FooterMenuItems.OfType<NavigationViewItem>().First(i => i.Tag.Equals(SourcePageTypeName));
                }

                NavView.Header = selectedItem.Content?.ToString();
            }
        }
    }
}
