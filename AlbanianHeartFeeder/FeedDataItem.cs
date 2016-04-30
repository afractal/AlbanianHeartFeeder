﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbanianHeartFeeder
{
    public class FeedDataItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Link { get; set; }
        public DateTime PubDate { get; set; }
        public Uri ImageLink { get; set; }
        public string FullDescription { get; set; }
    }

    // namespace ends here
}