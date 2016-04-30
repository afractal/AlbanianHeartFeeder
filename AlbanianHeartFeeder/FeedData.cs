using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbanianHeartFeeder
{
    public class FeedData
    {
        public string Title { get; set; }
        public Uri Link { get; set; }

        private List<FeedDataItem> _Items;
        public List<FeedDataItem> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }

        public FeedData()
        {
            _Items = new List<FeedDataItem>();
        }

    }

    // namespace ends here
}