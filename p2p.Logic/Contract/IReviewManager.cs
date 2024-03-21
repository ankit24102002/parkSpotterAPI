using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Review;
using static p2p.Common.Models.Space;

namespace p2p.Logic.Contract
{
    public interface  IReviewManager
    {
        ResponseData Addrating(Rating value);
    }
}
