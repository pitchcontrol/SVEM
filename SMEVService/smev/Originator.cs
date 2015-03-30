using System.ServiceModel;

namespace SMEVService.smev
{
    [MessageContract]
    public class Originator : OrgExternalType
    {
        public Originator()
        {
        }

        public Originator(string code, string name) : base(code, name)
        {
        }
    }
}