using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2p.Common.Models
{
    public class Review
    {

        public class Rating
        {
            public string Username { get; set; }
            public int SpaceID { get; set; }
            public int rating { get; set; }
            public int ratingid { get; set; }
            public int bookingID { get; set; }

        }

    }
}
