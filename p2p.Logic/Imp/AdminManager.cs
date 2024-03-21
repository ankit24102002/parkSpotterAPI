using p2p.DataAdaptor.Contract;
using p2p.DataAdaptor.Imp;
using p2p.Logic.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Admin;
using static p2p.Common.Models.Space;

namespace p2p.Logic.Imp
{
    public class AdminManager:IAdminManager
    {
        protected readonly IAdminRepository AdminRepository;
        public AdminManager(IAdminRepository adminRepository)
        {
            AdminRepository = adminRepository;
        }

        public List<Cus_and_own_detail> AllUserList(input_1 detail)
        {
            return AdminRepository.AllUserList(detail);
        }

        public ResponseData EnableDisable(ongoning_booking detail)
        {
            return AdminRepository.EnableDisable(detail);
        }
        public ResponseData ContactUs(contactus info)
        {
            return AdminRepository.ContactUs( info);
        }
    }
}
