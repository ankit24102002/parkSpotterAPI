using p2p.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Space;

namespace p2p.Logic.Contract
{
    public interface ICustomerManager
    {
        List<Space_Owner_Master> GetByLocation(string Longitude, string Latitude);
        List<Space_Owner_Master> GetAllSpaces();
        ResponseData UpdateSpace(Space_Owner_Master updatedSpace);
   
        List<Bookingdetail> GetBySpaceID(int SpaceID);
        ResponseData AddBookingDetail(Booking_Detail detail);
    
        List<cur_pas_book> ongoingcustomerbooking(ongoning_booking detail);
        List<lat_long> GetLocationsWithin2km(Location_user detail);
    }
}