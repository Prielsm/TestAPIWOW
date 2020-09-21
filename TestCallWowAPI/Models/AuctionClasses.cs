using System;
using System.Collections.Generic;
using System.Text;

namespace TestCallWowAPI.Models
{
    public class AuctionApiResponse
    {
        public List<AuctionFile> files { get; set; }
    }

    public class AuctionFile
    {
        public string url { get; set; }
        public long lastModified { get; set; }
    }

    public class AuctionFileContents
    {
        public List<Auction> auctions { get; set; }
    }

    public class Auction
    {
        public int item { get; set; } // This is the item's ID
        public string owner { get; set; } // This is the Seller Name
        public long bid { get; set; } // This is the bid price in copper
        public long buyout { get; set; } // This is the buyout price in copper. 1000g is 10000000
        public int quantity { get; set; } // This is the amount of this item
        public long PricePerItem => buyout / quantity; // This is helpful
    }
}
