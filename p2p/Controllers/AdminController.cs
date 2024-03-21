using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using p2p.Logic.Contract;
using p2p.Logic.Imp;
using static p2p.Common.Models.Admin;
using static p2p.Common.Models.Space;
using static p2p.Common.Models.Users;

namespace p2p.Controllers
{
    [MyAuthorizationFilter]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager _adminManager;
        private readonly ILogger<SpaceController> _logger;

        public AdminController(IAdminManager adminManager,
            ILogger<SpaceController> logger)
        {
            _adminManager = adminManager;
            _logger = logger;
        }

        [HttpPost()]
        public IActionResult AllUserList(input_1 detail)
        {
            try
            {
                var users = _adminManager.AllUserList(detail);
                if (users != null && users.Count > 0)
                {
                    return Ok(users);
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
        public IActionResult EnableDisable(ongoning_booking detail)
        {

            ResponseData response = _adminManager.EnableDisable(detail);
            try { 
            if (response.IsSaved)
            {
                return Ok(new { Message = response.Message });
            }
            else
            {
                return BadRequest(new { Error = "Failed to update state." });
            }
        }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }

        }


        [HttpPost()]
        public IActionResult ContactUs( contactus info)
        {
            try
            { 
                ResponseData response = _adminManager.ContactUs(info);

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
