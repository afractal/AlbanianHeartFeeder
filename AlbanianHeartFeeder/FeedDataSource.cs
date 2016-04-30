using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

//using FDI = ZemraShqiptare.Model.FeedDataItem;


namespace AlbanianHeartFeeder
{
    public class FeedDataSource
    {
        // TODO: change it to ObservableCollection<T>
        private static List<FeedData> _Data;
        public static List<FeedData> Data
        {
            get { return _Data; }
            set { _Data = value; }
        }

        public FeedDataSource()
        {
            _Data = new List<FeedData>();
        }

        public async Task GetFeedsAsync()
        {
            Task<FeedData> feed = GetFeedAsync("http://www.gazetaexpress.com/rss/ballina/?xml=1");
            Task<FeedData> feed1 = GetFeedAsync("http://www.gazetaexpress.com/rss/lajme/?xml=1");
            Task<FeedData> feed2 = GetFeedAsync("http://www.gazetaexpress.com/rss/sport/?xml=1");
            Task<FeedData> feed3 = GetFeedAsync("http://www.gazetaexpress.com/rss/roze/?xml=1");
            Task<FeedData> feed4 = GetFeedAsync("http://www.gazetaexpress.com/rss/shneta/?xml=1");
            Task<FeedData> feed5 = GetFeedAsync("http://www.gazetaexpress.com/rss/oped/?xml=1");
            Task<FeedData> feed6 = GetFeedAsync("http://www.gazetaexpress.com/rss/arte/?xml=1");
            Task<FeedData> feed7 = GetFeedAsync("http://www.gazetaexpress.com/rss/fun/?xml=1");
            Task<FeedData> feed8 = GetFeedAsync("http://www.gazetaexpress.com/rss/mistere/?xml=1");
            Task<FeedData> feed9 = GetFeedAsync("http://www.gazetaexpress.com/rss/tech/?xml=1");

            Data.Add(await feed);
            Data.Add(await feed1);
            Data.Add(await feed2);
            Data.Add(await feed3);
            Data.Add(await feed4);
            Data.Add(await feed5);
            Data.Add(await feed6);
            Data.Add(await feed7);
            Data.Add(await feed8);
            Data.Add(await feed9);
        }

        private async Task<FeedData> GetFeedAsync(string uriString)
        {
            Uri feedUri = new Uri(uriString);
            SyndicationClient client = new SyndicationClient();
            FeedData feedData = null;
            try
            {
                var feed = await client.RetrieveFeedAsync(feedUri);

                feedData = new FeedData();

                feedData.Title = feed?.Title?.Text;
                feedData.Link = feed?.Links[0]?.Uri;

                foreach (var item in feed.Items)
                {
                    var feedItem = new FeedDataItem();

                    feedItem.Title = item?.Title?.Text;
                    feedItem.Description = item?.Summary?.Text;
                    feedItem.Link = item?.Links[0]?.Uri;
                    feedItem.PubDate = item.PublishedDate.DateTime;
                    feedItem.ImageLink = item?.Links[1]?.Uri;

                    feedData.Items.Add(feedItem);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ldkeofjersngioregiorejgloergirejngnre" + ex.Message);
            }

            return feedData;
        }

        


        // return the feed that has the specified title
        public static FeedData GetFeed(string title)
        {
            var matches = Data.Where(p => p.Title == title).FirstOrDefault();

            return matches;
        }

        public static FeedDataItem GetFeedItem(string uniqueId)
        {
            var matches = Data.SelectMany(s => s.Items).Where(p => p.Title == uniqueId).FirstOrDefault();

            return matches;
        }

    }

    // namespace ends here
}