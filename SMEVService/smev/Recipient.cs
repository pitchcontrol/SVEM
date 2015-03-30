using System.ServiceModel;

namespace SMEVService.smev
{
    /// <summary>
    /// Данные о системе-получателе сообщения (Поставщике)
    /// </summary>
    [MessageContract]
    public class Recipient : OrgExternalType
    {
        public Recipient()
        {
        }

        public Recipient(string code, string name) : base(code, name)
        {
        }
    }
}