using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using p2p.Logic.Contract;
using p2p.Logic.Imp;
using static p2p.Common.Models.Review;
using static p2p.Common.Models.Space;

namespace p2p.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewManager _reviewManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewManager reviewManager, IConfiguration configuration,
            ILogger<ReviewController> logger)
        {
            _reviewManager = reviewManager;
            _configuration = configuration;
            _logger = logger;
            _logger.LogDebug("NLog is integrated to Review Controller");
        }


        [HttpPost()]
        public IActionResult Addrating([FromBody] Rating value)
        {

            ResponseData response = _reviewManager.Addrating(value);
            try
            {
                if (response.IsSaved)
                {
                    bool result = true;
                    string message = response.Message;
                    return Ok(new { result = result, message = message });
                }
                else
                {
                    bool nresult = false;
                    string message = response.Message;
                    return Ok(new { result = nresult, message = message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

    }
}
