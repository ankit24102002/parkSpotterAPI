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
    public class OwnerManager : IOwnerManager
    {
        protected readonly IOwnerRepository OwnerRepository;
        public OwnerManager(IOwnerRepository ownerRepository)
        {
            OwnerRepository = ownerRepository;
        }


        public ResponseData DeleteSpace(int spaceid)
        {
            return OwnerRepository.DeleteSpace(spaceid);
        }

        public List<current_booking> GetBySpaceIDowner(int SpaceID)
        {
            return OwnerRepository.GetBySpaceIDowner(SpaceID);
        }

        public List<cur_pasbooking> GetCurrentbooking(ongoning_booking detail)
        {
            return OwnerRepository.GetCurrentbooking(detail);
        }

        public List<all_spaces> GetAllSpacesbyusername(string username)
        {
            return OwnerRepository.GetAllSpacesbyusername(username);
        }

        public ResponseData Enablespace(int spaceid)
        {
            return OwnerRepository.Enablespace(spaceid);
        }

        public ResponseData Disablespace(int spaceid)
        {
            return OwnerRepository.Disablespace(spaceid);
        }

        public ResponseData Addspace(addspace space)
        {
            return OwnerRepository.Addspace(space);
        }
        public List<detail_space> OwnerGetSpaceDetail(int spaceid)
        {
            return OwnerRepository.OwnerGetSpaceDetail(spaceid);
        }

        public ResponseData UpdateSpace(update_space spacedata)
        {
            return OwnerRepository.UpdateSpace( spacedata);
        }
    }

}
