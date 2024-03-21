using p2p.DataAdaptor.Contract;
using p2p.DataAdaptor.Imp;
using p2p.Logic.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Space;

namespace p2p.Logic.Imp
{
    public class CustomerManager : ICustomerManager {
        protected readonly ICustomerRepository CustomerRepository;
        public CustomerManager(ICustomerRepository customerRepository)
        {
            CustomerRepository = customerRepository;
        }


        public List<Space_Owner_Master> GetByLocation(string Longitude, string Latitude)
        {
            return CustomerRepository.GetByLocation(Longitude, Latitude);
        }

        public List<Space_Owner_Master> GetAllSpaces()
        {
            return CustomerRepository.GetAllSpaces();
        }

        public ResponseData UpdateSpace(Space_Owner_Master updatedSpace)
        {
            return CustomerRepository.UpdateSpace(updatedSpace);
        }

 
        public List<Bookingdetail> GetBySpaceID(int SpaceID)
        {
            return CustomerRepository.GetBySpaceID( SpaceID);
        }

      public  ResponseData AddBookingDetail(Booking_Detail detail)
        {
            return CustomerRepository.AddBookingDetail( detail);
        }

   
        public List<cur_pas_book> ongoingcustomerbooking(ongoning_booking detail)
        {
        return CustomerRepository.ongoingcustomerbooking( detail);
        }

        public List<lat_long> GetLocationsWithin2km(Location_user detail)
        {
            return CustomerRepository.GetLocationsWithin2km( detail);
        }
    }
}
