using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Space;

namespace p2p.DataAdaptor.Contract
{
    public interface IOwnerRepository
    {

        ResponseData DeleteSpace(int spaceid);
        List<current_booking> GetBySpaceIDowner(int SpaceID);
        List<cur_pasbooking> GetCurrentbooking(ongoning_booking detail);
        List<all_spaces> GetAllSpacesbyusername(string usernaame);
        ResponseData Enablespace(int spaceid);
        ResponseData Disablespace(int spaceid);
        ResponseData Addspace(addspace space);
        List<detail_space> OwnerGetSpaceDetail(int spaceid);
        ResponseData UpdateSpace(update_space spacedata);
    }
}
