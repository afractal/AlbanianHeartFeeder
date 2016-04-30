using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AlbanianHeartFeeder
{
    public sealed partial class ItemDetailPage : Page
    {
        private SystemNavigationManager currentView;

        public ItemDetailPage()
        {
            this.InitializeComponent();
            currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            //this.SizeChanged += Size_Changed;


        }

        //private void Size_Changed(object sender, SizeChangedEventArgs e)
        //{
        //    var state = "VisualState500min";

        //    if (e.NewSize.Width > 800)
        //    {
        //        state = "VisualState800min";
        //    }
        //    else if (e.NewSize.Width > 1000)
        //    {
        //        state = "VisualState1000min";
        //    }

        //    VisualStateManager.GoToState(this, state, true);
        //}


        private void currentView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null && frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            currentView.BackRequested -= currentView_BackRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            currentView.BackRequested += currentView_BackRequested;

            var feedDataItem = e?.Parameter as FeedDataItem;
            this.detailGrid.DataContext = feedDataItem;
            Parsing(feedDataItem.Link.ToString());
        }



        // TODO: do some performace inhancment with the StringBuilder
        private async void Parsing(string website)
        {
            try
            {
                HttpClient http = new HttpClient();
                var response = await http.GetByteArrayAsync(website);
                string source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
                source = WebUtility.HtmlDecode(source);

                string str = "";

                var document = new HtmlDocument();
                document.LoadHtml(source);

                var div = document.GetElementbyId("main-content");
                var paragraphs = div.ChildNodes.Where(item => item.Name == "p");

                foreach (var item in paragraphs)
                {
                    str += item.InnerText;
                }

                fullDescription.Text = str;
            }
            catch (Exception ex)
            {
                await new MessageDialog("Could not load the file, please check your internet connection"
                                            + ex.Message).ShowAsync();
            }
        }


    }

// namespace ends hree
}
