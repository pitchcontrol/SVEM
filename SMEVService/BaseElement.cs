using System.ServiceModel;
using System.Xml.Serialization;

namespace SMEVService
{
    [MessageContract()]
    public class BaseElement
    {
        [XmlNamespaceDeclarations()]
        public XmlSerializerNamespaces xmlsn
        {
            get
            {
                XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                xsn.Add("smev", "http://smev.gosuslugi.ru/rev120315");
                xsn.Add("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                return xsn;
            }
            set
            {
            }
        }
    }
}