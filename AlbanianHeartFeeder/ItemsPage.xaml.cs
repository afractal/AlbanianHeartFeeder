using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AlbanianHeartFeeder
{
    public sealed partial class ItemsPage : Page
    {
        private FeedData reuseFeedData;
        private SystemNavigationManager currentView;

        public ItemsPage()
        {
            this.InitializeComponent();
            currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            currentView.BackRequested -= currentView_BackRequested;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            currentView.BackRequested += currentView_BackRequested;

            progressRing.IsActive = true;
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                   {
                       var feedData = e.Parameter as FeedData;
                       reuseFeedData = feedData;
                       this.DataContext = feedData;
                   });

            progressRing.IsActive = false;
            newsItems.Visibility = Visibility.Visible;
        }

        private void currentView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null && frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private void OnShare_Click(object sender, RoutedEventArgs e)
        {
            var manager = DataTransferManager.GetForCurrentView();
            manager.DataRequested += OnDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var deferral = args.Request.GetDeferral();

            if (args != null && newsItems.SelectedItem != null)
            {
                var selectedItem = newsItems?.SelectedItem as FeedDataItem;
                args.Request.Data.Properties.Title = "Sharing news";
                args.Request.Data.Properties.Description = selectedItem?.Title;
                args.Request.Data.SetText(selectedItem?.Title +
                    Environment.NewLine +
                    selectedItem?.Description +
                    Environment.NewLine +
                    selectedItem?.Link +
                    Environment.NewLine +
                    selectedItem?.PubDate.ToString());
                //args.Request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(selectedItem?.ImageLink));
            }
            else
                args.Request.FailWithDisplayText("Please select a news article first!");

            deferral.Complete();
        }

        private async void OnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshData();
        }

        private async Task RefreshData()
        {
            var feedDataSource = new FeedDataSource();
            await feedDataSource.GetFeedsAsync();

            var result = FeedDataSource.GetFeed(reuseFeedData.Title);
            this.newsItems.ItemsSource = result.Items;
            //DataContextChanged += ItemsPage_DataContextChanged;
            //private void ItemsPage_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
            //{
            //    newsItems.UpdateLayout();
            //}
        }

        private void newsItems_Click(object sender, ItemClickEventArgs e)
        {
            var clickedItem = e?.ClickedItem as FeedDataItem;

            Frame.Navigate(typeof(ItemDetailPage), clickedItem);
        }



        // Phase Priorities
        // 1. Simple Shapes(Placeholder visuals)
        // 2. Key text (title)
        // 3. Other text (subtitle)
        // 4. Images
        #region containerContentChanging

        private void IncrementalUpdateHandler(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            args.Handled = true;

            if (args.Phase != 0)
            {
                throw new Exception("Not in phase 0.");
            }

            // First, show the items' placeholders.
            var templateRoot = (StackPanel)args.ItemContainer.ContentTemplateRoot;
            var titleTextBlock = (TextBlock)templateRoot.FindName("textTitle");

            // Make everything else invisible.
            titleTextBlock.Opacity = 0;


            // Show the items' titles in the next phase.
            args.RegisterUpdateCallback(ShowText);
        }

        private void ShowText(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase != 1)
            {
                throw new Exception("Not in phase 1.");
            }

            // Next, show the items' titles. Keep everything else invisible.
            var myItem = (FeedDataItem)args.Item;
            SelectorItem itemContainer = (SelectorItem)args.ItemContainer;
            var templateRoot = (StackPanel)itemContainer.ContentTemplateRoot;
            var titleTextBlock = (TextBlock)templateRoot.FindName("textTitle");

            titleTextBlock.Text = myItem.Title;
            titleTextBlock.Opacity = 1;

            // Show the items' subtitles in the next phase.
            args.RegisterUpdateCallback(ShowDescription);
        }

        private void ShowDescription(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase != 2)
            {
                throw new Exception("Not in phase 2.");
            }

            // Next, show the items' subtitles. Keep everything else invisible.
            var myItem = (FeedDataItem)args.Item;
            SelectorItem itemContainer = (SelectorItem)args.ItemContainer;

            var templateRoot = (StackPanel)itemContainer.ContentTemplateRoot;
            var descriptionTextBlock = (TextBlock)templateRoot.FindName("textDescription");

            descriptionTextBlock.Text = myItem.Description;
            descriptionTextBlock.Opacity = 1;

            descriptionTextBlock.Text = myItem.Description;
            descriptionTextBlock.Opacity = 1;

            // Show the items' descriptions in the next phase.
            args.RegisterUpdateCallback(ShowImage);
        }

        private void ShowImage(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (args.Phase != 3)
            {
                throw new Exception("Not in phase 3.");
            }

            // Finally, show the items' descriptions. 
            var myItem = (FeedDataItem)args.Item;
            SelectorItem itemContainer = (SelectorItem)args.ItemContainer;

            var templateRoot = (StackPanel)itemContainer.ContentTemplateRoot;
            var picture = (Image)templateRoot.FindName("picture");

            var bitImage = new BitmapImage();
            bitImage.UriSource = myItem.ImageLink;

            picture.Source = bitImage;
            picture.Opacity = 1;
        }

        #endregion


    }

    // namespace ends here
}