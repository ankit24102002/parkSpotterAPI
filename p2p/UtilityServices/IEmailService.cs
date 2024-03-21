using p2p.Common.Models;

namespace p2p.UtilityServices
{
    public interface IEmailServices
    {
        void SendEmail(EmailModel emilModel);
    }
}
