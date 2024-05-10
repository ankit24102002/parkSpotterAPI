using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using p2p.Logic.Contract;
using p2p.Logic.Imp;
using static p2p.Common.Models.Space;
using static p2p.Common.Models.Users;

namespace p2p.Controllers
{
   // [MyAuthorizationFilter]
    [Route("[controller]/[action]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerManager _ownerManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OwnerController> _logger;

        public OwnerController(IOwnerManager ownerManager, IConfiguration configuration,
            ILogger<OwnerController> logger)
        {
            _ownerManager = ownerManager;
            _logger = logger;
            _configuration = configuration;
            _logger.LogDebug("NLog is integrated to space Controller");
        }



        [HttpPost()]
        public IActionResult OwnerDeletespace(int spaceid)
        {

            ResponseData response = _ownerManager.DeleteSpace(spaceid);
            try { 
            if (response.IsSaved)
            {
                    bool result = true;
                    return Ok(new { result = result, Message = response.Message });
            }
            else
                {
                    response.Message = "Currently Booked";
                    bool result = false;
                    return Ok(new { result = result, Message = response.Message });
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }



        [HttpPost()]
        public IActionResult OwnerCurrentPastbooking(ongoning_booking detail)
        {
            try
            {
                var spaces = _ownerManager.GetCurrentbooking(detail);
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
        public IActionResult OwnerCurrentBookingDetail(int SpaceID)
        {
            _logger.LogInformation("Start : Getting item details for {ID}");
            try
            {
                var spaces = _ownerManager.GetBySpaceIDowner(SpaceID);



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
        public IActionResult OwnerAllSpace(string username)
        {
            _logger.LogInformation("Start : Getting item details for {ID}");
            try
            {
                var spaces = _ownerManager.GetAllSpacesbyusername(username);



                if (spaces != null && spaces.Count > 0)
                {
                    return Ok(spaces);
                }
                else
                {
                    return NotFound("No space found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost()]
        public IActionResult OwnerEnablespace(int SpaceID)
        {

            ResponseData response = _ownerManager.Enablespace(SpaceID);
            try { 
            if (response.IsSaved)
            {
                response.Message = "Space enabled Successfully";
                bool result = true;
                return Ok(new { result = result, Message = response.Message });
            }
            else
            {
                    response.Message = "Currently Booked";
                    bool result = false;
                    return Ok(new { result = result, Message = response.Message });
                }
        } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost()]
        public IActionResult OwnerDisablespace(int SpaceID)
        {

            ResponseData response = _ownerManager.Disablespace(SpaceID);
            try { 
            if (response.IsSaved)
            {
                response.Message = "Space disabled Successfully";
                bool result = true;
                return Ok(new { result = result, Message = response.Message });
            }
            else
            {
                    response.Message = "Currently Booked";
                    bool result = false;
                    return Ok(new { result = result, Message = response.Message });
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost()]
        public IActionResult OwnerAddspace([FromBody] addspace space)
        {

            ResponseData response = _ownerManager.Addspace(space);
            try { 
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


        [HttpPost()]
        public IActionResult OwnerGetSpaceDetail(int spaceid)
        {
            _logger.LogInformation("Start : Getting item details for {ID}");
            try
            {
                var spaces = _ownerManager.OwnerGetSpaceDetail(spaceid);



                if (spaces != null && spaces.Count > 0)
                {
                    string message = "Success!";
                    bool result = true;

                    return Ok(new { result = result, message = message, detail = spaces });
                }
                else
                {
                    bool nresult = false;
                    return Ok(new { result = nresult, message = "Failed", detail = spaces });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }
        [HttpPost()]
        public IActionResult UpdateSpace(update_space spacedata)
        {

            ResponseData response = _ownerManager.UpdateSpace(spacedata);
            try
            {
                if (response.IsSaved)
                {
                    response.Message = "Profile updated";
                    bool result = true;
                    return Ok(new { result = result, Message = response.Message });
                }
                else
                {
                    bool result = false;
                    return Ok(new { result = result, Message = response.Message });
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
