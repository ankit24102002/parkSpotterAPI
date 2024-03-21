using p2p.DataAdaptor.Contract;
using p2p.DataAdaptor.Imp;
using p2p.Logic.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Review;
using static p2p.Common.Models.Space;

namespace p2p.Logic.Imp
{
    public class ReviewManager : IReviewManager
    {
        protected readonly IReviewRepository ReviewRepository;
        public ReviewManager(IReviewRepository reviewRepository)
        {
            ReviewRepository = reviewRepository;
        }

        public ResponseData Addrating(Rating value)
        {
            return ReviewRepository.Addrating(value);
        }
    }
}
