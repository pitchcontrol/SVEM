using System.ServiceModel;

namespace SMEVService.smev
{
    /// <summary>
    /// ������ � �������-���������� �������������� (�����������)
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