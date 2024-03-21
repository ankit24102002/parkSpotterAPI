using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2p.Common.Models
{
    public class Admin
    {
        public class Cus_and_own_detail
        {
            public string Username { get; set; }
            public bool Enable { get; set; }
            public string Phoneno { get; set; }
            public string Email { get; set; }
        }

        public class input_1
        {
            public int Roleid {  get; set; }
        }

        public class contactus
        {
            public string Username { get; set; }
            public string Q_Username { get; set; }
            public string Q_Message { get; set; }
            public string Q_Email { get; set; }
        }
    }
}
