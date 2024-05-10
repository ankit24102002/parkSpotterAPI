using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using p2p.Common.Models;
using p2p.Logic.Imp;
using System.Data.SqlClient;
using static p2p.Common.Models.Space;
using p2p.Logic.Contract;
using Microsoft.AspNetCore.Identity;
using static p2p.Common.Models.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using p2p.Helper;
using System.Security.Claims;
using static p2p.Common.Models.Admin;

namespace p2p.Controllers
{
//    [MyAuthorizationFilter]
    [Route("[controller]/[action]")]
    [ApiController]
    public class SpaceController : ControllerBase
    {
        private readonly ICustomerManager _customerManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SpaceController> _logger;
        private readonly ParkingPredictionService _predictionService;

        public SpaceController(ICustomerManager customerManager, IConfiguration configuration,
            ILogger<SpaceController> logger, ParkingPredictionService predictionService)
        {
            _customerManager = customerManager;
            _predictionService = predictionService;
            _logger = logger;
            _configuration = configuration;
            _logger.LogDebug("NLog is integrated to space Controller");
        }

     
        [HttpPost()]
        public IActionResult CustomerGetAllSpaces()
        {
            try
            {
                var spaces = _customerManager.GetAllSpaces();
                if (spaces != null && spaces.Count > 0)
                {
                    return Ok(spaces);
                }
                else
                {
                    return NotFound("No spacers found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost()]
        public IActionResult GetByLocation(string Longitude, string Latitude)
        {
            _logger.LogInformation("Start : Getting item details for {ID}");
            try
            {
                var spaces = _customerManager.GetByLocation(Longitude, Latitude);

              

                if (spaces != null && spaces.Count > 0)
                {
                    return Ok(spaces);
                }
                else
                {
                    return NotFound("No spacers found.");
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }
   
        [HttpPost()]
        public IActionResult UpdateSpace(Space_Owner_Master space)
        {
            ResponseData response = _customerManager.UpdateSpace(space);
            try
            {

                if (response.IsSaved)
            {
                response.Message = "Space Updated Successfully";
                return Ok(new { Message = response.Message });
            }
            else
            {
                return BadRequest(new { Error = "Failed to Update Space." });
            }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost()]
        public IActionResult CustomerSpaceDetail(int SpaceID)
        {
            _logger.LogInformation("Start : Getting item details for {ID}");
            try
            {
                var spaces = _customerManager.GetBySpaceID(SpaceID);



                if (spaces != null && spaces.Count > 0)
                {
                    return Ok(spaces);
                }
                else
                {
                    return NotFound("No spacers found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost]
        public IActionResult CustomerDoBooking(Booking_Detail detail)
        {
            ResponseData response = _customerManager.AddBookingDetail(detail);
            try { 
            if (response.IsSaved)
            {
                bool result = true;
                return Ok(new { result = result, Message = response.Message ,});
            }
            else
            {
                bool nresult = false;
                return BadRequest(new { result = nresult, Message = response.Message });
            }
        } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost()]
        public IActionResult CustomerCurPastBooking(ongoning_booking detail)
        {
            try
            {


         var spaces = _customerManager.ongoingcustomerbooking(detail);
                if (spaces != null && spaces.Count > 0)
                {
                    return Ok(spaces);
                }
                else
                {
                    return Ok(spaces);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }


        [HttpPost()]
        public IActionResult GetLocationsWithin2km(Location_user detail)
        {
            try
            {
                var spaces = _customerManager.GetLocationsWithin2km(detail);

                // Get the current date and time
                DateTime currentDateTime = DateTime.Now;
                ParkingDataInput input = new ParkingDataInput
               {          
        Hour = currentDateTime.Hour,
        DayOfWeek = (int)currentDateTime.DayOfWeek ,
        Demand = 5,
                };

                var price = _predictionService.PredictPrice(input.Hour, input.DayOfWeek, input.Demand);

                //    return Ok(price);
             

                if (spaces != null && spaces.Count > 0)
                {
                    return Ok(new { Spaces = spaces, Price = price });
                }
                else
                {
                    return Ok(new { Spaces = spaces, Price = price });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpGet()]
        public ActionResult<DateTime> GetCurrentDateTime()
        {
            try
            {
             // Intentionally cause a DivideByZeroException
                int zero = 0;
                int result = 1 / zero;

                DateTime utcNow = DateTime.UtcNow;
            DateTime datetimenow = DateTime.Now;
            var datetimeResult = new
            {
                UTCDateTime = utcNow,
                LocalDateTime = datetimenow
            };
            return Ok(datetimeResult);

            }
            catch (DivideByZeroException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.ToString());
                return BadRequest("An error occurred due to a division by zero.");
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.ToString());

                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

    }
}
