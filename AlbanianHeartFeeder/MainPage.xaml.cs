using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace AlbanianHeartFeeder
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.Loaded += OnLoaded;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;


        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            gridView.Visibility = Visibility.Collapsed;
            progressRing.IsActive = true;
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                RetrieveData();
            });

            progressRing.IsActive = false;

            gridView.Visibility = Visibility.Visible;
        }

        private async void RetrieveData()
        {
            var _dataSource = new FeedDataSource();
            await _dataSource.GetFeedsAsync();
            gridView.ItemsSource = FeedDataSource.Data;
        }

        //protected override async void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    progressRing.IsActive = true;
        //    await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //              {
        //                  RetrieveData();
        //              });

        //    progressRing.IsActive = false;
        //}

        private void gridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var clickedItem = e?.ClickedItem as FeedData;

            Frame.Navigate(typeof(ItemsPage), clickedItem);
        }

    }

    // namespace ends here
}

