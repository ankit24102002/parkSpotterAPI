using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Admin;
using static p2p.Common.Models.Space;

namespace p2p.DataAdaptor.Contract
{
    public interface IAdminRepository
    {
        List<Cus_and_own_detail> AllUserList(input_1 detail);
        ResponseData EnableDisable(ongoning_booking detaile);
        ResponseData ContactUs(contactus info);
    }
}
