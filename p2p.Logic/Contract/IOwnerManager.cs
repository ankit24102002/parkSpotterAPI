using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Admin;
using static p2p.Common.Models.Space;

namespace p2p.Logic.Contract
{
    public interface IOwnerManager
    {
        ResponseData DeleteSpace(int spaceid);
        List<cur_pasbooking> GetCurrentbooking(ongoning_booking detail);
        List<current_booking> GetBySpaceIDowner(int SpaceID);
        List<all_spaces> GetAllSpacesbyusername(string username);
        ResponseData Enablespace(int spaceid);
        ResponseData Disablespace(int spaceid);
        ResponseData Addspace(addspace space);
        List<detail_space> OwnerGetSpaceDetail(int spaceid);
        ResponseData UpdateSpace(update_space spacedata);
    }
}
