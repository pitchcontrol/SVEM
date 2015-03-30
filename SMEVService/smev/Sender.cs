using System.ServiceModel;

namespace SMEVService.smev
{
    /// <summary>
    /// Данные о системе-инициаторе взаимодействия (Потребителе)
    /// </summary>
    [MessageContract]
    public class Sender : OrgExternalType
    {
        public Sender()
        {
        }

        public Sender(string code, string name) : base(code, name)
        {
        }
    }
}