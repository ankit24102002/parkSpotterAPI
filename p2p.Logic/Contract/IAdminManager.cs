using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Admin;
using static p2p.Common.Models.Space;

namespace p2p.Logic.Contract
{
    public interface IAdminManager
    {
        List<Cus_and_own_detail> AllUserList(input_1 detail);
        ResponseData EnableDisable(ongoning_booking detail);
        ResponseData ContactUs(contactus info);

    }
}
