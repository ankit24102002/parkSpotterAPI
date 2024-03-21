using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace p2p.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoCalculatorController : ControllerBase
    {
     private const double EarthRadiusKm = 6371.0;

            [HttpGet]
            public ActionResult<double> Get(double lat1, double lon1, double lat2, double lon2)
            {
                double distance = CalculateDistance(lat1, lon1, lat2, lon2);
                return Ok(distance);
            }

            private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
            {
                var dLat = DegreeToRadian(lat2 - lat1);
                var dLon = DegreeToRadian(lon2 - lon1);

                var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2)) *
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

                var distance = EarthRadiusKm * c;
                return distance;
            }

            private double DegreeToRadian(double angle)
            {
                return Math.PI * angle / 180.0;
            }
        }
    }
